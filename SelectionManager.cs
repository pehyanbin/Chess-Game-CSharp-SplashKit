// SelectionManager.cs
using SplashKitSDK;
using System.Collections.Generic;

public class SelectionManager
{
    private Piece _selectedPiece;
    private Board _board;
    private Color _playerColor;
    public bool MoveMade { get; private set; }
    private List<(int x, int y)> _validMoves = new List<(int, int)>();
    private ChessGame _game;

    public Piece MovedPiece { get; private set; }
    public int MovedFromX { get; private set; }
    public int MovedFromY { get; private set; }
    public int MovedToX { get; private set; }
    public int MovedToY { get; private set; }
    public Piece CapturedPiece { get; private set; }
    public int MovedOriginalMoveCount { get; private set; }

    public SelectionManager(Board board, Color playerColor, ChessGame game)
    {
        _board = board;
        _playerColor = playerColor;
        _game = game;
    }

    public void Update()
    {
        MoveMade = false;
        MovedPiece = null;
        CapturedPiece = null;

        if (SplashKit.MouseClicked(MouseButton.LeftButton))
        {
            int col = (int)(SplashKit.MouseX() / 100);
            int row = (int)(SplashKit.MouseY() / 100);

            if (_selectedPiece == null)
            {
                Piece clickedPiece = _board.PieceAt(col, row);
                if (clickedPiece != null && clickedPiece.Color == _playerColor)
                {
                    _selectedPiece = clickedPiece;
                    _validMoves = _selectedPiece.GetValidMoves(_board);
                }
            }
            else
            {
                if (_validMoves.Contains((col, row)))
                {
                    int fromX = _selectedPiece.X;
                    int fromY = _selectedPiece.Y;
                    int originalMoveCount = _selectedPiece.MoveCount;

                    if (_board.MovePiece(_selectedPiece, col, row, out Piece captured))
                    {
                        MoveMade = true;
                        MovedPiece = _selectedPiece;
                        MovedFromX = fromX;
                        MovedFromY = fromY;
                        MovedToX = col;
                        MovedToY = row;
                        CapturedPiece = captured;
                        MovedOriginalMoveCount = originalMoveCount;

                        if (_selectedPiece.Type == "pawn" && (row == 0 || row == 7))
                        {
                            _game.SetPromotingPawn(_selectedPiece);
                        }
                    }
                }
                _selectedPiece = null;
                _validMoves.Clear();
            }
        }
    }

    public void DrawHighlight()
    {
        if (_selectedPiece != null)
        {
            SplashKit.FillRectangle(Color.Yellow, _selectedPiece.X * 100, _selectedPiece.Y * 100, 100, 100);
            _selectedPiece.Draw();

            foreach (var move in _validMoves)
            {
                SplashKit.FillCircle(Color.Green, move.x * 100 + 50, move.y * 100 + 50, 10);
            }
        }
    }
}