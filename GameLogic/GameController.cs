using System;
using System.Text;

namespace GameLogic
{
    public class GameController

    {
        public GameBoard m_GameBoard { get; private set; }
        public Player m_Player1 { get; private set; }
        public Player m_Player2 { get; private set; }
        public bool m_IsPlayingAgainstComputer { get; private set; }
        public bool m_IsCurrentPlayerComputer => CurrentPlayer.Name == "Computer";

        private Player m_CurrentPlayer;
        private Player m_LastPlayer;

        public Player CurrentPlayer
        {
            get { return m_CurrentPlayer; }
            set { m_CurrentPlayer = value; }
        }

        public Player LastPlayer
        {
            get { return m_LastPlayer; }
            private set { m_LastPlayer = value; }
        }


        public GameController(int i_BoardSize, Player i_Player1, Player i_Player2, bool i_IsPlayingAgainstComputer)
        {
            m_GameBoard = new GameBoard(i_BoardSize);
            m_Player1 = i_Player1;
            m_Player2 = i_Player2;
            m_IsPlayingAgainstComputer = i_IsPlayingAgainstComputer;
            m_CurrentPlayer = m_Player2;
            m_LastPlayer = m_Player2;
        }

        public void ResetGame()
        {
            m_GameBoard.ResetBoard();
            CurrentPlayer = m_Player1;
            m_GameBoard.CurrentTurnColor = ePieceColor.PlayerO;
        }

        public bool HasValidMove(Player i_Player)
        {
            bool hasValidMove = false;

            for (int row = 0; row < m_GameBoard.BoardSize; row++)
            {
                for (int col = 0; col < m_GameBoard.BoardSize; col++)
                {
                    if (checkValidMoveFromPosition(row, col, i_Player))
                    {
                        hasValidMove = true;
                        break;
                    }
                }

                if (hasValidMove)
                {
                    break;
                }
            }

            return hasValidMove;
        }

        private bool checkValidMoveFromPosition(int i_Row, int i_Col, Player i_Player)
        {
            bool isValidMove = false;

            GamePiece piece = m_GameBoard.Board[i_Row, i_Col];

            if (piece != null && piece.PieceColor == i_Player.PlayerColor)
            {
                int[] rowDiffs = { -1, 1, -2, 2 };
                int[] colDiffs = { -1, 1, -2, 2 };

                foreach (int rowDiff in rowDiffs)
                {
                    foreach (int colDiff in colDiffs)
                    {
                        int toRow = i_Row + rowDiff;
                        int toCol = i_Col + colDiff;

                        if (m_GameBoard.IsValidPosition(toRow, toCol) &&
                            m_GameBoard.IsMoveValid(i_Row, i_Col, toRow, toCol, i_Player))
                        {
                            isValidMove = true;
                            break;
                        }
                    }

                    if (isValidMove)
                    {
                        break;
                    }
                }
            }

            return isValidMove;
        }

        public bool IsGameOver(out string o_WinnerMessage)
        {
            bool isGameOver = false;
            o_WinnerMessage = string.Empty;

            if (m_GameBoard.PlayerOCount == 0 || m_GameBoard.PlayerXCount == 0)
            {
                o_WinnerMessage = m_GameBoard.PlayerOCount > m_GameBoard.PlayerXCount
                    ? $"{m_Player1.Name} wins!"
                    : $"{m_Player2.Name} wins!";
                isGameOver = true;
            }

            return isGameOver;
        }

        public Player GetOpponentPlayer()
        {

            return CurrentPlayer == m_Player1 ? m_Player2 : m_Player1;
        }

        public void CalculateAndAssignPoints()
        {
            int player1Points = m_GameBoard.CalculatePointsForRemainingPieces(m_Player1.PlayerColor);
            int player2Points = m_GameBoard.CalculatePointsForRemainingPieces(m_Player2.PlayerColor);
            int scoreDifference = Math.Abs(player1Points - player2Points);

            if (scoreDifference > 0)
            {
                if (player1Points > player2Points)
                {
                    m_Player1.AddPoints(scoreDifference);
                }
                else
                {
                    m_Player2.AddPoints(scoreDifference);
                }
            }
        }

