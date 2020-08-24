using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class BlockSpawn : MonoBehaviour{
    [SerializeField] private Color[]  poletteRandomColor; //массив для палитры начальных цветов
    [SerializeField] private RawImage backGround;         // обьект заднего фона

    [SerializeField] private float speedBlock         = 2f;   // скорость для блока
    [SerializeField] private float rangeAccurateBlock = 0.5f; //  диапазон для точного попадания блока

    [SerializeField] private float rangeAnimAxis = 10f; // растояние от центра по осям для движения зацикленого блока 

    [SerializeField] private int        setSuccessBlock = 1; // счетчик удачных блоков
    [SerializeField] private GameObject assignedBlock;       //  назначенный блок для иницилизации предыдущего
    [SerializeField] private Camera     myCamer;

    [SerializeField] private Text record; //  цифра рекорда на канвасе
    [SerializeField] private Text countBlockText; // вывод счетчика на экран

    [SerializeField] private GameObject buttonStart;   // кнопка для старта
    [SerializeField] private GameObject buttonRestart; // кнопка для рестарта
    [SerializeField] private GameObject restartCanvas; // канвас который должен появится после гейм овера
    [SerializeField] private GameObject startCanvas;   // канвас который должен появится при старте
    [SerializeField] private GameObject stand;         // стенд для раскраски 
    [SerializeField] private GameObject fireWork; // эффект попадания блоков

    private float randomColorForBlock;      // переменная для выбора начального цвета блока
    private float randomColorForBackGround; // переменная для выбора начального цвета заднего фона
    private float startBlockCount; 
    private int   successBlockCount;

    private GameObject currentBlock;  // текущий блок
    private GameObject previousBlock; // предыдйщий блок

    private Vector3 currentBlockScale;  // скейл текущего блока
    private Vector3 previousBlockScale; // скейл предыдущего блока

    private int stopBlockId;

    private int  recordTemp;
    private bool directionX = true; // текущее направление по оси х / если фолс то это по z

    private float endValue;
    private int   currentBlockCount; //счетчик текущего блока


    PlayerInput controls;


    private void Awake()
    {
        controls = new PlayerInput();
        controls.Main.Click.performed += ctx => NextTap();
    }

    private void Start() {
        //GameManager.Instance.stopMouse = false;
        currentBlockCount              = 0; // сброс счетчика для рестарта 
        previousBlock = assignedBlock;    // назначили сразу предыдцщий блок - для первого
        currentBlockScale = assignedBlock.transform.localScale; // получаили начальный скейл блока
        CanvasStartControlVisible();
        ColorStartManager();
        AudioManager.Instance.SoundStartRound();
        //GameManager.Instance.MouseControl += NextTap;
        Button.Instance.StartGame += firstStart;
        LoadRecord();
    }

    private void ColorStartManager()
    {
        randomColorForBlock = Random.Range(0f, poletteRandomColor.Length - 1f); //   записать начальный рандомный цвет блоку
        randomColorForBackGround = Random.Range(0f, poletteRandomColor.Length - 1f); //   записать рандомный цвет заднему фону
        stand.GetComponent<MeshRenderer>().material.color = SetColorShade(randomColorForBlock);
        assignedBlock.GetComponent<MeshRenderer>().material.color = SetColorShade(randomColorForBlock);
        backGround.color = SetColorShade(randomColorForBackGround);
    }
    private void CanvasStartControlVisible()
    {
        countBlockText.gameObject.SetActive(false);
        startCanvas.gameObject.SetActive(true);
        record.gameObject.SetActive(true);
        buttonStart.gameObject.SetActive(true);
        buttonRestart.gameObject.SetActive(false);
    }
    public void firstStart() {
        countBlockText.gameObject.SetActive(true);
        startCanvas.gameObject.SetActive(false);
        record.gameObject.SetActive(false);
        buttonStart.gameObject.SetActive(false);
        StartCoroutine(SpawnBlockInPosition());
        GameManager.Instance.stopMouse = true;
    }
    public void RestartButton() {
        recordTemp  = currentBlockCount;
        record.text = recordTemp.ToString();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    IEnumerator SpawnBlockInPosition() {
        MoveCamera();
        Vector3 setPosition = new Vector3();
        Vector3 destination = new Vector3();

        if(directionX) {
            // вычесляем позицию спауна  по х
            setPosition.x = -rangeAnimAxis + previousBlock.transform.position.x;
            setPosition.y = currentBlockCount; // позиция равна высоте текущего блока 
            setPosition.z = previousBlock.transform.position.z;
            destination   = setPosition;
            destination.x = rangeAnimAxis + previousBlock.transform.position.x;
        }
        else {
            // вычесляем позицию спауна по Z
            setPosition.x = previousBlock.transform.position.x;
            setPosition.y = currentBlockCount;
            setPosition.z = -rangeAnimAxis + previousBlock.transform.position.z;

            destination   = setPosition;
            destination.z = rangeAnimAxis + previousBlock.transform.position.z;
        }
           // метод для спауна на вход текущий скейл предыдущего блока и позиция спауна
        currentBlock = SpawnBlock(currentBlockScale, setPosition); 
        countBlockText.text = currentBlockCount.ToString();
        currentBlockCount++; //  +1 к текущему блоку
        stopBlockId = LeanTween.move(currentBlock, destination, speedBlock).setLoopPingPong().id; // движение блока туда сюда
        CorrectSpeed();
        yield return null;
    }
    private void CorrectSpeed() {
        if(speedBlock > 1.3f) {
            speedBlock -= 0.02f;
        }
    }
    
    // спаун блока на вход текущий скейл от предыдущего и позиция где спаунить
    GameObject SpawnBlock(Vector3 originalScale, Vector3 spawnPosition)
    {
        GameObject block = GameObject.CreatePrimitive(PrimitiveType.Cube);
        block.transform.localScale                        = originalScale; // скейл от оригинала
        block.transform.position                          = spawnPosition; // позиция куда заспаунили
        block.transform.parent                            = transform;     // назначить паренту 
        block.GetComponent<MeshRenderer>().material.color = SetColorShade(randomColorForBlock);
        return block;
    }
    GameObject CutOutOfBlock(GameObject originalBlock, Vector3 previusBlockForCut) {
        GameObject cutBlock = Instantiate(originalBlock, transform); // создали вырезаный блок  

        Vector3 originalBlockPos = originalBlock.transform.position;   // записали позицию вырезаного  блока 
        Vector3 originalScale    = originalBlock.transform.localScale; // записали скейл  текущего блока

        // вычесляем дистанция без учета y
        Vector2 originalPosForDistance = new Vector2(originalBlockPos.x,   originalBlockPos.z);
        Vector2 previusPosForDistance  = new Vector2(previusBlockForCut.x, previusBlockForCut.z);
        float   distance               = Vector2.Distance(originalPosForDistance, previusPosForDistance);
        // если текущее направление равно направленю х то  иначе делаем для z
        if(directionX) {
            // позиция для х для смещения оригинала на кусок отрезка
            float positionForX = (previusBlockForCut.x + originalBlockPos.x) / 2f;
            originalBlock.transform.position = new Vector3(positionForX, originalBlockPos.y, originalBlockPos.z);
            originalBlock.transform.localScale = new Vector3(originalScale.x - distance, originalScale.y, originalScale.z);
         
            int scaleDirectionX;    // направление скейла

            if(originalBlockPos.x - previusBlockForCut.x > 0) {
                scaleDirectionX = 1;
            }
            else {
                scaleDirectionX = -1;
            }

            // новая позиция и скейл для вырезого блока
            float posX = ((previusBlockForCut.x + originalBlockPos.x) / 2f + originalScale.x * scaleDirectionX / 2f);
            cutBlock.transform.position = new Vector3(posX, originalBlockPos.y,  originalBlockPos.z);
            cutBlock.transform.localScale = new Vector3(distance, originalScale.y, originalScale.z);
        }

        else
        {
            float positionForZ = (previusBlockForCut.z + originalBlockPos.z) /2f;
            originalBlock.transform.position = new Vector3(originalBlockPos.x, originalBlockPos.y, positionForZ);
            originalBlock.transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z - distance);

            int scaleDirectionZ;

            if(originalBlockPos.z - previusBlockForCut.z > 0) {
                scaleDirectionZ = 1;
            }
            else {
                scaleDirectionZ = -1;
            }

            float posZ = (previusBlockForCut.z + originalBlockPos.z) /2f + originalScale.z * scaleDirectionZ /2f; 
            cutBlock.transform.position = new Vector3(originalBlockPos.x, originalBlockPos.y, posZ);
            cutBlock.transform.localScale = new Vector3(originalScale.x, originalScale.y, distance);
        }

        cutBlock.AddComponent<Rigidbody>().mass = 200f; // добавили физику блоку во время игры (отрезаному)
        return originalBlock;                           // вернули блок после созданию со скйлом и размерами 
    }
    private void NextTap() {
        if(!GameManager.Instance.stopMouse) {
            return;
        }

        LeanTween.cancel(stopBlockId);            // кансел зациклегого блока
        BlockVerify(currentBlock, previousBlock); // проверка куда попал блок и переключение в нужный метод
    }

    private void SuccessBlock() {
        AudioManager.Instance.SoundSuccessBlock();
        successBlockCount++; // добавить + удачный блок 
        float posX = previousBlock.transform.position.x;
        float posY = currentBlock.transform.position.y;
        float posZ = previousBlock.transform.position.z;
        Vector3 successBlock = new Vector3(posX, posY, posZ);
        currentBlock.transform.position = successBlock; // установить ровно в том место как и  предущий  блок но +1 по Y
        Instantiate(fireWork, currentBlock.transform.position, Quaternion.identity);

        if(successBlockCount >= setSuccessBlock) {
            AudioManager.Instance.SoundLuckyBlock();
         Vector3 successBlockScale = currentBlock.transform.localScale; // переменная для скейла по нужной оси

            if(directionX) {
                successBlockScale.x = currentBlock.transform.localScale.x + 0.5f; // делаем скейл по  оси х, текущий + 0.5
            }
            else {
                successBlockScale.z = currentBlock.transform.localScale.z + 0.5f;
            }

            currentBlockScale = successBlockScale;
        }

        if(directionX) {
            directionX = false;
        }
        else {
            directionX = true;
        }
        StartCoroutine(SpawnBlockInPosition());
    }
    private void FailBlock() {
        currentBlock.AddComponent<Rigidbody>();
        GameManager.Instance.EndGame += ReloadGame;
        GameManager.Instance.GameOver();
    }
    private void ReloadGame() {
        SaveRecord();
        SetValueCamTower();
        StartCoroutine(ShowTower());
    }
    private void SaveRecord() {
        recordTemp = currentBlockCount;
        int currentRecord = int.Parse(record.text);
        if(recordTemp > currentRecord) {
            PlayerPrefs.SetInt("record", recordTemp - 1);
            PlayerPrefs.Save();
        }
    }
    private void LoadRecord() {
        int sRecord = PlayerPrefs.GetInt("record");
        record.text = sRecord.ToString();
    }
    IEnumerator ShowTower() {
        yield return new WaitForSeconds(1f);
        float currenValue = myCamer.orthographicSize;
        float elapsedTime = 0;

        while(elapsedTime < 1) {
            elapsedTime              += Time.deltaTime;
            myCamer.orthographicSize =  Mathf.Lerp(currenValue, endValue, elapsedTime / 1);
            yield return null;
        }

        CanvasEndGame();

        yield return null;
    }
    private void CanvasEndGame() {
        restartCanvas.gameObject.SetActive(true);
        buttonRestart.gameObject.SetActive(true);
    }
    private void NormalBlock() {
        AudioManager.Instance.SoundNormalBlock();
        successBlockCount = 0; // сбросить перменную на ноль
        // отрезание блока  на вход текущий блок и предыдущий
        currentBlock = CutOutOfBlock(currentBlock, previousBlock.transform.position);  
        currentBlockScale = currentBlock.transform.localScale; // записать в перменнную текущий скейл
        previousBlock     = currentBlock;

        if(directionX) {
            directionX = false;
        }
        else {
            directionX = true;
        }

        StartCoroutine(SpawnBlockInPosition());
    }

    void MoveCamera() {
        if(currentBlockCount > 3) {
            LeanTween.moveY(Camera.main.gameObject, currentBlockCount + 14, 1f);
        }
    }

    private void
        BlockVerify(GameObject currentBlocks, GameObject previousBlocks)  // верификация блока 
    {
        float distance;
        if(directionX) {
            distance = Mathf.Abs(currentBlocks.transform.position.x - previousBlocks.transform.position.x);
        }
        else {
            distance = Mathf.Abs(currentBlocks.transform.position.z - previousBlocks.transform.position.z);
        }

        if(distance < rangeAccurateBlock) // диапазон для проверки попал блок в центр или нет 
        {
            SuccessBlock();
        }
        else {
            float originalScale;
            if(directionX) {
                originalScale = currentBlocks.transform.localScale.x; // скейл текущего блока по х
            }
            else {
                originalScale = currentBlocks.transform.localScale.z; // скейл текущего блока по z
            }

            if(originalScale > distance)
            // если текущий скейл больше дистанции предыдущего блока  значит блок попдает в предыдущий 
            {
                NormalBlock();
            }
            // иначе  дистанция больше значения скейла  и блок не попадает.
            else
                FailBlock();
        }
    }
    Color SetColorShade(float offset) {  // установить смещение оттенков для текущего цвета
        float setColor = ((currentBlockCount + poletteRandomColor.Length) * 0.2f + offset) % poletteRandomColor.Length;
        int startcolor = (int) setColor;
        int endColor   = (startcolor + 1) % poletteRandomColor.Length;
        float middleValue = setColor - startcolor;
        return Color.Lerp(poletteRandomColor[startcolor], poletteRandomColor[endColor], middleValue);
    }
    //TODO  найти решение  для показа башни более оптимальное
    private void SetValueCamTower() {
        if(currentBlockCount >= 1) {
            endValue = 9;
        }

        if(currentBlockCount > 10) {
            endValue = 20;
        }

        if(currentBlockCount > 20) {
            endValue = 40;
        }

        if(currentBlockCount > 40) {
            endValue = 60;
        }

        if(currentBlockCount > 60) {
            endValue = 70;
        }

        if(currentBlockCount > 70) {
            endValue = 90;
        }

        if(currentBlockCount > 100) {
            endValue = 100;
        }

        if(currentBlockCount > 150) {
            endValue = 120;
        }

        if(currentBlockCount > 200) {
            endValue = 150;
        }
    }


    private void OnEnable()
    {
        controls.Enable();
    }


    private void OnDisable()
    {
        controls.Disable();
    }
}
