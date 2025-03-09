using System;
using System.Drawing;
using System.Windows.Forms;
using GameLogic;

namespace UIGameView
{
    public partial class CheckersGameForm : Form
    {
        private Button[,] m_ButtonsBoard;
        private GameController m_GameController;
        private Label m_LabelPlayer1Score;
        private Label m_LabelPlayer2Score;
        private Button m_SelectedPiece = null;
        private PieceAnimator m_Animator;

        public CheckersGameForm(CheckersSettingsForm i_Settings)
        {
            InitializeComponent();
            m_Animator = new PieceAnimator(this);

            initializeGame(i_Settings);
            initializeControls();
        }

        private void initializeGame(CheckersSettingsForm i_Settings)
        {
            Player player1 = new Player(i_Settings.Player1Name, ePieceColor.PlayerO);
            Player player2 = new Player(i_Settings.Player2Name, ePieceColor.PlayerX);
            m_GameController = new GameController(
                i_Settings.BoardSize,
                player1,
                player2,
                i_Settings.IsPlayingAgainstComputer);

            m_GameController.CurrentPlayer = player1;
            m_GameController.m_GameBoard.CurrentTurnColor = ePieceColor.PlayerO;
        }

        private void initializeControls()
        {
            this.Text = "Checkers Game";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            int buttonSize = 60;
            int padding = 20;
            int boardSize = m_GameController.m_GameBoard.BoardSize;
            int formWidth = (boardSize * buttonSize) + (padding * 2);
            int formHeight = (boardSize * buttonSize) + (padding * 2) + 60;

            this.ClientSize = new Size(formWidth, formHeight);
            m_LabelPlayer1Score = new Label
            {
                Text = $"{m_GameController.m_Player1.Name}: {m_GameController.m_Player1.TotalPoints}",
                Location = new Point(padding + 60, padding / 2),
                AutoSize = true,
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.Black
            };

            m_LabelPlayer2Score = new Label
            {
                Text = $"{m_GameController.m_Player2.Name}: {m_GameController.m_Player2.TotalPoints}",
                Location = new Point(formWidth - 182, padding / 2),
                AutoSize = true,
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.Black
            };

            this.Controls.Add(m_LabelPlayer1Score);
            this.Controls.Add(m_LabelPlayer2Score);
            m_ButtonsBoard = new Button[boardSize, boardSize];

            for (int row = 0; row < boardSize; row++)
            {
                for (int col = 0; col < boardSize; col++)
                {
                    Button button = new Button
                    {
                        Size = new Size(buttonSize, buttonSize),
                        Location = new Point(col * buttonSize + padding, row * buttonSize + padding + 30),
                        FlatStyle = FlatStyle.Flat,
                        Tag = new Point(row, col)
                    };

                    button.BackColor = (row + col) % 2 == 0 ? Color.Black : Color.White;
                    button.Click += squareClick;
                    m_ButtonsBoard[row, col] = button;
                    this.Controls.Add(button);
                }
            }

            updateBoardDisplay();
        }

        private void updateBoardDisplay()
        {
            int boardSize = m_GameController.m_GameBoard.BoardSize;

            for (int row = 0; row < boardSize; row++)
            {
                for (int col = 0; col < boardSize; col++)
                {
                    Button button = m_ButtonsBoard[row, col];
                    GamePiece piece = m_GameController.m_GameBoard.Board[row, col];

                    button.Text = "";
                    if (piece != null)
                    {
                        button.Text = piece.GetSymbol().ToString();
                        button.ForeColor = piece.PieceColor == ePieceColor.PlayerO ? Color.Black : Color.Black;
                        button.Font = new Font(button.Font.FontFamily, 24, FontStyle.Bold);
                    }
                }
            }
        }

        private void updateScoreLabels()
        {
            m_LabelPlayer1Score.Text = $"{m_GameController.m_Player1.Name}: {m_GameController.m_Player1.TotalPoints}";
            m_LabelPlayer2Score.Text = $"{m_GameController.m_Player2.Name}: {m_GameController.m_Player2.TotalPoints}";
        }

