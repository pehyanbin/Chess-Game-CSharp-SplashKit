// Piece.cs
using SplashKitSDK;
using System;
using System.Collections.Generic;

public class Piece
{
    public string Type { get; }
    public Color Color { get; }
    public int X { get; set; }
    public int Y { get; set; }
    public bool HasMoved { get; set; }

    public Piece(string type, Color color, int x, int y)
    {
        Type = type.ToLower();
        Color = color;
        X = x;
        Y = y;
        HasMoved = false;
    }

    public void Draw()
    {
        Bitmap pieceImage = ChessImages.GetImage(Type, Color);
        if (pieceImage != null)
        {
            float drawX = X * 100 + (100 - pieceImage.Width) / 2;
            float drawY = Y * 100 + (100 - pieceImage.Height) / 2;
            SplashKit.DrawBitmap(pieceImage, drawX, drawY);
        }
        else
        {
            string label = (Type == "knight" ? "N" : Type[0].ToString()).ToUpper();
            SplashKit.DrawText(label, Color == Color.White ? Color.White : Color.Black,
                "Arial", 36, X * 100 + 35, Y * 100 + 30);
        }
    }

    public bool IsValidMove(int toX, int toY, Board board)
    {
        if (X == toX && Y == toY) return false;

        Piece target = board.PieceAt(toX, toY);
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
                // Normal king move
                if (absDx <= 1 && absDy <= 1)
                    return true;

                // Castling check
                if (!HasMoved && absDy == 0 && absDx == 2 && Y == (Color == Color.White ? 7 : 0))
                {
                    int rookX = toX > X ? 7 : 0;
                    Piece rook = board.PieceAt(rookX, Y);

                    return rook != null && rook.Type == "rook" && !rook.HasMoved &&
                           board.IsPathClear(X, Y, rookX, Y);
                }
                return false;

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