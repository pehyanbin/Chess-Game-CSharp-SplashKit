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

    // Move tracking
    public Piece MovedPiece { get; private set; }
    public int MovedFromX { get; private set; }
    public int MovedFromY { get; private set; }
    public int MovedToX { get; private set; }
    public int MovedToY { get; private set; }
    public Piece CapturedPiece { get; private set; }

    public SelectionManager(Board board, Color playerColor)
    {
        _board = board;
        _playerColor = playerColor;
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

            // If no piece is selected, try to select one
            if (_selectedPiece == null)
            {
                Piece clickedPiece = _board.PieceAt(col, row);
                if (clickedPiece != null && clickedPiece.Color == _playerColor)
                {
                    _selectedPiece = clickedPiece;
                    _validMoves = _selectedPiece.GetValidMoves(_board);
                }
            }
            // If a piece is already selected, try to move it
            else
            {
                // Check if clicked position is a valid move
                if (_validMoves.Contains((col, row)))
                {
                    // Remember original position
                    int fromX = _selectedPiece.X;
                    int fromY = _selectedPiece.Y;

                    // Check for capture
                    Piece target = _board.PieceAt(col, row);

                    if (_board.MovePiece(_selectedPiece, col, row))
                    {
                        MoveMade = true;
                        MovedPiece = _selectedPiece;
                        MovedFromX = fromX;
                        MovedFromY = fromY;
                        MovedToX = col;
                        MovedToY = row;
                        CapturedPiece = target;
                    }
                }
                // Deselect if clicked elsewhere
                _selectedPiece = null;
                _validMoves.Clear();
            }
        }
    }

    public void DrawHighlight()
    {
        if (_selectedPiece != null)
        {
            // Highlight selected piece
            SplashKit.FillRectangle(Color.Yellow, _selectedPiece.X * 100, _selectedPiece.Y * 100, 100, 100);

            // Draw piece on top of highlight
            _selectedPiece.Draw();

            // Draw valid moves
            foreach (var move in _validMoves)
            {
                SplashKit.FillCircle(Color.Green, move.x * 100 + 50, move.y * 100 + 50, 10);
            }
        }
    }
}