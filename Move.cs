// Move.cs
public struct Move
{
    public Piece Piece;
    public int FromX;
    public int FromY;
    public int ToX;
    public int ToY;
    public bool Capture;
    public bool IsValid;
    public bool IsCastling;
    public bool IsPromotion;
    public string PromotionType;

    public Move(Piece piece, int fromX, int fromY, int toX, int toY, bool capture, bool isCastling = false)
    {
        Piece = piece;
        FromX = fromX;
        FromY = fromY;
        ToX = toX;
        ToY = toY;
        Capture = capture;
        IsValid = true;
        IsCastling = isCastling;
        IsPromotion = false;
        PromotionType = null;
    }

    // Promotion constructor
    public Move(Piece piece, int fromX, int fromY, int toX, int toY, bool capture, string promotionType)
        : this(piece, fromX, fromY, toX, toY, capture)
    {
        IsPromotion = true;
        PromotionType = promotionType;
    }

    // Invalid move constructor
    public static Move Invalid = new Move
    {
        IsValid = false
    };
}