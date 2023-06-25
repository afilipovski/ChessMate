﻿using ChessMate.Pieces;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace ChessMate
{
    public class Board
    {
        public Dictionary<Position, Piece> PieceByPosition { get; set; }
        public bool WhiteTurn { get; set; }
        public static int WIDTH { get; set; }
        public static int HEIGHT { get; set; }
        public static int OFFSET { get; set; }
        public Piece currentClickedPiece { get; set; }
        public List<Position> greenPositions { get; set; } = new List<Position>();
        public Position newPos { get; set; }

        // copy constructor
        public Board(Board board)
        {
            PieceByPosition = new Dictionary<Position, Piece>();
            WhiteTurn = !board.WhiteTurn;
            foreach (Position key in board.PieceByPosition.Keys)
            {
                PieceByPosition[key] = board.PieceByPosition[key];
            }
        }

        // copy 2
        public Board(Board b, Position posOld, Position posNew, Piece p) : this(b)
        {
            PieceByPosition[posNew] = p;
            PieceByPosition.Remove(posOld);
        }

        public Board()
        {
            PieceByPosition = new Dictionary<Position, Piece>();
            for (int i = 0; i < 8; ++i)
            {
                for (int j = 0; j < 8; j++)
                {
                    PieceByPosition.Add(new Position(i, j), null);
                }
            }
            {
                Position pos = new Position(3, 0);
                PieceByPosition[pos] = new King(pos, false);
                pos = new Position(3, 7);
                PieceByPosition[pos] = new King(pos, true);
                pos = new Position(4, 0);
                PieceByPosition[pos] = new Queen(pos, false);
                pos = new Position(4, 7);
                PieceByPosition[pos] = new Queen(pos, true);
            }
            for (int i = 0; i < 8; ++i)
            {
                Position pos = new Position(i, 1);
                PieceByPosition[pos] = new Pawn(pos, false);
                pos = new Position(i, 6);
                PieceByPosition[pos] = new Pawn(pos, true);
            }
            for (int i = 0; i < 8; i += 7)
            {
                Position pos = new Position(i, 0);
                PieceByPosition[pos] = new Rook(pos, false);
                pos = new Position(i, 7);
                PieceByPosition[pos] = new Rook(pos, true);

            }
            for (int i = 1; i < 8; i += 5)
            {
                Position pos = new Position(i, 0);
                PieceByPosition[pos] = new Knight(pos, false);
                pos = new Position(i, 7);
                PieceByPosition[pos] = new Knight(pos, true);

            }
            for (int i = 2; i < 8; i += 3)
            {
                Position pos = new Position(i, 0);
                PieceByPosition[pos] = new Bishop(pos, false);
                pos = new Position(i, 7);
                PieceByPosition[pos] = new Bishop(pos, true);

            }

        }

        public List<Board> Successor()
        {
            List<Board> res = new List<Board>();

            PieceByPosition.Values.ToList()
                .Where(piece => piece.White == WhiteTurn).ToList()
                .ForEach(piece =>
                {
                    res.Concat(piece.PossibleMoves(this));
                });

            return res;
        }
        public bool IsOccupied(Position position)
        {
            return PieceByPosition[position] != null;
        }

        public void DrawTiles(Graphics g, int height, int width, int offset)
        {
            HEIGHT = height;
            WIDTH = width;
            OFFSET = offset;
            Position[] pos = PieceByPosition.Keys.ToArray();
            for (int i = 0; i < 64; ++i)
            {
                pos[i].Draw(g);
            }
            Position[] greenPos = greenPositions.ToArray();
            for ( int i = 0; i < greenPositions.Count; ++i )
            {
                Debug.WriteLine("TEST");
                greenPos[i].Green = true;
                greenPos[i].Draw(g);
            }
            foreach (Piece piece in PieceByPosition.Values)
            {
                if (piece == null) continue;
                piece.Draw(g);
            }
        }

        public Board Click(Position p)
        {
            Position clickedPosition = new Position((p.X - OFFSET) / HEIGHT, p.Y / HEIGHT);
            Debug.WriteLine(clickedPosition.X + " " + clickedPosition.Y);
            Piece clickedPiece = PieceByPosition[clickedPosition];
            if (clickedPiece != null) Debug.WriteLine("You clicked " + clickedPiece.GetType());
            if (clickedPiece == null || !clickedPiece.White)
            {
                if (currentClickedPiece == null) return this;
                Board newBoard = currentClickedPiece.PossibleMoves(this).Find(board => 
                    board.PieceByPosition[clickedPosition] == currentClickedPiece);
                if (newBoard == null) return this;
                currentClickedPiece.PossibleMoves(this).ForEach(board => { this.greenPositions.Add(board.newPos); });
                currentClickedPiece.Position = clickedPosition;
                currentClickedPiece = null;
                return newBoard;
            }
            else if(currentClickedPiece == null || clickedPiece.White)
            {
                currentClickedPiece = clickedPiece;
            }
            return this;
            
        }

    }
}
