// AIPlayer.cs
using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;

public class AIPlayer : Player
{
    private Random _random;

    public AIPlayer(Color color, Board board, string name) : base(color, board, name)
    {
        _random = new Random();
    }

    public override bool HandleTurn()
    {
        SplashKit.Delay(500); // AI thinking time

        // Get all AI pieces that have valid moves
        var myPieces = _board.GetPieces(_color)
            .Where(p => p.GetValidMoves(_board).Count > 0)
            .ToList();

        if (myPieces.Count == 0) return false;

        // Select a random piece with moves
        Piece piece = myPieces[_random.Next(myPieces.Count)];
        List<(int x, int y)> moves = piece.GetValidMoves(_board);

        if (moves.Count > 0)
        {
            // Select a random valid move
            var move = moves[_random.Next(moves.Count)];

            // Remember original position
            int fromX = piece.X;
            int fromY = piece.Y;
            Piece captured = _board.PieceAt(move.x, move.y);

            if (_board.MovePiece(piece, move.x, move.y))
            {
                LastMove = new Move(piece, fromX, fromY, move.x, move.y, captured != null);
                return true;
            }
        }

        return false;
    }
}