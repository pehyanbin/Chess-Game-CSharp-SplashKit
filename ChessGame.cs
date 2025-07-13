// ChessGame.cs
using SplashKitSDK;
using System;
using System.IO;

public class ChessGame
{
    private Board _board;
    private Player _whitePlayer;
    private Player _blackPlayer;
    private GameMode _mode;
    private bool _isWhiteTurn;
    private Color? _winner = null;
    private DateTime? _gameStartTime = null;
    private DateTime? _gameEndTime = null;
    private string _whitePlayerName;
    private string _blackPlayerName;
    private string _moveHistory = "";

    // Promotion state
    private Piece _promotingPawn = null;
    private string[] _promotionOptions = new[] { "queen", "rook", "bishop", "knight" };

    public ChessGame(GameMode mode, string whitePlayerName, string blackPlayerName)
    {
        _mode = mode;
        _board = new Board();
        _whitePlayerName = whitePlayerName;
        _blackPlayerName = blackPlayerName;

        _whitePlayer = new Player(Color.White, _board, whitePlayerName, this);

        if (_mode == GameMode.TwoPlayer)
            _blackPlayer = new Player(Color.Black, _board, blackPlayerName, this);
        else
            _blackPlayer = new AIPlayer(Color.Black, _board, "AI Player", this);

        _isWhiteTurn = true;
    }

    public void Start()
    {
        while (!SplashKit.WindowCloseRequested("2D Chess Game"))
        {
            SplashKit.ProcessEvents();
            SplashKit.ClearScreen(Color.White);

            _board.Draw();

            // Display player names and turn indicator
            SplashKit.DrawText($"{_whitePlayerName}'s turn",
                _isWhiteTurn ? Color.Blue : Color.LightGray,
                "Arial", 20, 20, 750);

            SplashKit.DrawText($"{_blackPlayerName}'s turn",
                !_isWhiteTurn ? Color.Blue : Color.LightGray,
                "Arial", 20, 20, 760);

            if (_promotingPawn != null)
            {
                DrawPromotionMenu();
                HandlePromotionSelection();
            }
            else if (_winner == null)
            {
                if (_isWhiteTurn)
                {
                    if (_whitePlayer.HandleTurn())
                    {
                        if (_gameStartTime == null)
                            _gameStartTime = DateTime.Now;

                        RecordMove(_whitePlayer.Name, _whitePlayer.LastMove);
                        _winner = _board.CheckForWinner();
                        if (_winner == null)
                            _isWhiteTurn = !_isWhiteTurn;
                    }
                }
                else
                {
                    if (_blackPlayer.HandleTurn())
                    {
                        RecordMove(_blackPlayer.Name, _blackPlayer.LastMove);
                        _winner = _board.CheckForWinner();
                        if (_winner == null)
                            _isWhiteTurn = !_isWhiteTurn;
                    }
                }
            }
            else
            {
                if (_gameEndTime == null)
                {
                    _gameEndTime = DateTime.Now;
                    SaveGameRecord();
                }

                string winnerName = _winner == Color.White ? _whitePlayerName : _blackPlayerName;
                SplashKit.DrawText($"{winnerName} wins!",
                    Color.Red, "Arial", 32, 300, 750);

                if (_gameStartTime.HasValue && _gameEndTime.HasValue)
                {
                    TimeSpan duration = _gameEndTime.Value - _gameStartTime.Value;
                    SplashKit.DrawText($"Game time: {duration.TotalSeconds:F1} seconds",
                        Color.Green, "Arial", 20, 600, 750);
                }
            }

            SplashKit.RefreshScreen(60);
        }
    }

    private void DrawPromotionMenu()
    {
        int startX = 300;
        int startY = 300;
        int optionSize = 50;

        SplashKit.FillRectangle(Color.White, startX - 10, startY - 30,
            _promotionOptions.Length * optionSize + 20, optionSize + 40);
        SplashKit.DrawText("Promote to:", Color.Black, "Arial", 20, startX, startY - 25);

        for (int i = 0; i < _promotionOptions.Length; i++)
        {
            string pieceType = _promotionOptions[i];
            Bitmap img = ChessImages.GetImage(pieceType, _promotingPawn.Color);
            SplashKit.DrawBitmap(img, startX + i * optionSize, startY);
        }
    }

    private void HandlePromotionSelection()
    {
        if (SplashKit.MouseClicked(MouseButton.LeftButton))
        {
            float mouseX = SplashKit.MouseX();
            float mouseY = SplashKit.MouseY();

            int startX = 300;
            int startY = 300;
            int optionSize = 50;

            for (int i = 0; i < _promotionOptions.Length; i++)
            {
                if (mouseX >= startX + i * optionSize && mouseX <= startX + (i + 1) * optionSize &&
                    mouseY >= startY && mouseY <= startY + optionSize)
                {
                    _board.PromotePawn(_promotingPawn, _promotionOptions[i]);
                    _promotingPawn = null;
                    break;
                }
            }
        }
    }

    public void SetPromotingPawn(Piece pawn)
    {
        _promotingPawn = pawn;
    }

    private void RecordMove(string playerName, Move move)
    {
        if (move.IsValid)
        {
            string pieceName = move.Piece.Type;
            if (pieceName == "knight") pieceName = "N";
            else pieceName = pieceName.Substring(0, 1).ToUpper();

            string moveRecord = $"{playerName}: {pieceName} from {move.FromX},{move.FromY} to {move.ToX},{move.ToY}";
            if (move.Capture) moveRecord += " (capture)";
            if (move.IsCastling) moveRecord += " (castling)";
            if (move.IsPromotion) moveRecord += $" (promoted to {move.PromotionType})";

            Console.WriteLine(moveRecord);
            _moveHistory += moveRecord + Environment.NewLine;
        }
    }

    private void SaveGameRecord()
    {
        if (!_gameStartTime.HasValue || !_gameEndTime.HasValue || _winner == null)
            return;

        try
        {
            string winnerName = _winner == Color.White ? _whitePlayerName : _blackPlayerName;
            TimeSpan duration = _gameEndTime.Value - _gameStartTime.Value;

            string fileName = $"ChessGame_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine($"Chess Game - {DateTime.Now}");
                writer.WriteLine($"White Player: {_whitePlayerName}");
                writer.WriteLine($"Black Player: {_blackPlayerName}");
                writer.WriteLine($"Winner: {winnerName}");
                writer.WriteLine($"Game Duration: {duration.TotalSeconds:F1} seconds");
                writer.WriteLine();
                writer.WriteLine("Move History:");
                writer.WriteLine(_moveHistory);
            }
            Console.WriteLine($"Game record saved to {fileName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving game record: {ex.Message}");
        }
    }
}