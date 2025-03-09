using GameLogic;
using System;
using System.Collections.Generic;


namespace GameLogic
{
    public class Move
    {
        private readonly GameBoard r_GameBoard;
        private readonly Player r_Player;

        public int FromRow { get; set; }
        public int FromCol { get; set; }
        public int ToRow { get; set; }
        public int ToCol { get; set; }
        public int Score { get; set; }

        public Move(GameBoard i_GameBoard, Player i_Player)
        {
            r_GameBoard = i_GameBoard;
            r_Player = i_Player;
        }

        public Move FindBestMove()
        {
            List<Move> possibleMoves = getPossibleCaptureMoves();
            Move bestMove = null;

            if (possibleMoves.Count > 0)
            {
                bestMove = findBestMoveFromList(possibleMoves);
            }
            else
            {
                possibleMoves = GetPossibleRegularMoves();
                if (possibleMoves.Count > 0)
                {
                    bestMove = findBestMoveFromList(possibleMoves);
                }
            }

            return bestMove;
        }

        private Move findBestMoveFromList(List<Move> i_PossibleMoves)
        {
            Move bestMove = i_PossibleMoves[0];
            foreach (Move move in i_PossibleMoves)
            {
                move.Score = evaluateMove(move);
                if (move.Score > bestMove.Score)
                {
                    bestMove = move;
                }
            }

            return bestMove;
        }

        private List<Move> getPossibleCaptureMoves()
        {
            List<Move> captureMoves = new List<Move>();

            for (int row = 0; row < r_GameBoard.BoardSize; row++)
            {
                for (int col = 0; col < r_GameBoard.BoardSize; col++)
                {
                    GamePiece piece = r_GameBoard.Board[row, col];

                    if (piece != null && piece.PieceColor == r_Player.PlayerColor)
                    {
                        captureMoves.AddRange(getCaptureMoves(row, col));
                    }
                }
            }

            return captureMoves;
        }

        private List<Move> GetPossibleRegularMoves()
        {
            List<Move> regularMoves = new List<Move>();

            for (int row = 0; row < r_GameBoard.BoardSize; row++)
            {
                for (int col = 0; col < r_GameBoard.BoardSize; col++)
                {
                    GamePiece piece = r_GameBoard.Board[row, col];
                    if (piece != null && piece.PieceColor == r_Player.PlayerColor)
                    {
                        addPossibleMoves(row, col, regularMoves);
                    }
                }
            }

            return regularMoves;
        }

        private void addPossibleMoves(int i_FromRow, int i_FromCol, List<Move> io_Moves)
        {
            GamePiece piece = r_GameBoard.Board[i_FromRow, i_FromCol];
            int[] rowDiffs = piece.PieceType == ePieceType.King ? new[] { -1, 1 } : new[] { -1 };
            int[] colDiffs = { -1, 1 };

            foreach (int rowDiff in rowDiffs)
            {
                foreach (int colDiff in colDiffs)
                {
                    int toRow = i_FromRow + rowDiff;
                    int toCol = i_FromCol + colDiff;

                    if (r_GameBoard.IsValidPosition(toRow, toCol) &&
                        r_GameBoard.IsMoveValid(i_FromRow, i_FromCol, toRow, toCol, r_Player))
                    {
                        io_Moves.Add(createMove(i_FromRow, i_FromCol, toRow, toCol));
                    }
                }
            }
        }

        private List<Move> getCaptureMoves(int i_FromRow, int i_FromCol)
        {
            List<Move> captureMoves = new List<Move>();
            GamePiece piece = r_GameBoard.Board[i_FromRow, i_FromCol];
            int[] rowDiffs = piece.PieceType == ePieceType.King ? new[] { -2, 2 } : new[] { -2 };
            int[] colDiffs = { -2, 2 };

            foreach (int rowDiff in rowDiffs)
            {
                foreach (int colDiff in colDiffs)
                {
                    int toRow = i_FromRow + rowDiff;
                    int toCol = i_FromCol + colDiff;

                    if (r_GameBoard.IsValidPosition(toRow, toCol) &&
                        r_GameBoard.IsMoveValid(i_FromRow, i_FromCol, toRow, toCol, r_Player))
                    {
                        captureMoves.Add(createMove(i_FromRow, i_FromCol, toRow, toCol));
                    }
                }
            }

            return captureMoves;
        }

        private Move createMove(int i_FromRow, int i_FromCol, int i_ToRow, int i_ToCol)
        {
            return new Move(r_GameBoard, r_Player)
            {
                FromRow = i_FromRow,
                FromCol = i_FromCol,
                ToRow = i_ToRow,
                ToCol = i_ToCol,
                Score = 0
            };
        }

        private int evaluateMove(Move i_Move)
        {
            int score = 0;
            const int k_CaptureScore = 100;
            const int k_KingScore = 50;
            const int k_PositionalScore = 2;

            if (Math.Abs(i_Move.ToRow - i_Move.FromRow) == 2)
            {
                score += k_CaptureScore;
            }

            GamePiece piece = r_GameBoard.Board[i_Move.FromRow, i_Move.FromCol];
            if (piece != null &&
                piece.PieceType != ePieceType.King &&
                i_Move.ToRow == 0)
            {
                score += k_KingScore;
            }

            score += (r_GameBoard.BoardSize / 2 - Math.Abs(r_GameBoard.BoardSize / 2 - i_Move.ToCol)) * k_PositionalScore;

            return score;
        }
    }
}