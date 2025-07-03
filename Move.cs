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

    public Move(Piece piece, int fromX, int fromY, int toX, int toY, bool capture)
    {
        Piece = piece;
        FromX = fromX;
        FromY = fromY;
        ToX = toX;
        ToY = toY;
        Capture = capture;
        IsValid = true;
    }

    // Invalid move constructor
    public static Move Invalid = new Move
    {
        IsValid = false
    };
}