using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResearchFinal
{
    public class GameManager
    {

        private Panel board;
        private Label lbltimer;
        private int sec;

        private System.Windows.Forms.Timer timer;
        private ResolveMap resolveMap;
        private readonly CellState[,] matrix;
        private List<Point> resList;
        private List<List<PictureBox>> picMap;

        private readonly int width;
        private readonly int height;
        private readonly int pWidth;
        private readonly int pHeight;
        private Point sPoint;
        private Point ePoint;
        private Point pPoint;

        public Panel Board { get => board; set => board = value; }
        public Label Lbltimer { get => lbltimer; set => lbltimer = value; }

        public GameManager(Panel _board, Label _lblTimer)
        {
            this.Board = _board;
            this.Lbltimer = _lblTimer;
            sec = 0;
            sPoint = Content._sPoint;
            ePoint = Content._ePoint;
            pPoint = sPoint;

            resolveMap = new ResolveMap(sPoint, ePoint);
            matrix = resolveMap.Matrix;
            resList = resolveMap.ResList;

            width = Content._width;
            height = Content._height;
            pWidth = Content._pWidth;
            pHeight = Content._pHeight;

            DrawBoard();
            picMap[sPoint.Y][sPoint.X].BackgroundImage = Properties.Resources.human;
            picMap[ePoint.Y][ePoint.X].BackgroundImage = Properties.Resources.finish;

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += new EventHandler(timer_Tick);
        }

        private void DrawBoard()
        {
            
            picMap = new List<List<PictureBox>>();
            for (int y = 0; y < height * 2 + 1; y++)
            {
                int horBorder = (board.Width - Content._pWidth * (width * 2 + 1)) / 2;
                int verBorder = (board.Height - Content._pHeight * (height * 2 + 1)) / 2;

                picMap.Add(new List<PictureBox>());
                for (int x = 0; x < width * 2 + 1; x++)
                {
                    Image img = null;
                    if ((int)matrix[x, y] == 15)
                        img = Properties.Resources.brick;

                    PictureBox pic = new PictureBox()
                    {
                        Width = pWidth,
                        Height = pHeight,
                        Location = new Point(x * pWidth + horBorder, y * pHeight + verBorder),
                        BackgroundImageLayout = ImageLayout.Stretch,
                        BackgroundImage = img,
                        Tag = y.ToString()
                    };
                    Board.Controls.Add(pic);
                    picMap[y].Add(pic);
                }
            }
        }

        public Point Directional(Point p, PreviewKeyDownEventArgs e)
        {
            Point np = p;
            switch (e.KeyCode)
            {
                case Keys.Right:
                    np.X++;
                    break;
                case Keys.Left:
                    np.X--;
                    break;
                case Keys.Down:
                    np.Y++;
                    break;
                case Keys.Up:
                    np.Y--;
                    break;
            }
            if ((int)matrix[np.X, np.Y] != 15)
            {
                picMap[p.Y][p.X].BackgroundImage = null;
                return np;
            }
            return p;
        }

        private bool isEnd(Point p)
        {
            return p == ePoint;
        }

        private void AutoRun()
        {
            for (int i = 1; i < resList.Count(); i++)
            {

                picMap[resList[i].Y][resList[i].X].BackgroundImage = Properties.Resources.human;
                picMap[resList[i - 1].Y][resList[i - 1].X].BackgroundImage = null;

                picMap[resList[i].Y][resList[i].X].Refresh();
                picMap[resList[i - 1].Y][resList[i - 1].X].Refresh();
                Thread.Sleep(100);

                if (isEnd(resList[i]))
                {
                    picMap[resList[i].Y][resList[i].X].BackgroundImage = Properties.Resources.finish;
                    MessageBox.Show("Congratulation, you win!", "Notification!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                }
            }
        }

        private void ScoreProcessor()
        {
            List<string> score;
            try { score = File.ReadAllLines(Properties.Resources.highscore).ToList(); }
            catch { score = new List<string>(); }

            score.Add(sec.ToString());
            score.Sort();
            if (score.Count > 10)
                score.RemoveAt(10);

            using (StreamWriter file = new StreamWriter(Properties.Resources.highscore))
            {
                foreach (var line in score)
                {
                    file.WriteLine(line);
                }
            }
        }

        private void pnlBoard_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            pPoint = Directional(pPoint, e);
            picMap[pPoint.Y][pPoint.X].BackgroundImage = Properties.Resources.human;

            if (isEnd(pPoint))
            {               
                timer.Stop();
                ScoreProcessor();
                picMap[pPoint.Y][pPoint.X].BackgroundImage = Properties.Resources.finish;
                pPoint = new Point(1, 1);
                MessageBox.Show("Congratulation, you win!", "Notification!", MessageBoxButtons.OK, MessageBoxIcon.Information);               
            }
        }

        public void btnStart_Click(object sender, EventArgs e)
        {
            pPoint = sPoint;
            Board.PreviewKeyDown += pnlBoard_PreviewKeyDown;
            Board.Focus();

            timer.Start();
        }

        public void btnAuto_Click(object sender, EventArgs e)
        {
            timer.Stop();
            if (pPoint != sPoint)
            {
                resolveMap.Process(pPoint, ePoint);
                resList = resolveMap.ResList;
            }
            Board.Focus();
            AutoRun();          
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            sec++;
            int min = sec / 60;
            int displaySec = sec - min * 60;
            string ssec = displaySec < 10 ? "0" + displaySec.ToString() : displaySec.ToString();
            string smin = min < 10 ? "0" + min.ToString() : min.ToString();
            Lbltimer.Text = smin + ":" + ssec;
        }
    }
}
