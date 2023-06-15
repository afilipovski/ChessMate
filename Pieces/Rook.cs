﻿using System.Collections.Generic;

namespace ChessMate.Pieces
{
    // Top
    public class Rook : ContinuousPathPiece
    {
        public bool MovedSinceStart { get; set; }

        public Rook(Position position, bool white) : base(position, white)
        {
            MovedSinceStart = false;
        }

        public Rook(ContinuousPathPiece cpp) : base(cpp)
        {
        }

        public override List<Board> PossibleMoves(Board b)
        {
            List<Board> boards = new List<Board>();

            // right
            findValidPositions(new Position(Position.X + 1, Position.Y), b, boards, p => p.X <= 7, p => p.X++);
            // left
            findValidPositions(new Position(Position.X - 1, Position.Y), b, boards, p => p.X >= 0, p => p.X--);
            // top
            findValidPositions(new Position(Position.X, Position.Y + 1), b, boards, p => p.Y <= 7, p => p.Y++);
            // bottom
            findValidPositions(new Position(Position.X, Position.Y - 1), b, boards, p => p.Y >= 0, p => p.Y--);

            return boards;
        }
    }
}
