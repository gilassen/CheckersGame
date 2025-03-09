using GameLogic;
using System;
using System.Windows.Forms;

namespace UIGameView
{
    public partial class CheckersSettingsForm : Form
    {
        public CheckersSettingsForm()
        {
            InitializeComponent();
            attachEventHandlers();
        }

        public int BoardSize
        {
            get
            {
                int boardSize = 6;

                if (m_RadioButtonBoardSize8x8.Checked)
                {
                    boardSize = 8;
                }
                else if (m_RadioButtonBoardSize10x10.Checked)
                {
                    boardSize = 10;
                }

                return boardSize;
            }
        }

        public string Player1Name
        {
            get
            {

                return m_TextBoxPlayer1.Text;
            }
        }

        public string Player2Name
        {
            get
            {

                return m_TextBoxPlayer2.Text;
            }
        }

        public bool IsPlayingAgainstComputer
        {
            get
            {

                return !m_CheckBoxPlayerOrComputer.Checked;
            }
        }

        private void attachEventHandlers()
        {
            m_ButtonStart.Click += startButtonClick;
            m_CheckBoxPlayerOrComputer.CheckedChanged += playerVsComputerCheckedChanged;
        }

        private void playerVsComputerCheckedChanged(object sender, EventArgs e)
        {
            m_TextBoxPlayer2.Enabled = m_CheckBoxPlayerOrComputer.Checked;
            if (m_CheckBoxPlayerOrComputer.Checked)
            {
                m_TextBoxPlayer2.Text = string.Empty;
            }
            else
            {
                m_TextBoxPlayer2.Text = "Computer";
            }
        }

        private void startButtonClick(object sender, EventArgs e)
        {
            if (IsSettingsValid())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        public bool IsSettingsValid()
        {
            bool isValid = true;
            string playerName;

            if (!GameBoard.ValidatePlayerName(m_TextBoxPlayer1.Text, out playerName))
            {
                showMissingPlayerNameMessage($"Player 1: Invalid name. Please use only English letters, no spaces, and max length of {GameBoard.k_MaxNameLength}");
                isValid = false;
            }
            else
            {
                m_TextBoxPlayer1.Text = playerName;
            }
            if (m_CheckBoxPlayerOrComputer.Checked)
            {
                if (!GameBoard.ValidatePlayerName(m_TextBoxPlayer2.Text, out playerName))
                {
                    showMissingPlayerNameMessage($"Player 2: Invalid name. Please use only English letters, no spaces, and max length of {GameBoard.k_MaxNameLength}");
                    isValid = false;
                }
                else
                {
                    m_TextBoxPlayer2.Text = playerName;
                }
            }

            return isValid;
        }

        private void showMissingPlayerNameMessage(string i_playerName)
        {
            MessageBox.Show($"Please enter {i_playerName} name", "Missing Information",
            MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void CheckersSettingsForm_Load(object sender, EventArgs e)
        {

        }

        private void m_LabelBoardSize_Click(object sender, EventArgs e)
        {

        }

        private void m_RadioButtonBoardSize6x6_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void m_RadioButtonBoardSize8x8_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void m_LabelBoardSize6x6_Click(object sender, EventArgs e)
        {

        }

        private void m_LabelBoardSize8x8_Click(object sender, EventArgs e)
        {

        }

        private void m_RadioButtonBoardSize10x10_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void m_LabelPlayer1_Click(object sender, EventArgs e)
        {

        }

        private void m_LabelPlayers_Click(object sender, EventArgs e)
        {

        }

        private void m_LabelPlayer2_Click(object sender, EventArgs e)
        {

        }

        private void m_TextBoxPlayer2_TextChanged(object sender, EventArgs e)
        {

        }

        private void m_ButtonStart_Click(object sender, EventArgs e)
        {

        }

        private void m_CheckBoxPlayerOrComputer_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void m_TextBoxPlayer1_TextChanged(object sender, EventArgs e)
        {

        }

        private void m_LabelBoardSize10x10_Click(object sender, EventArgs e)
        {

        }
    }
}

