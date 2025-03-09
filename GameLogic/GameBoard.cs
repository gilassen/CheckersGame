using GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameLogic
{
    public class GameBoard
    {
        public int BoardSize { get; private set; }
        private readonly int[] r_AllowedBoardSizes = { 6, 8, 10 };
        private ePieceColor m_CurrentTurnColor;
        private GamePiece[,] m_GameBoard;
        private bool m_IsInMultiJump;
        private int m_LastJumpedRow = -1;
        private int m_LastJumpedCol = -1;
        private int m_PlayerOCount;
        private int m_PlayerXCount;
        public const int k_MaxNameLength = 20;

        public ePieceColor CurrentTurnColor
        {
            get { return m_CurrentTurnColor; }
            set { m_CurrentTurnColor = value; }
        }

        public GamePiece[,] Board
        {
            get { return m_GameBoard; }
            private set { m_GameBoard = value; }
        }

        public int PlayerOCount
        {
            get { return m_PlayerOCount; }
            private set { m_PlayerOCount = value; }
        }

        public int PlayerXCount
        {
            get { return m_PlayerXCount; }
            private set { m_PlayerXCount = value; }
        }

        public bool IsInMultiJump
        {
            get { return m_IsInMultiJump; }
            private set { m_IsInMultiJump = value; }
        }

        public int[] AllowedBoardSizes
        {
            get { return r_AllowedBoardSizes; }
        }

        public GameBoard(int i_BoardSize)
        {
            BoardSize = i_BoardSize;
            m_GameBoard = new GamePiece[BoardSize, BoardSize];
            initializeBoard();
        }

        private void initializeBoard()
        {
            int playerXCount = 0, playerOCount = 0;

            initializePlayerPieces(0, BoardSize / 2 - 1, ePieceColor.PlayerO, ref playerOCount);
            initializePlayerPieces(BoardSize / 2 + 1, BoardSize, ePieceColor.PlayerX, ref playerXCount);

            m_PlayerXCount = playerXCount;
            m_PlayerOCount = playerOCount;
        }

        private void initializePlayerPieces(int i_StartRow, int i_EndRow, ePieceColor i_Color, ref int io_PlayerCount)
        {
            for (int row = i_StartRow; row < i_EndRow; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    if ((row + col) % 2 == 1)
                    {
                        m_GameBoard[row, col] = new GamePiece(i_Color, row, col);
                        io_PlayerCount++;
                    }
                }
            }
        }

        public void ResetBoard()
        {
            m_GameBoard = new GamePiece[BoardSize, BoardSize];
            initializeBoard();
        }

        public string GetColumnLabel(int i_Column)
        {

            return char.ToString((char)('A' + i_Column));
        }

        public string GetRowLabel(int i_Row)
        {

            return char.ToString((char)('a' + i_Row));
        }


        public bool IsValidPosition(int i_Row, int i_Col)
        {

            return i_Row >= 0 && i_Row < BoardSize && i_Col >= 0 && i_Col < BoardSize;
        }

        public int CalculatePointsForRemainingPieces(ePieceColor i_PlayerColor)
        {
            int points = 0;

            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    GamePiece piece = m_GameBoard[row, col];
                    if (piece != null && piece.PieceColor == i_PlayerColor)
                    {
                        points += piece.PieceType == ePieceType.King ? 4 : 1;
                    }
                }
            }

            return points;
        }

        public bool MovePiece(int i_FromRow, int i_FromCol, int i_ToRow, int i_ToCol,
            Player i_CurrentPlayer, out string o_ErrorMessage)
        {
            bool moveSuccessful = false;
            o_ErrorMessage = string.Empty;

            if (!validateMoveParameters(i_FromRow, i_FromCol, i_ToRow, i_ToCol,
                i_CurrentPlayer, out o_ErrorMessage))
            {
                moveSuccessful = false;
            }

            GamePiece piece = m_GameBoard[i_FromRow, i_FromCol];
            bool isCaptureMove = IsCaptureMove(i_FromRow, i_FromCol, i_ToRow, i_ToCol);

            if (validateMove(piece, i_FromRow, i_FromCol, i_ToRow, i_ToCol,
                i_CurrentPlayer, isCaptureMove, out o_ErrorMessage))
            {
                performMove(i_FromRow, i_FromCol, i_ToRow, i_ToCol, i_CurrentPlayer, piece, isCaptureMove);
                handlePostMove(i_ToRow, i_ToCol, isCaptureMove, out o_ErrorMessage);
                moveSuccessful = true;
            }

            return moveSuccessful;
        }

        private bool validateMoveParameters(int i_FromRow, int i_FromCol, int i_ToRow, int i_ToCol,
            Player i_CurrentPlayer, out string o_ErrorMessage)
        {
            o_ErrorMessage = string.Empty;
            bool isValid = true;

            if (!IsValidPosition(i_ToRow, i_ToCol))
            {
                o_ErrorMessage = "The target position is out of bounds.";
                isValid = false;
            }
            else if (!IsValidPosition(i_FromRow, i_FromCol))
            {
                o_ErrorMessage = "The source position is out of bounds.";
                isValid = false;
            }
            else if (m_GameBoard[i_FromRow, i_FromCol] == null)
            {
                o_ErrorMessage = "No piece at the source position.";
                isValid = false;
            }
            else if (i_CurrentPlayer.PlayerColor != m_CurrentTurnColor)
            {
                o_ErrorMessage = "It is not your turn.";
                isValid = false;
            }

            return isValid;
        }

        private bool validateMove(GamePiece i_Piece, int i_FromRow, int i_FromCol, int i_ToRow, int i_ToCol,
            Player i_CurrentPlayer, bool i_IsCaptureMove, out string o_ErrorMessage)
        {
            o_ErrorMessage = string.Empty;
            bool isValid = true;

            if (i_Piece.PieceColor != i_CurrentPlayer.PlayerColor)
            {
                o_ErrorMessage = "You can only move your own pieces.";
                isValid = false;
            }
            else if (m_IsInMultiJump && (i_FromRow != m_LastJumpedRow || i_FromCol != m_LastJumpedCol))
            {
                o_ErrorMessage = "You must continue capturing with the same piece.";
                isValid = false;
            }
            else if (m_IsInMultiJump && !i_IsCaptureMove)
            {
                o_ErrorMessage = "You must continue capturing during a multi-jump.";
                isValid = false;
            }
            else if (m_GameBoard[i_ToRow, i_ToCol] != null)
            {
                o_ErrorMessage = "The target position is already occupied.";
                isValid = false;
            }
            else if (!isMoveDirectionValid(i_Piece, i_FromRow, i_FromCol, i_ToRow, i_ToCol))
            {
                o_ErrorMessage = "Invalid move direction.";
                isValid = false;
            }
            else if (!m_IsInMultiJump && isPlayerMustCapture(i_CurrentPlayer) && !i_IsCaptureMove)
            {
                o_ErrorMessage = "You must perform a capture move.";
                isValid = false;
            }

            return isValid;
        }

        private void performMove(int i_FromRow, int i_FromCol, int i_ToRow, int i_ToCol,
            Player i_CurrentPlayer, GamePiece i_Piece, bool i_IsCaptureMove)
        {
            if (i_IsCaptureMove)
            {
                int capturedRow = (i_FromRow + i_ToRow) / 2, capturedCol = (i_FromCol + i_ToCol) / 2;
                removePiece(capturedRow, capturedCol);
            }

            m_GameBoard[i_FromRow, i_FromCol] = null;
            m_GameBoard[i_ToRow, i_ToCol] = i_Piece;
            i_Piece.Row = i_ToRow;
            i_Piece.Column = i_ToCol;
            promoteToKingIfEligible(i_Piece);
        }

        private void handlePostMove(int i_ToRow, int i_ToCol, bool i_IsCaptureMove, out string o_Message)
        {
            o_Message = string.Empty;

            if (i_IsCaptureMove && hasMoreJumps(i_ToRow, i_ToCol))
            {
                m_LastJumpedRow = i_ToRow;
                m_LastJumpedCol = i_ToCol;
                m_IsInMultiJump = true;
                o_Message = "You have more jumps. Continue your turn.";
            }
            else
            {
                m_LastJumpedRow = -1;
                m_LastJumpedCol = -1;
                m_IsInMultiJump = false;
                switchTurn();
            }
        }

        public bool IsCaptureMove(int i_FromRow, int i_FromCol, int i_ToRow, int i_ToCol)
        {
            int rowDiff = Math.Abs(i_ToRow - i_FromRow), colDiff = Math.Abs(i_ToCol - i_FromCol);

            return rowDiff == 2 && colDiff == 2;
        }

        private bool hasMoreJumps(int i_Row, int i_Col)
        {
            GamePiece piece = m_GameBoard[i_Row, i_Col];

            return piece != null && getCaptureMoves(i_Row, i_Col, piece).Count > 0;
        }

        private bool isPlayerMustCapture(Player i_CurrentPlayer)
        {
            bool mustCapture = false;

            for (int row = 0; row < BoardSize && !mustCapture; row++)
            {
                for (int col = 0; col < BoardSize && !mustCapture; col++)
                {
                    GamePiece piece = m_GameBoard[row, col];
                    if (piece != null && piece.PieceColor == i_CurrentPlayer.PlayerColor &&
                        canCaptureWithPiece(piece))
                    {
                        mustCapture = true;
                    }
                }
            }

            return mustCapture;
        }

        private bool canCaptureWithPiece(GamePiece i_Piece)
        {

            return getCaptureMoves(i_Piece.Row, i_Piece.Column, i_Piece).Count > 0;
        }

        private void removePiece(int i_Row, int i_Col)
        {
            GamePiece piece = m_GameBoard[i_Row, i_Col];
            if (piece != null)
            {
                if (piece.PieceColor == ePieceColor.PlayerO)
                {
                    m_PlayerOCount--;
                }
                else
                {
                    m_PlayerXCount--;
                }

                m_GameBoard[i_Row, i_Col] = null;
            }
        }

        private void promoteToKingIfEligible(GamePiece i_Piece)
        {
            if (i_Piece.PieceType == ePieceType.Regular)
            {
                if ((i_Piece.PieceColor == ePieceColor.PlayerO && i_Piece.Row == BoardSize - 1) ||
                    (i_Piece.PieceColor == ePieceColor.PlayerX && i_Piece.Row == 0))
                {
                    i_Piece.PromoteToKing();
                }
            }
        }

        private void switchTurn()
        {
            m_CurrentTurnColor = m_CurrentTurnColor == ePieceColor.PlayerO ?
                ePieceColor.PlayerX : ePieceColor.PlayerO;
        }

        private bool isMoveDirectionValid(GamePiece i_Piece, int i_FromRow, int i_FromCol,
            int i_ToRow, int i_ToCol)
        {
            int rowDiff = i_ToRow - i_FromRow, colDiff = Math.Abs(i_ToCol - i_FromCol);
            bool isValid = true;

            if (colDiff != Math.Abs(rowDiff))
            {
                isValid = false;
            }
            else if (i_Piece.PieceType != ePieceType.King)
            {
                isValid = !((i_Piece.PieceColor == ePieceColor.PlayerX && rowDiff >= 0) ||
                    (i_Piece.PieceColor == ePieceColor.PlayerO && rowDiff <= 0));
            }

            return isValid;
        }

        private List<Tuple<int, int>> getCaptureMoves(int i_FromRow, int i_FromCol, GamePiece i_Piece)
        {
            List<Tuple<int, int>> captures = new List<Tuple<int, int>>();
            int[] rowDirections = getRowDirections(i_Piece), colDirections = { -2, 2 };

            foreach (int rowDiff in rowDirections)
            {
                foreach (int colDiff in colDirections)
                {
                    if (isCaptureValid(i_FromRow, i_FromCol, rowDiff, colDiff, i_Piece,
                        out Tuple<int, int> captureMove))
                    {
                        captures.Add(captureMove);
                    }
                }
            }

            return captures;
        }

        private int[] getRowDirections(GamePiece i_Piece)
        {
            int[] directions;

            if (i_Piece.PieceType == ePieceType.King)
            {
                directions = new[] { -2, 2 };
            }
            else
            {
                directions = i_Piece.PieceColor == ePieceColor.PlayerX ? new[] { -2 } : new[] { 2 };
            }

            return directions;
        }

        private bool isCaptureValid(int i_FromRow, int i_FromCol, int i_RowDiff, int i_ColDiff,
            GamePiece i_Piece, out Tuple<int, int> o_CaptureMove)
        {
            bool isValid = false;
            o_CaptureMove = null;

            int toRow = i_FromRow + i_RowDiff, toCol = i_FromCol + i_ColDiff;

            if (IsValidPosition(toRow, toCol) && m_GameBoard[toRow, toCol] == null)
            {
                int midRow = (i_FromRow + toRow) / 2, midCol = (i_FromCol + toCol) / 2;
                GamePiece midPiece = m_GameBoard[midRow, midCol];

                if (midPiece != null && midPiece.PieceColor != i_Piece.PieceColor)
                {
                    o_CaptureMove = new Tuple<int, int>(toRow, toCol);
                    isValid = true;
                }
            }

            return isValid;
        }

        public bool IsMoveValid(int i_FromRow, int i_FromCol, int i_ToRow, int i_ToCol, Player i_Player)
        {
            bool isValid = true;

            if (!IsValidPosition(i_FromRow, i_FromCol) || !IsValidPosition(i_ToRow, i_ToCol))
            {
                isValid = false;
            }
            else
            {
                GamePiece piece = m_GameBoard[i_FromRow, i_FromCol];

                if (piece == null || piece.PieceColor != i_Player.PlayerColor)
                {
                    isValid = false;
                }
                else if (m_GameBoard[i_ToRow, i_ToCol] != null)
                {
                    isValid = false;
                }
                else
                {
                    int rowDiff = i_ToRow - i_FromRow;
                    int colDiff = i_ToCol - i_FromCol;

                    if (piece.PieceType == ePieceType.Regular)
                    {
                        int direction = i_Player.PlayerColor == ePieceColor.PlayerX ? -1 : 1;

                        if (rowDiff == direction && Math.Abs(colDiff) == 1)
                        {
                            isValid = true;
                        }
                        else if (rowDiff == 2 * direction && Math.Abs(colDiff) == 2)
                        {
                            int midRow = i_FromRow + rowDiff / 2;
                            int midCol = i_FromCol + colDiff / 2;
                            GamePiece midPiece = m_GameBoard[midRow, midCol];

                            isValid = midPiece != null && midPiece.PieceColor != i_Player.PlayerColor;
                        }
                        else
                        {
                            isValid = false;
                        }
                    }
                    else if (piece.PieceType == ePieceType.King)
                    {
                        if (Math.Abs(rowDiff) == 1 && Math.Abs(colDiff) == 1)
                        {
                            isValid = true;
                        }
                        else if (Math.Abs(rowDiff) == 2 && Math.Abs(colDiff) == 2)
                        {
                            int midRow = i_FromRow + rowDiff / 2;
                            int midCol = i_FromCol + colDiff / 2;
                            GamePiece midPiece = m_GameBoard[midRow, midCol];

                            isValid = midPiece != null && midPiece.PieceColor != i_Player.PlayerColor;
                        }
                        else
                        {
                            isValid = false;
                        }
                    }
                }
            }

            return isValid;
        }

        public bool HasMoreJumps(int i_Row, int i_Col)
        {
            GamePiece piece = m_GameBoard[i_Row, i_Col];
            return piece != null && getCaptureMoves(i_Row, i_Col, piece).Count > 0;
        }

        public static bool ValidatePlayerName(string i_Input, out string o_Name)
        {
            o_Name = i_Input;

            return !string.IsNullOrEmpty(i_Input) &&
                   i_Input.Length <= k_MaxNameLength &&
                   !i_Input.Contains(" ") &&
                   containsOnlyEnglishLetters(i_Input); ;
        }

        private static bool containsOnlyEnglishLetters(string i_input)
        {
            bool isletter = true;
            foreach (char c in i_input)
            {
                if (!char.IsLetter(c) || c < 'A' || (c > 'Z' && c < 'a') || c > 'z')
                {
                    isletter = false;
                }
            }

            return isletter;
        }
    }
}