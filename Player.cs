// Player.cs
using SplashKitSDK;

public class Player
{
    protected Color _color;
    protected Board _board;
    protected SelectionManager _selector;

    public Player(Color color, Board board)
    {
        _color = color;
        _board = board;
        _selector = new SelectionManager(board, _color);
    }

    public virtual bool HandleTurn()
    {
        _selector.Update();
        _selector.DrawHighlight();
        return _selector.MoveMade;
    }
}