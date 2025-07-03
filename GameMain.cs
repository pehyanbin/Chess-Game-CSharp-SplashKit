// GameMain.cs
using SplashKitSDK;

public class GameMain
{
    private GameMenu _menu;
    private ChessGame _chessGame;

    public void Run()
    {
        SplashKit.OpenWindow("2D Chess Game", 800, 780);

        // Load chess piece images
        ChessImages.LoadImages();

        _menu = new GameMenu();
        var result = _menu.Show();
        GameMode mode = result.mode;
        string whitePlayer = result.whitePlayer;
        string blackPlayer = result.blackPlayer;

        _chessGame = new ChessGame(mode, whitePlayer, blackPlayer);
        _chessGame.Start();

        SplashKit.CloseWindow("2D Chess Game");
    }
}