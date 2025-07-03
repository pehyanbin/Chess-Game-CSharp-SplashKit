// Board.cs
using SplashKitSDK;
using System.Collections.Generic;
using System.Linq;
using System;

public class Board
{
    private List<Piece> _pieces;

    public Board()
    {
        _pieces = new List<Piece>();
        SetupPieces();
    }

    public void SetupPieces()
    {
        // Clear existing pieces
        _pieces.Clear();

        // Pawns
        for (int i = 0; i < 8; i++)
        {
            _pieces.Add(new Piece("pawn", Color.White, i, 6));
            _pieces.Add(new Piece("pawn", Color.Black, i, 1));
        }

        // Rooks
        _pieces.Add(new Piece("rook", Color.White, 0, 7));
        _pieces.Add(new Piece("rook", Color.White, 7, 7));
        _pieces.Add(new Piece("rook", Color.Black, 0, 0));
        _pieces.Add(new Piece("rook", Color.Black, 7, 0));

        // Knights
        _pieces.Add(new Piece("knight", Color.White, 1, 7));
        _pieces.Add(new Piece("knight", Color.White, 6, 7));
        _pieces.Add(new Piece("knight", Color.Black, 1, 0));
        _pieces.Add(new Piece("knight", Color.Black, 6, 0));

        // Bishops
        _pieces.Add(new Piece("bishop", Color.White, 2, 7));
        _pieces.Add(new Piece("bishop", Color.White, 5, 7));
        _pieces.Add(new Piece("bishop", Color.Black, 2, 0));
        _pieces.Add(new Piece("bishop", Color.Black, 5, 0));

        // Queens
        _pieces.Add(new Piece("queen", Color.White, 3, 7));
        _pieces.Add(new Piece("queen", Color.Black, 3, 0));

        // Kings
        _pieces.Add(new Piece("king", Color.White, 4, 7));
        _pieces.Add(new Piece("king", Color.Black, 4, 0));
    }

    public void Draw()
    {
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                Color squareColor = ((row + col) % 2 == 0) ? Color.LightGray : Color.DarkGray;
                SplashKit.FillRectangle(squareColor, col * 100, row * 100, 100, 100);
            }
        }

        foreach (Piece piece in _pieces)
        {
            piece.Draw();
        }
    }

    public Piece PieceAt(int col, int row)
    {
        foreach (Piece p in _pieces)
        {
            if (p.X == col && p.Y == row)
                return p;
        }
        return null;
    }

    public bool MovePiece(Piece piece, int toX, int toY)
    {
        if (!piece.IsValidMove(toX, toY, this))
            return false;

        Piece target = PieceAt(toX, toY);
        if (target != null && target.Color != piece.Color)
            _pieces.Remove(target);

        piece.MoveTo(toX, toY);
        return true;
    }

    public Color? CheckForWinner()
    {
        bool whiteKing = false, blackKing = false;
        foreach (Piece p in _pieces)
        {
            if (p.Type == "king")
            {
                if (p.Color == Color.White) whiteKing = true;
                else if (p.Color == Color.Black) blackKing = true;
            }
        }

        if (whiteKing && blackKing) return null;
        if (whiteKing) return Color.White;
        if (blackKing) return Color.Black;
        return null;
    }

    public bool IsPathClear(int fromX, int fromY, int toX, int toY)
    {
        int dx = toX - fromX;
        int dy = toY - fromY;
        int steps = Math.Max(Math.Abs(dx), Math.Abs(dy));
        int stepX = dx == 0 ? 0 : dx / Math.Abs(dx);
        int stepY = dy == 0 ? 0 : dy / Math.Abs(dy);

        // Check all squares between start and end (excluding start and end)
        for (int i = 1; i < steps; i++)
        {
            int checkX = fromX + i * stepX;
            int checkY = fromY + i * stepY;
            if (PieceAt(checkX, checkY) != null)
                return false;
        }
        return true;
    }

    public List<Piece> GetPieces(Color color)
    {
        return _pieces.Where(p => p.Color == color).ToList();
    }
}