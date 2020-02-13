using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResearchFinal
{
    public partial class Home : Form
    {
        GameManager gameManager;

        public Home()
        {
            InitializeComponent();

            gameManager = new GameManager(pnlBoard, lblTimer);
            startGameToolStripMenuItem.Click += gameManager.btnStart_Click;
            autoRunToolStripMenuItem.Click += gameManager.btnAuto_Click;
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlBoard.Controls.Clear();
            lblTimer.Text = "00:00";
            gameManager = new GameManager(pnlBoard, lblTimer);
            startGameToolStripMenuItem.Click += gameManager.btnStart_Click;
            autoRunToolStripMenuItem.Click += gameManager.btnAuto_Click;
        }

        //private void Home_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.Control && e.KeyCode == Keys.N)
        //        newGameToolStripMenuItem.PerformClick();

        //    if (e.Control && e.KeyCode == Keys.S)
        //        startGameToolStripMenuItem.PerformClick();

        //    if (e.Control && e.KeyCode == Keys.R)
        //        autoRunToolStripMenuItem.PerformClick();
        //}

        private void DisplayScoreBoard()
        {
            string mes = "";
            try
            {
                List<string> scores = File.ReadAllLines(Properties.Resources.highscore).ToList();

                int i = 1;
                foreach (var line in scores)
                {
                    int sec = Int32.Parse(line);
                    int min = sec / 60;
                    int displaySec = sec - min * 60;
                    string ssec = displaySec < 10 ? "0" + displaySec.ToString() : displaySec.ToString();
                    string smin = min < 10 ? "0" + min.ToString() : min.ToString();
                    mes += i + "\t" + smin + ":" + ssec + "\n";
                    i++;
                }
            }
            catch{ }
            MessageBox.Show(mes, "High Score Board", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void highScoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisplayScoreBoard();
        }

        private void resetHighScoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            File.Delete(Properties.Resources.highscore);
        }
    }
}
