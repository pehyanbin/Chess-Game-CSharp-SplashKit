// Player.cs
using SplashKitSDK;

public class Player
{
    protected Color _color;
    protected Board _board;
    protected SelectionManager _selector;
    public string Name { get; }
    public Move LastMove { get; protected set; }

    public Player(Color color, Board board, string name, ChessGame game)
    {
        _color = color;
        _board = board;
        _selector = new SelectionManager(board, _color, game);
        Name = name;
        LastMove = Move.Invalid;
    }

    public virtual bool HandleTurn()
    {
        _selector.Update();
        _selector.DrawHighlight();

        if (_selector.MoveMade)
        {
            bool isCastling = _selector.MovedPiece.Type == "king" &&
                            Math.Abs(_selector.MovedToX - _selector.MovedFromX) == 2;

            LastMove = new Move(
                _selector.MovedPiece,
                _selector.MovedFromX,
                _selector.MovedFromY,
                _selector.MovedToX,
                _selector.MovedToY,
                _selector.CapturedPiece != null,
                _selector.CapturedPiece,
                isCastling
            );
        }

        return _selector.MoveMade;
    }
}