        public string ProcessPlayerMove(string i_MoveInput, out string o_ErrorMessage, out string o_OutputMessage)
        {
            string moveResult;
            o_ErrorMessage = string.Empty;

            if (isQuitMove(i_MoveInput, out o_OutputMessage))
            {
                moveResult = null;
            }
            else if (!tryParseMoveInput(i_MoveInput, out int fromRow, out int fromCol,
                out int toRow, out int toCol, out o_ErrorMessage))
            {
                moveResult = null;
            }
            else if (!isValidPieceSelection(fromRow, fromCol, out o_ErrorMessage))
            {
                moveResult = null;
            }
            else if (!executeMove(fromRow, fromCol, toRow, toCol, out o_ErrorMessage))
            {
                moveResult = null;
            }
            else
            {
                moveResult = formatMoveOutput(fromRow, fromCol, toRow, toCol);
            }

            return moveResult;
        }

        private bool isQuitMove(string i_MoveInput, out string o_OutputMessage)
        {
            bool IsQuitMove = true;
            o_OutputMessage = string.Empty;

            if (i_MoveInput.ToUpper() == "Q")
            {
                Player opponent = GetOpponentPlayer();
                o_OutputMessage = $"{opponent.Name} wins by forfeit!";

                int player1Points = m_GameBoard.CalculatePointsForRemainingPieces(m_Player1.PlayerColor);
                int player2Points = m_GameBoard.CalculatePointsForRemainingPieces(m_Player2.PlayerColor);
                int scoreDifference = Math.Abs(player1Points - player2Points);

                if (opponent == m_Player1)
                {
                    m_Player1.AddPoints(scoreDifference);
                }
                else
                {
                    m_Player2.AddPoints(scoreDifference);
                }

            }
            else
            {
                IsQuitMove = false;
            }

            return IsQuitMove;
        }

        private bool tryParseMoveInput(string i_MoveInput, out int o_FromRow, out int o_FromCol,
            out int o_ToRow, out int o_ToCol, out string o_ErrorMessage)
        {
            bool isValidFormat = true;
            string[] parts = i_MoveInput.Split('>');

            o_FromRow = o_FromCol = o_ToRow = o_ToCol = -1;
            o_ErrorMessage = string.Empty;

            if (parts.Length != 2)
            {
                o_ErrorMessage = "Invalid move format. Use 'rowcol>rowcol' (e.g., aB>cD).";
                isValidFormat = false;
            }
            else
            {
                if (!isValidPositionFormat(parts[0]))
                {
                    o_ErrorMessage = "The source position format is invalid: uppercase followed by lowercase is not allowed.";
                    isValidFormat = false;
                }
                else if (!isValidPositionFormat(parts[1]))
                {
                    o_ErrorMessage = "The target position format is invalid: uppercase followed by lowercase is not allowed.";
                    isValidFormat = false;
                }
                else
                {
                    o_FromRow = parts[0][0] - 'a';
                    o_FromCol = parts[0][1] - 'A';
                    o_ToRow = parts[1][0] - 'a';
                    o_ToCol = parts[1][1] - 'A';
                }
            }

            return isValidFormat;
        }

        private bool isValidPositionFormat(string i_Position)
        {

            return char.IsLower(i_Position[0]) && char.IsUpper(i_Position[1]);
        }

        private bool isValidPieceSelection(int i_FromRow, int i_FromCol, out string o_ErrorMessage)
        {
            bool isValid = true;
            o_ErrorMessage = string.Empty;

            GamePiece piece = m_GameBoard.Board[i_FromRow, i_FromCol];
            if (piece == null || piece.PieceColor != CurrentPlayer.PlayerColor)
            {
                o_ErrorMessage = "Invalid move. You must move your own piece.";
                isValid = false;
            }

            return isValid;
        }

        private bool executeMove(int i_FromRow, int i_FromCol, int i_ToRow, int i_ToCol,
            out string o_ErrorMessage)
        {

            return m_GameBoard.MovePiece(i_FromRow, i_FromCol, i_ToRow, i_ToCol,
                CurrentPlayer, out o_ErrorMessage);
        }

        private string formatMoveOutput(int i_FromRow, int i_FromCol, int i_ToRow, int i_ToCol)
        {
            StringBuilder output = new StringBuilder();
            output.Append(m_GameBoard.GetRowLabel(i_FromRow));
            output.Append(m_GameBoard.GetColumnLabel(i_FromCol));
            output.Append(">");
            output.Append(m_GameBoard.GetRowLabel(i_ToRow));
            output.Append(m_GameBoard.GetColumnLabel(i_ToCol));

            return output.ToString();
        }

