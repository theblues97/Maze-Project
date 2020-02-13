using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace ResearchFinal
{

    public class ResolveMap
    {
        Maze maze;
        StackController sc;
        private List<Point> resList;

        private readonly CellState[,] matrix;
        private CellState[,] dirMatrix;

        public CellState[,] Matrix => matrix;

        public List<Point> ResList { get => resList; set => resList = value; }

        public ResolveMap(Point sPoint, Point ePoint)
        {
            maze = new Maze();
            matrix = maze.Matrix;

            Process(sPoint, ePoint);
        }
        public void Process(Point sPoint, Point ePoint)
        {
            maze.CreateDirMatrix();
            dirMatrix = maze.DirMatrix;
            sc = new StackController();
            resList = new List<Point>();

            sc.Init();
            Point pPoint = sPoint;

            while (pPoint != ePoint)
                pPoint = Directional(pPoint);
            resList.Add(ePoint);

            //DisplayResult();
        }

        Point Directional(Point p)
        {
            Point np = new Point();
            CellState toWall = 0;
            int c = 0, lastResid;
            if (!dirMatrix[p.X, p.Y].HasFlag(CellState.Left))
            {
                c++;
                np.X = p.X - 1;
                np.Y = p.Y;
                toWall = CellState.Left;
            }
            if (!dirMatrix[p.X, p.Y].HasFlag(CellState.Top))
            {
                c++;
                np.X = p.X;
                np.Y = p.Y - 1;
                toWall = CellState.Top;
            }
            if (!dirMatrix[p.X, p.Y].HasFlag(CellState.Right))
            {
                c++;
                np.X = p.X + 1;
                np.Y = p.Y;
                toWall = CellState.Right;
            }
            if (!dirMatrix[p.X, p.Y].HasFlag(CellState.Bottom))
            {
                c++;
                np.X = p.X;
                np.Y = p.Y + 1;
                toWall = CellState.Bottom;
            }

            lastResid = resList.Count() - 1;
            dirMatrix[p.X, p.Y] |= toWall;
            dirMatrix[np.X, np.Y] |= toWall.OppositeWall();

            if (c == 0)
            {
                Note stackpeak = sc.Peak();
                resList.RemoveRange(stackpeak.loc, lastResid - stackpeak.loc + 1);
                sc.Pop();
                return stackpeak.point;
            }
            else
            {
                resList.Add(p);
                if (c > 1)
                {
                    Note stacknote = new Note
                    { point = p, loc = lastResid + 1 };
                    sc.Push(stacknote);
                }
            }
            return np;
        }

        public void DisplayResult()
        {
            for (int y = 0; y < Content._height * 2 + 1; y++)
            {
                for (int x = 0; x < Content._width * 2 + 1; x++)
                {
                    Point tp = new Point(x, y);
                    if ((int)matrix[x, y] == 15)
                    {
                        if (x % 2 == 0 && y % 2 == 0)
                            Debug.Write("+");
                        else if (x % 2 != 0)
                            Debug.Write("-");
                        else
                            Debug.Write("|");
                    }
                    else
                    {
                        int t = 0;
                        foreach (var p in resList)
                        {
                            if (p == tp)
                            { Debug.Write("*"); t = 1; break; }
                        }
                        if (t != 1)
                            Debug.Write(" ");
                    }

                }
                Debug.WriteLine("");
            }
        }
    }
}
