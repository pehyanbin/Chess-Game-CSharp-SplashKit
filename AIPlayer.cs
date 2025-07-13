// AIPlayer.cs
using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;

public class AIPlayer : Player
{
    private Random _random;

    public AIPlayer(Color color, Board board, string name, ChessGame game)
        : base(color, board, name, game)
    {
        _random = new Random();
    }

    public override bool HandleTurn()
    {
        SplashKit.Delay(500);

        var myPieces = _board.GetPieces(_color)
            .Where(p => p.GetValidMoves(_board).Count > 0)
            .ToList();

        if (myPieces.Count == 0) return false;

        Piece piece = myPieces[_random.Next(myPieces.Count)];
        List<(int x, int y)> moves = piece.GetValidMoves(_board);

        if (moves.Count > 0)
        {
            var move = moves[_random.Next(moves.Count)];

            int fromX = piece.X;
            int fromY = piece.Y;
            bool isCastling = piece.Type == "king" && Math.Abs(move.x - fromX) == 2;

            if (_board.MovePiece(piece, move.x, move.y, out Piece captured))
            {
                LastMove = new Move(piece, fromX, fromY, move.x, move.y,
                                 captured != null, captured, isCastling);
                return true;
            }
        }

        return false;
    }
}