        public string PerformComputerMove(out string o_OutputMessage)
        {
            o_OutputMessage = string.Empty;
            string result = null;

            if (isGameOverForComputer(out o_OutputMessage))
            {
                result = o_OutputMessage;
            }
            else
            {
                result = executeBestComputerMove(out o_OutputMessage);
            }

            return result;
        }

        private bool isGameOverForComputer(out string o_OutputMessage)
        {
            const bool v_IsGameOver = true;
            const bool v_IsNotGameOver = false;
            o_OutputMessage = string.Empty;
            bool isGameOver = v_IsNotGameOver;

            if (!HasValidMove(m_Player2))
            {
                if (!HasValidMove(m_Player1))
                {
                    o_OutputMessage = "No valid moves for both players. It's a tie!";
                    isGameOver = v_IsGameOver;
                }
                else
                {
                    o_OutputMessage = "Computer has no valid moves. Player wins!";
                    isGameOver = v_IsGameOver;
                }
            }

            return isGameOver;
        }


        private string executeBestComputerMove(out string o_OutputMessage)
        {
            string moveResult = null;
            o_OutputMessage = string.Empty;

            Move moveHandler = new Move(m_GameBoard, m_Player2);
            Move bestMove = moveHandler.FindBestMove();

            while (bestMove != null)
            {
                if (!tryExecuteMove(bestMove, out o_OutputMessage))
                {
                    moveResult = null;
                    break;
                }

                if (isMultiJumpAvailable(bestMove))
                {
                    bestMove = moveHandler.FindBestMove();
                }
                else
                {
                    break;
                }
            }

            if (bestMove != null)
            {
                moveResult = formatMoveOutput(bestMove);
            }

            return moveResult;
        }

        private string formatMoveOutput(Move i_Move)
        {
            string result;
            if (i_Move == null)
            {
                result = null;
            }
            else
            {
                StringBuilder output = new StringBuilder();
                output.Append(m_GameBoard.GetRowLabel(i_Move.FromRow));
                output.Append(m_GameBoard.GetColumnLabel(i_Move.FromCol));
                output.Append(">");
                output.Append(m_GameBoard.GetRowLabel(i_Move.ToRow));
                output.Append(m_GameBoard.GetColumnLabel(i_Move.ToCol));
                result = output.ToString();
            }

            return result;
        }

        private bool tryExecuteMove(Move i_Move, out string o_ErrorMessage)
        {

            return m_GameBoard.MovePiece(i_Move.FromRow,i_Move.FromCol,i_Move.ToRow,i_Move.ToCol,m_Player2,out o_ErrorMessage);
        }

        private bool isMultiJumpAvailable(Move i_Move)
        {

            return m_GameBoard.IsCaptureMove(i_Move.FromRow, i_Move.FromCol,
                i_Move.ToRow, i_Move.ToCol) && m_GameBoard.HasMoreJumps(i_Move.ToRow, i_Move.ToCol);
        }

        public void SwitchTurn(out bool o_GameEnded, out string o_OutputMessage)
        {
            o_GameEnded = false;
            o_OutputMessage = string.Empty;

            if (m_GameBoard.IsInMultiJump)
            {

                return;
            }

            updateCurrentPlayer();

            if (!HasValidMove(CurrentPlayer))
            {
                handleNoValidMoves(out o_GameEnded, out o_OutputMessage);
            }
            else
            {
                m_GameBoard.CurrentTurnColor = CurrentPlayer.PlayerColor;

            }
        }

        private void updateCurrentPlayer()
        {
            LastPlayer = CurrentPlayer;
            CurrentPlayer = CurrentPlayer == m_Player1 ? m_Player2 : m_Player1;
        }

        private void handleNoValidMoves(out bool o_GameEnded, out string o_OutputMessage)
        {
            o_GameEnded = true;

            StringBuilder output = new StringBuilder();
            if (m_GameBoard.PlayerOCount == 0 || m_GameBoard.PlayerXCount == 0)
            {
                Player winner = m_GameBoard.PlayerOCount > m_GameBoard.PlayerXCount ? m_Player1 : m_Player2;
                output.Append($"{winner.Name} wins! The opponent has no pieces left.");
            }
            else
            {
                output.Append($"{CurrentPlayer.Name} has no valid moves.{Environment.NewLine}");

                Player opponentPlayer = GetOpponentPlayer();

                if (HasValidMove(opponentPlayer))
                {
                    output.Append($"{opponentPlayer.Name} wins!");
                }
                else
                {
                    output.Append("No valid moves for both players. It's a tie!");
                }
            }
            o_OutputMessage = output.ToString();
        }
    }
}
