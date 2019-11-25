using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResearchFinal
{
    public partial class Form1 : Form
    {
        Maze maze;
        public Form1()
        {
            InitializeComponent();
            maze = new Maze(20, 20);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            maze.Display();
            maze.Load();
        }
    }
}
