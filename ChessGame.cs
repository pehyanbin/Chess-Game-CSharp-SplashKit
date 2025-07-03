// ChessGame.cs
using SplashKitSDK;

public class ChessGame
{
    private Board _board;
    private Player _whitePlayer;
    private Player _blackPlayer;
    private GameMode _mode;
    private bool _isWhiteTurn;
    private Color? _winner = null;

    public ChessGame(GameMode mode)
    {
        _mode = mode;
        _board = new Board();

        _whitePlayer = new Player(Color.White, _board);

        if (_mode == GameMode.TwoPlayer)
            _blackPlayer = new Player(Color.Black, _board);
        else
            _blackPlayer = new AIPlayer(Color.Black, _board);

        _isWhiteTurn = true;
    }

    public void Start()
    {
        while (!SplashKit.WindowCloseRequested("2D Chess Game"))
        {
            SplashKit.ProcessEvents();
            SplashKit.ClearScreen(Color.White);

            _board.Draw();

            if (_winner == null)
            {
                if (_isWhiteTurn)
                {
                    if (_whitePlayer.HandleTurn())
                    {
                        _winner = _board.CheckForWinner();
                        if (_winner == null)
                            _isWhiteTurn = !_isWhiteTurn;
                    }
                }
                else
                {
                    if (_blackPlayer.HandleTurn())
                    {
                        _winner = _board.CheckForWinner();
                        if (_winner == null)
                            _isWhiteTurn = !_isWhiteTurn;
                    }
                }
            }
            else
            {
                SplashKit.DrawText((_winner == Color.White ? "White" : "Black") + " wins!",
                    Color.Red, "Arial", 32, 300, 750);
            }

            SplashKit.RefreshScreen(60);
        }
    }
}