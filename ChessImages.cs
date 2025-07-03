// ChessImages.cs (NEW FILE)
using SplashKitSDK;
using System.Collections.Generic;

public static class ChessImages
{
    private static Dictionary<string, Bitmap> _images = new Dictionary<string, Bitmap>();

    public static void LoadImages()
    {
        // White pieces
        _images["white_pawn"] = SplashKit.LoadBitmap("white_pawn", "white_pawn.png");
        _images["white_rook"] = SplashKit.LoadBitmap("white_rook", "white_rook.png");
        _images["white_knight"] = SplashKit.LoadBitmap("white_knight", "white_knight.png");
        _images["white_bishop"] = SplashKit.LoadBitmap("white_bishop", "white_bishop.png");
        _images["white_queen"] = SplashKit.LoadBitmap("white_queen", "white_queen.png");
        _images["white_king"] = SplashKit.LoadBitmap("white_king", "white_king.png");
        
        // Black pieces
        _images["black_pawn"] = SplashKit.LoadBitmap("black_pawn", "black_pawn.png");
        _images["black_rook"] = SplashKit.LoadBitmap("black_rook", "black_rook.png");
        _images["black_knight"] = SplashKit.LoadBitmap("black_knight", "black_knight.png");
        _images["black_bishop"] = SplashKit.LoadBitmap("black_bishop", "black_bishop.png");
        _images["black_queen"] = SplashKit.LoadBitmap("black_queen", "black_queen.png");
        _images["black_king"] = SplashKit.LoadBitmap("black_king", "black_king.png");
    }

    public static Bitmap GetImage(string pieceType, Color color)
    {
        string key = (color == Color.White ? "white" : "black") + "_" + pieceType;
        if (_images.ContainsKey(key))
            return _images[key];
        return null;
    }
}