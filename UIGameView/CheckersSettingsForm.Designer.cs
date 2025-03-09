namespace UIGameView
{
    partial class CheckersSettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private System.Windows.Forms.TextBox m_TextBoxPlayer1;
        private System.Windows.Forms.TextBox m_TextBoxPlayer2;
        private System.Windows.Forms.CheckBox m_CheckBoxPlayerOrComputer;
        private System.Windows.Forms.Button m_ButtonStart;
        private System.Windows.Forms.Label m_LabelPlayers;
        private System.Windows.Forms.Label m_LabelPlayer1;
        private System.Windows.Forms.Label m_LabelPlayer2;
        private System.Windows.Forms.Label m_LabelBoardSize;
        private System.Windows.Forms.RadioButton m_RadioButtonBoardSize6x6;
        private System.Windows.Forms.RadioButton m_RadioButtonBoardSize8x8;
        private System.Windows.Forms.RadioButton m_RadioButtonBoardSize10x10;
        private System.Windows.Forms.Label m_LabelBoardSize6x6;
        private System.Windows.Forms.Label m_LabelBoardSize8x8;
        private System.Windows.Forms.Label m_LabelBoardSize10x10;

        public void InitializeComponent()
        {
            this.m_TextBoxPlayer1 = new System.Windows.Forms.TextBox();
            this.m_TextBoxPlayer2 = new System.Windows.Forms.TextBox();
            this.m_CheckBoxPlayerOrComputer = new System.Windows.Forms.CheckBox();
            this.m_ButtonStart = new System.Windows.Forms.Button();
            this.m_LabelPlayer1 = new System.Windows.Forms.Label();
            this.m_LabelPlayers = new System.Windows.Forms.Label();
            this.m_LabelPlayer2 = new System.Windows.Forms.Label();
            this.m_LabelBoardSize = new System.Windows.Forms.Label();
            this.m_RadioButtonBoardSize6x6 = new System.Windows.Forms.RadioButton();
            this.m_RadioButtonBoardSize8x8 = new System.Windows.Forms.RadioButton();
            this.m_RadioButtonBoardSize10x10 = new System.Windows.Forms.RadioButton();
            this.m_LabelBoardSize6x6 = new System.Windows.Forms.Label();
            this.m_LabelBoardSize8x8 = new System.Windows.Forms.Label();
            this.m_LabelBoardSize10x10 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // m_TextBoxPlayer1
            // 
            this.m_TextBoxPlayer1.Location = new System.Drawing.Point(126, 88);
            this.m_TextBoxPlayer1.Name = "m_TextBoxPlayer1";
            this.m_TextBoxPlayer1.Size = new System.Drawing.Size(128, 26);
            this.m_TextBoxPlayer1.TabIndex = 1;
            this.m_TextBoxPlayer1.TextChanged += new System.EventHandler(this.m_TextBoxPlayer1_TextChanged);
            // 
            // m_TextBoxPlayer2
            // 
            this.m_TextBoxPlayer2.Enabled = false;
            this.m_TextBoxPlayer2.Location = new System.Drawing.Point(126, 139);
            this.m_TextBoxPlayer2.Name = "m_TextBoxPlayer2";
            this.m_TextBoxPlayer2.Size = new System.Drawing.Size(128, 26);
            this.m_TextBoxPlayer2.TabIndex = 3;
            this.m_TextBoxPlayer2.Text = "Computer";
            this.m_TextBoxPlayer2.TextChanged += new System.EventHandler(this.m_TextBoxPlayer2_TextChanged);
            // 
            // m_CheckBoxPlayerOrComputer
            // 
            this.m_CheckBoxPlayerOrComputer.AutoSize = true;
            this.m_CheckBoxPlayerOrComputer.Location = new System.Drawing.Point(32, 143);
            this.m_CheckBoxPlayerOrComputer.Name = "m_CheckBoxPlayerOrComputer";
            this.m_CheckBoxPlayerOrComputer.Size = new System.Drawing.Size(22, 21);
            this.m_CheckBoxPlayerOrComputer.TabIndex = 4;
            this.m_CheckBoxPlayerOrComputer.CheckedChanged += new System.EventHandler(this.m_CheckBoxPlayerOrComputer_CheckedChanged);
            // 
            // m_ButtonStart
            // 
            this.m_ButtonStart.Location = new System.Drawing.Point(154, 181);
            this.m_ButtonStart.Name = "m_ButtonStart";
            this.m_ButtonStart.Size = new System.Drawing.Size(100, 30);
            this.m_ButtonStart.TabIndex = 9;
            this.m_ButtonStart.Text = "Start Game";
            this.m_ButtonStart.UseVisualStyleBackColor = true;
            this.m_ButtonStart.Click += new System.EventHandler(this.m_ButtonStart_Click);
            // 
            // m_LabelPlayer1
            // 
            this.m_LabelPlayer1.AutoSize = true;
            this.m_LabelPlayer1.Location = new System.Drawing.Point(28, 94);
            this.m_LabelPlayer1.Name = "m_LabelPlayer1";
            this.m_LabelPlayer1.Size = new System.Drawing.Size(69, 20);
            this.m_LabelPlayer1.TabIndex = 0;
            this.m_LabelPlayer1.Text = "Player 1:";
            this.m_LabelPlayer1.Click += new System.EventHandler(this.m_LabelPlayer1_Click);
            // 
            // m_LabelPlayers
            // 
            this.m_LabelPlayers.AutoSize = true;
            this.m_LabelPlayers.Location = new System.Drawing.Point(8, 59);
            this.m_LabelPlayers.Name = "m_LabelPlayers";
            this.m_LabelPlayers.Size = new System.Drawing.Size(64, 20);
            this.m_LabelPlayers.TabIndex = 10;
            this.m_LabelPlayers.Text = "Players:";
            this.m_LabelPlayers.Click += new System.EventHandler(this.m_LabelPlayers_Click);
            // 
            // m_LabelPlayer2
            // 
            this.m_LabelPlayer2.AutoSize = true;
            this.m_LabelPlayer2.Location = new System.Drawing.Point(51, 144);
            this.m_LabelPlayer2.Name = "m_LabelPlayer2";
            this.m_LabelPlayer2.Size = new System.Drawing.Size(69, 20);
            this.m_LabelPlayer2.TabIndex = 2;
            this.m_LabelPlayer2.Text = "Player 2:";
            this.m_LabelPlayer2.Click += new System.EventHandler(this.m_LabelPlayer2_Click);
            // 
            // m_LabelBoardSize
            // 
            this.m_LabelBoardSize.AutoSize = true;
            this.m_LabelBoardSize.Location = new System.Drawing.Point(8, 9);
            this.m_LabelBoardSize.Name = "m_LabelBoardSize";
            this.m_LabelBoardSize.Size = new System.Drawing.Size(91, 20);
            this.m_LabelBoardSize.TabIndex = 5;
            this.m_LabelBoardSize.Text = "Board Size:";
            this.m_LabelBoardSize.Click += new System.EventHandler(this.m_LabelBoardSize_Click);
            // 
            // m_RadioButtonBoardSize6x6
            // 
            this.m_RadioButtonBoardSize6x6.AutoSize = true;
            this.m_RadioButtonBoardSize6x6.Checked = true;
            this.m_RadioButtonBoardSize6x6.Location = new System.Drawing.Point(32, 32);
            this.m_RadioButtonBoardSize6x6.Name = "m_RadioButtonBoardSize6x6";
            this.m_RadioButtonBoardSize6x6.Size = new System.Drawing.Size(21, 20);
            this.m_RadioButtonBoardSize6x6.TabIndex = 6;
            this.m_RadioButtonBoardSize6x6.TabStop = true;
            this.m_RadioButtonBoardSize6x6.CheckedChanged += new System.EventHandler(this.m_RadioButtonBoardSize6x6_CheckedChanged);
            // 
            // m_RadioButtonBoardSize8x8
            // 
            this.m_RadioButtonBoardSize8x8.AutoSize = true;
            this.m_RadioButtonBoardSize8x8.Location = new System.Drawing.Point(105, 32);
            this.m_RadioButtonBoardSize8x8.Name = "m_RadioButtonBoardSize8x8";
            this.m_RadioButtonBoardSize8x8.Size = new System.Drawing.Size(21, 20);
            this.m_RadioButtonBoardSize8x8.TabIndex = 7;
            this.m_RadioButtonBoardSize8x8.CheckedChanged += new System.EventHandler(this.m_RadioButtonBoardSize8x8_CheckedChanged);
            // 
            // m_RadioButtonBoardSize10x10
            // 
            this.m_RadioButtonBoardSize10x10.AutoSize = true;
            this.m_RadioButtonBoardSize10x10.Location = new System.Drawing.Point(181, 32);
            this.m_RadioButtonBoardSize10x10.Name = "m_RadioButtonBoardSize10x10";
            this.m_RadioButtonBoardSize10x10.Size = new System.Drawing.Size(21, 20);
            this.m_RadioButtonBoardSize10x10.TabIndex = 8;
            this.m_RadioButtonBoardSize10x10.CheckedChanged += new System.EventHandler(this.m_RadioButtonBoardSize10x10_CheckedChanged);
            // 
            // m_LabelBoardSize6x6
            // 
            this.m_LabelBoardSize6x6.AutoSize = true;
            this.m_LabelBoardSize6x6.Location = new System.Drawing.Point(50, 31);
            this.m_LabelBoardSize6x6.Name = "m_LabelBoardSize6x6";
            this.m_LabelBoardSize6x6.Size = new System.Drawing.Size(42, 20);
            this.m_LabelBoardSize6x6.TabIndex = 7;
            this.m_LabelBoardSize6x6.Text = "6 x 6";
            this.m_LabelBoardSize6x6.Click += new System.EventHandler(this.m_LabelBoardSize6x6_Click);
            // 
            // m_LabelBoardSize8x8
            // 
            this.m_LabelBoardSize8x8.AutoSize = true;
            this.m_LabelBoardSize8x8.Location = new System.Drawing.Point(123, 31);
            this.m_LabelBoardSize8x8.Name = "m_LabelBoardSize8x8";
            this.m_LabelBoardSize8x8.Size = new System.Drawing.Size(42, 20);
            this.m_LabelBoardSize8x8.TabIndex = 8;
            this.m_LabelBoardSize8x8.Text = "8 x 8";
            this.m_LabelBoardSize8x8.Click += new System.EventHandler(this.m_LabelBoardSize8x8_Click);
            // 
            // m_LabelBoardSize10x10
            // 
            this.m_LabelBoardSize10x10.AutoSize = true;
            this.m_LabelBoardSize10x10.Location = new System.Drawing.Point(199, 31);
            this.m_LabelBoardSize10x10.Name = "m_LabelBoardSize10x10";
            this.m_LabelBoardSize10x10.Size = new System.Drawing.Size(60, 20);
            this.m_LabelBoardSize10x10.TabIndex = 9;
            this.m_LabelBoardSize10x10.Text = "10 x 10";
            this.m_LabelBoardSize10x10.Click += new System.EventHandler(this.m_LabelBoardSize10x10_Click);
            // 
            // CheckersSettingsForm
            // 
            this.ClientSize = new System.Drawing.Size(263, 224);
            this.Controls.Add(this.m_LabelPlayer1);
            this.Controls.Add(this.m_TextBoxPlayer1);
            this.Controls.Add(this.m_LabelPlayer2);
            this.Controls.Add(this.m_TextBoxPlayer2);
            this.Controls.Add(this.m_CheckBoxPlayerOrComputer);
            this.Controls.Add(this.m_LabelBoardSize);
            this.Controls.Add(this.m_RadioButtonBoardSize6x6);
            this.Controls.Add(this.m_LabelBoardSize6x6);
            this.Controls.Add(this.m_RadioButtonBoardSize8x8);
            this.Controls.Add(this.m_LabelBoardSize8x8);
            this.Controls.Add(this.m_RadioButtonBoardSize10x10);
            this.Controls.Add(this.m_LabelBoardSize10x10);
            this.Controls.Add(this.m_ButtonStart);
            this.Controls.Add(this.m_LabelPlayers);
            this.Name = "CheckersSettingsForm";
            this.Text = "GameSetting";
            this.Load += new System.EventHandler(this.CheckersSettingsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
    }
}


