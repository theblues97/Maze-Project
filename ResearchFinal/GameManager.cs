using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace ResearchFinal
{

    public class GameManager
    {
        Maze maze;

        public GameManager()
        {
            maze = new Maze(20, 20);
        }
    }
}
