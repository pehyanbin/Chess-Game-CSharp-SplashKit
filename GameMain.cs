// GameMain.cs (UPDATED)
using SplashKitSDK;

public class GameMain
{
    private GameMenu _menu;
    private ChessGame _chessGame;

    public void Run()
    {
        SplashKit.OpenWindow("2D Chess Game", 800, 800);

        // Load chess piece images
        ChessImages.LoadImages();

        _menu = new GameMenu();
        GameMode mode = _menu.Show();

        _chessGame = new ChessGame(mode);
        _chessGame.Start();

        SplashKit.CloseWindow("2D Chess Game");
    }
}