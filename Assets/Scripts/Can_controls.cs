using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Can_controls : MonoBehaviour
{

    PlayerInput controls;

    private Vector2 pointerPosition;
    private Vector3 aim;

    [SerializeField] Projectile projectilePrefab;

    [SerializeField] float firerate = 2f;

    [SerializeField] Can_Manager levelManager;

   
    bool canFire = true;
  
    private GameObject gun;



    private void Awake()
    {
        controls = new PlayerInput();
        controls.Main.Click.performed += ctx => Fire();
        controls.Main.PointerPosition.performed += ctx => UpdateCursorPos(ctx.ReadValue<Vector2>());
    }


    // Start is called before the first frame update
    void Start()
    {

        gun = transform.GetChild(1).gameObject;
        
    }

    // Update is called once per frame
    void Update()
    {


    
        
    }


    private void LateUpdate()
    {
        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, -10, 10);
        pos.y = Mathf.Clamp(pos.y, 5, 15);

        transform.position = pos;
    }




    void Fire()
    {
        if (levelManager.isCharged)
        {
            Debug.Log("Boom!");

        }
        else if (canFire)
        {

            Projectile projectile = Instantiate(projectilePrefab) as Projectile;
            projectile.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
            projectile.transform.rotation = transform.rotation;
            projectile.transform.LookAt(aim);

            projectile.levelManager = levelManager;

            StartCoroutine(FireTimer());
        }

    }


    void UpdateCursorPos(Vector2 cursorPos)
    {
        pointerPosition = cursorPos;

        aim = new Vector3(pointerPosition.x, pointerPosition.y, 10);
        aim = Camera.main.ScreenToWorldPoint(aim);

        //gun.transform.LookAt(aim);

    }


    IEnumerator FireTimer()
    {
        canFire = false;

        yield return new WaitForSeconds(1 / firerate);

        canFire = true;
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
