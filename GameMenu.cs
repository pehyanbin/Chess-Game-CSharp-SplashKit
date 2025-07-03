// GameMenu.cs
using SplashKitSDK;

public enum GameMode
{
    TwoPlayer,
    VsAI
}

public class GameMenu
{
    public GameMode Show()
    {
        while (!SplashKit.WindowCloseRequested("2D Chess Game"))
        {
            SplashKit.ProcessEvents();
            SplashKit.ClearScreen(Color.White);
            SplashKit.DrawText("Welcome to Chess", Color.Black, "Arial", 32, 250, 100);
            SplashKit.DrawText("Press 1 for 2-Player Mode", Color.Black, "Arial", 24, 250, 200);
            SplashKit.DrawText("Press 2 for Play vs AI", Color.Black, "Arial", 24, 250, 250);
            SplashKit.RefreshScreen();

            if (SplashKit.KeyTyped(KeyCode.Num1Key)) return GameMode.TwoPlayer;
            if (SplashKit.KeyTyped(KeyCode.Num2Key)) return GameMode.VsAI;
        }

        return GameMode.TwoPlayer;
    }
}