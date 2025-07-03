// Piece.cs
using SplashKitSDK;
using System;
using System.Collections.Generic;

public class Piece
{
    public string Type { get; }
    public Color Color { get; }
    public int X { get; private set; }
    public int Y { get; private set; }
    public bool HasMoved { get; private set; }

    public Piece(string type, Color color, int x, int y)
    {
        Type = type.ToLower();
        Color = color;
        X = x;
        Y = y;
        HasMoved = false;
    }

    public void MoveTo(int x, int y)
    {
        X = x;
        Y = y;
        HasMoved = true;
    }

    public void Draw()
    {
        Bitmap pieceImage = ChessImages.GetImage(Type, Color);
        if (pieceImage != null)
        {
            // Center the image in the square
            float drawX = X * 100 + (100 - pieceImage.Width) / 2;
            float drawY = Y * 100 + (100 - pieceImage.Height) / 2;
            SplashKit.DrawBitmap(pieceImage, drawX, drawY);
        }
        else
        {
            // Fallback to text if image not found
            string label = (Type == "knight" ? "N" : Type[0].ToString()).ToUpper();
            SplashKit.DrawText(label, Color == Color.White ? Color.White : Color.Black,
                "Arial", 36, X * 100 + 35, Y * 100 + 30);
        }
    }

    public bool IsValidMove(int toX, int toY, Board board)
    {
        // Can't move to the same square
        if (X == toX && Y == toY) return false;

        Piece target = board.PieceAt(toX, toY);
        // Can't capture own pieces
        if (target != null && target.Color == Color) return false;

        int dx = toX - X;
        int dy = toY - Y;
        int absDx = Math.Abs(dx);
        int absDy = Math.Abs(dy);

        switch (Type)
        {
            case "pawn":
                int forward = Color == Color.White ? -1 : 1;

                // Forward move
                if (dx == 0 && dy == forward && target == null)
                    return true;

                // First double move
                if (dx == 0 && !HasMoved && dy == 2 * forward &&
                    target == null && board.PieceAt(X, Y + forward) == null)
                    return true;

                // Capture
                if (absDx == 1 && dy == forward && target != null && target.Color != Color)
                    return true;

                return false;

            case "rook":
                if (dx != 0 && dy != 0) return false;
                return board.IsPathClear(X, Y, toX, toY);

            case "bishop":
                if (absDx != absDy) return false;
                return board.IsPathClear(X, Y, toX, toY);

            case "queen":
                if (dx != 0 && dy != 0 && absDx != absDy) return false;
                return board.IsPathClear(X, Y, toX, toY);

            case "king":
                return absDx <= 1 && absDy <= 1;

            case "knight":
                return (absDx == 2 && absDy == 1) || (absDx == 1 && absDy == 2);
        }

        return false;
    }

    public List<(int x, int y)> GetValidMoves(Board board)
    {
        List<(int x, int y)> moves = new List<(int, int)>();

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (IsValidMove(x, y, board))
                    moves.Add((x, y));
            }
        }
        return moves;
    }
}