        private void handleGameEnd(string i_GameEndMsg)
        {
            m_GameController.CalculateAndAssignPoints();
            updateScoreLabels();
            DialogResult result = MessageBox.Show(
     $"{i_GameEndMsg}{Environment.NewLine}{Environment.NewLine}Would you like to play again?",
     "Game Over",
     MessageBoxButtons.YesNo,
     MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                m_GameController.ResetGame();
                updateBoardDisplay();
            }
            else
            {
                this.Close();
            }
        }

        private void CheckersGameForm_Load(object sender, EventArgs e)
        {
        }


        private void highlightLegalMoves(int i_FromRow, int i_FromCol)
        {
            int boardSize = m_GameController.m_GameBoard.BoardSize;

            for (int row = 0; row < boardSize; row++)
            {
                for (int col = 0; col < boardSize; col++)
                {
                    if (m_GameController.m_GameBoard.IsMoveValid(i_FromRow, i_FromCol, row, col, m_GameController.CurrentPlayer))
                    {
                        m_ButtonsBoard[row, col].BackColor = Color.LightGreen;
                    }
                }
            }
        }

        private void showCapturedPiece(int i_FromRow, int i_FromCol, int i_ToRow, int i_ToCol)
        {
            if (Math.Abs(i_ToRow - i_FromRow) == 2 && Math.Abs(i_ToCol - i_FromCol) == 2)
            {
                int capturedRow = (i_ToRow + i_FromRow) / 2;
                int capturedCol = (i_ToCol + i_FromCol) / 2;
                int boardSize = m_GameController.m_GameBoard.BoardSize;

                if (capturedRow < 0 || capturedRow >= boardSize ||
                    capturedCol < 0 || capturedCol >= boardSize ||
                    m_ButtonsBoard[capturedRow, capturedCol] == null)
                {

                    return;
                }

                m_ButtonsBoard[capturedRow, capturedCol].BackColor = Color.Red;
                Timer resetColorTimer = new Timer { Interval = 500 };

                resetColorTimer.Tick += (sender, args) =>
                {
                    if (m_ButtonsBoard[capturedRow, capturedCol] != null)
                    {
                        m_ButtonsBoard[capturedRow, capturedCol].BackColor = (capturedRow + capturedCol) % 2 == 0 ? Color.Black : Color.White;
                        m_ButtonsBoard[capturedRow, capturedCol].Text = "";
                    }

                    resetColorTimer.Stop();
                    resetColorTimer.Dispose();
                };
                resetColorTimer.Start();
            }
        }

        private void resetBoardColors()
        {
            int boardSize = m_GameController.m_GameBoard.BoardSize;

            for (int row = 0; row < boardSize; row++)
            {
                for (int col = 0; col < boardSize; col++)
                {
                    m_ButtonsBoard[row, col].BackColor = (row + col) % 2 == 0 ? Color.Black : Color.White;
                }
            }
        }

