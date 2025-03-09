using System.Windows.Forms;

namespace UIGameView
{
    public static class GameUIManager
    {
        public static void Run()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            CheckersSettingsForm settingsForm = new CheckersSettingsForm();

            if (settingsForm.ShowDialog() == DialogResult.OK)
            {
                CheckersGameForm gameForm = new CheckersGameForm(settingsForm);

                Application.Run(gameForm);
            }
        }
    }
}
