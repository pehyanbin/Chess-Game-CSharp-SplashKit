// Move.cs
public struct Move
{
    public Piece Piece;
    public int FromX;
    public int FromY;
    public int ToX;
    public int ToY;
    public bool Capture;
    public Piece CapturedPiece;
    public bool IsValid;
    public bool IsCastling;
    public bool IsPromotion;
    public string PromotionType;
    public Piece OriginalPiece;
    public bool OriginalHasMovedState;

    public Move(Piece piece, int fromX, int fromY, int toX, int toY, bool capture, Piece capturedPiece, bool isCastling = false)
    {
        Piece = piece;
        FromX = fromX;
        FromY = fromY;
        ToX = toX;
        ToY = toY;
        Capture = capture;
        CapturedPiece = capturedPiece;
        IsValid = true;
        IsCastling = isCastling;
        IsPromotion = false;
        PromotionType = null;
        OriginalPiece = null;
        OriginalHasMovedState = piece.HasMoved;
    }

    public Move(Piece piece, int fromX, int fromY, int toX, int toY, bool capture, Piece capturedPiece, string promotionType, Piece originalPiece)
        : this(piece, fromX, fromY, toX, toY, capture, capturedPiece)
    {
        IsPromotion = true;
        PromotionType = promotionType;
        OriginalPiece = originalPiece;
        OriginalHasMovedState = originalPiece.HasMoved;
    }

    public static Move Invalid = new Move
    {
        IsValid = false
    };
}