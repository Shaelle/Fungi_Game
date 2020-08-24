using System;

public class Button : GenericSingletonClass<Button>{
    public Action StartGame = delegate {};
    public Action RestartGame = delegate {};

    public void Restart() {
        RestartGame();
    }

    public void FirstStart() {
        StartGame();
    }
}