        private void squareClick(object sender, EventArgs e)
        {
            if (m_GameController.m_IsCurrentPlayerComputer)
            {
                return;
            }

            Button clickedButton = (Button)sender;
            Point position = (Point)clickedButton.Tag;
            int row = position.X;
            int col = position.Y;

            if (m_SelectedPiece == null)
            {
                GamePiece piece = m_GameController.m_GameBoard.Board[row, col];

                if (piece != null && piece.PieceColor == m_GameController.CurrentPlayer.PlayerColor)
                {
                    m_SelectedPiece = clickedButton;
                    m_SelectedPiece.BackColor = Color.LightBlue;
                    highlightLegalMoves(row, col);
                }
            }
            else
            {
                if (m_SelectedPiece == clickedButton)
                {
                    resetBoardColors();
                    m_SelectedPiece = null;
                }
                else
                {
                    Point selectedPos = (Point)m_SelectedPiece.Tag;
                    string moveStr = string.Format("{0}{1}>{2}{3}",
                        m_GameController.m_GameBoard.GetRowLabel(selectedPos.X),
                        m_GameController.m_GameBoard.GetColumnLabel(selectedPos.Y),
                        m_GameController.m_GameBoard.GetRowLabel(row),
                        m_GameController.m_GameBoard.GetColumnLabel(col));
                    string errorMsg, outputMsg;
                    GamePiece originalPiece = m_GameController.m_GameBoard.Board[selectedPos.X, selectedPos.Y];
                    bool isRegularPiece = originalPiece != null && originalPiece.PieceType != ePieceType.King;

                    if (m_GameController.ProcessPlayerMove(moveStr, out errorMsg, out outputMsg) != null)
                    {
                        Button selectedButton = m_SelectedPiece;

                        resetBoardColors();
                        m_SelectedPiece = null;
                        m_Animator.StartMoveAnimation(
                            selectedButton,
                            selectedButton.Location,
                            m_ButtonsBoard[row, col].Location,
                            () =>
                            {
                                showCapturedPiece(selectedPos.X, selectedPos.Y, row, col);
                                GamePiece movedPiece = m_GameController.m_GameBoard.Board[row, col];
                                bool turnedKing = isRegularPiece && movedPiece != null && movedPiece.PieceType == ePieceType.King;

                                updateBoardDisplay();

                                if (turnedKing)
                                {
                                    m_Animator.StartKingAnimation(m_ButtonsBoard[row, col], completeMoveAndSwitchTurn);
                                }
                                else
                                {
                                    completeMoveAndSwitchTurn();
                                }
                            });
                    }
                    else if (!string.IsNullOrEmpty(errorMsg))
                    {
                        MessageBox.Show(errorMsg, "Invalid Move", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void completeMoveAndSwitchTurn()
        {
            bool isGameEnded;
            string gameEndMsg;

            m_GameController.SwitchTurn(out isGameEnded, out gameEndMsg);
            if (isGameEnded)
            {
                handleGameEnd(gameEndMsg);
            }
            else if (m_GameController.m_IsCurrentPlayerComputer)
            {
                string computerMoveOutput;
                string moveString = m_GameController.PerformComputerMove(out computerMoveOutput);

                if (!string.IsNullOrEmpty(moveString) && tryParseMoveString(moveString, out int startRow, out int startCol, out int endRow, out int endCol))
                {
                    Button sourceButton = m_ButtonsBoard[startRow, startCol];
                    Button targetButton = m_ButtonsBoard[endRow, endCol];
                    string previousSymbol = sourceButton.Text;

                    m_Animator.StartMoveAnimation(
                        sourceButton,
                        sourceButton.Location,
                        targetButton.Location,
                        () =>
                        {
                            showCapturedPiece(startRow, startCol, endRow, endCol);
                            updateBoardDisplay();
                            GamePiece movedPiece = m_GameController.m_GameBoard.Board[endRow, endCol];
                            bool turnedKing = movedPiece != null && movedPiece.PieceType == ePieceType.King;
                            string newSymbol = targetButton.Text;

                            if (turnedKing && previousSymbol != newSymbol)
                            {
                                m_Animator.StartKingAnimation(targetButton, () =>
                                {
                                    finalizeComputerTurn();
                                });
                            }
                            else
                            {
                                finalizeComputerTurn();
                            }
                        }
                    );
                }
            }
        }

        private bool tryParseMoveString(string i_MoveString, out int o_StartRow, out int o_StartCol, out int o_EndRow, out int o_EndCol)
        {
            bool isValidMove = false;

            o_StartRow = o_StartCol = o_EndRow = o_EndCol = -1;
            if (i_MoveString.Length == 5 && i_MoveString[2] == '>')
            {
                o_StartRow = i_MoveString[0] - 'a';
                o_StartCol = i_MoveString[1] - 'A';
                o_EndRow = i_MoveString[3] - 'a';
                o_EndCol = i_MoveString[4] - 'A';
                isValidMove = true;
            }

            return isValidMove;
        }

        private void finalizeComputerTurn()
        {
            bool isGameEnded;
            string gameEndMsg;

            m_GameController.SwitchTurn(out isGameEnded, out gameEndMsg);
            if (isGameEnded)
            {
                handleGameEnd(gameEndMsg);
            }
        }
    }
}










