using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ResearchFinal
{
    public static class Extensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
        {
            var e = source.ToArray();
            for (var i = e.Length - 1; i >= 0; i--)
            {
                var swapIndex = rng.Next(i + 1);
                yield return e[swapIndex];
                e[swapIndex] = e[i];
            }
        }

        public static CellState OppositeWall(this CellState orig)
        {
            return (CellState)(((int)orig >> 2) | ((int)orig << 2)) & CellState.Initial;
        }

        public static bool HasFlag(this CellState cs, CellState flag)
        {
            return ((int)cs & (int)flag) != 0;
        }
    }

    [Flags]
    public enum CellState
    {
        Top = 1,
        Right = 2,
        Bottom = 4,
        Left = 8,
        Visited = 128,
        Initial = Top | Right | Bottom | Left,
    }

    public struct RemoveWallAction
    {
        public Point Neighbour;
        public CellState Wall;
    }
    public class Maze
    {
        private readonly CellState[,] _cells;
        private readonly int _width;
        private readonly int _height;
        private readonly Random _rng;

        StackController sc;
        private List<Point> resList;

        private readonly CellState[,] matrix;

        private static CellState[,] dirMatrix;

        public Maze(int width, int height)
        {
            _width = width;
            _height = height;
            _cells = new CellState[width, height];
            for (var x = 0; x < width; x++)
                for (var y = 0; y < height; y++)
                    _cells[x, y] = CellState.Initial;
            _rng = new Random();
            VisitCell(_rng.Next(width), _rng.Next(height));

            //Display1();

            matrix = new CellState[width * 2 + 1, height * 2 + 1];
            for (var x = 0; x < width * 2 + 1; x++)
                for (var y = 0; y < height * 2 + 1; y++)
                    matrix[x, y] = CellState.Initial;

            for (var x = 0; x < width; x++)
                for (var y = 0; y < height; y++)
                {
                    matrix[x * 2 + 1, y * 2 + 1] = _cells[x, y];
                    if (!_cells[x, y].HasFlag(CellState.Top)) matrix[x * 2 + 1, y * 2] -= CellState.Bottom;
                    if (!_cells[x, y].HasFlag(CellState.Left)) matrix[x * 2, y * 2 + 1] -= CellState.Right;
                    if (!_cells[x, y].HasFlag(CellState.Right)) matrix[x * 2 + 2, y * 2 + 1] -= CellState.Left;
                    if (!_cells[x, y].HasFlag(CellState.Bottom)) matrix[x * 2 + 1, y * 2 + 2] -= CellState.Top;
                }


            dirMatrix = new CellState[width * 2 + 1, height * 2 + 1];
            for (var x = 0; x < width * 2 + 1; x++)
                for (var y = 0; y < height * 2 + 1; y++)
                    dirMatrix[x, y] = this[x, y];

            Process();
        }

        public CellState this[int x, int y]
        {
            get { return matrix[x, y]; }
        }

        public IEnumerable<RemoveWallAction> GetNeighbours(Point p)
        {
            if (p.X > 0) yield return new RemoveWallAction { Neighbour = new Point(p.X - 1, p.Y), Wall = CellState.Left };
            if (p.Y > 0) yield return new RemoveWallAction { Neighbour = new Point(p.X, p.Y - 1), Wall = CellState.Top };
            if (p.X < _width - 1) yield return new RemoveWallAction { Neighbour = new Point(p.X + 1, p.Y), Wall = CellState.Right };
            if (p.Y < _height - 1) yield return new RemoveWallAction { Neighbour = new Point(p.X, p.Y + 1), Wall = CellState.Bottom };
        }

        public void VisitCell(int x, int y)
        {
            _cells[x, y] |= CellState.Visited;
            foreach (var p in GetNeighbours(new Point(x, y)).Shuffle(_rng).Where(z => !(_cells[z.Neighbour.X, z.Neighbour.Y].HasFlag(CellState.Visited))))
            {
                _cells[x, y] -= p.Wall;
                _cells[p.Neighbour.X, p.Neighbour.Y] -= p.Wall.OppositeWall();
                VisitCell(p.Neighbour.X, p.Neighbour.Y);
            }
        }

        //

        private void Process()
        {
            sc = new StackController();
            resList = new List<Point>();

            sc.Init();
            Point sPoint = Content._sPoint;
            Point ePoint = Content._ePoint;
            Point rPoint = sPoint;

            while (rPoint != ePoint)
                rPoint = Directional(rPoint);
            resList.Add(ePoint);

            DisplayResult();
        }

        Point Directional(Point p)
        {
            Point np = new Point();
            CellState toWall = 0;
            int c = 0, resid;
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

            resid = resList.Count() - 1;
            dirMatrix[p.X, p.Y] |= toWall;
            dirMatrix[np.X, np.Y] |= toWall.OppositeWall();

            if (c == 0)
            {
                Note stackpeak = sc.Peak();
                resList.RemoveRange(stackpeak.loc, resid - stackpeak.loc + 1);
                sc.Pop();
                return stackpeak.point;
            }
            else
            {
                resList.Add(p);
                if (c > 1)
                {
                    Note stacknote = new Note();
                    stacknote.point = p;
                    stacknote.loc = resid + 1;
                    sc.Push(stacknote);
                }
            }
            return np;
        }

        //public List<List<int>> Load()
        //{
        //    matrix = new List<List<int>>();

        //    int[] b1 = { 1, 1 }, b2 = { 1, 0 }, b3 = { 0, 0 };
        //    List<int> B1 = new List<int>(b1), B2 = new List<int>(b2), B3 = new List<int>(b3);

        //    List<int> lLast = new List<int>();

        //    for (var y = 0; y < _height; y++)
        //    {
        //        List<int> lTop = new List<int>(), lLeft = new List<int>();

        //        for (var x = 0; x < _width; x++)
        //        {

        //            if (this[x, y].HasFlag(CellState.Top))
        //                lTop.AddRange(B1);
        //            else
        //                lTop.AddRange(B2);

        //            if (this[x, y].HasFlag(CellState.Left))
        //                lLeft.AddRange(B2);
        //            else
        //                lLeft.AddRange(B3);
        //        }
        //        if (lLast.Count() == 0)
        //            lLast = lTop;

        //        lTop.Add(1);
        //        lLeft.Add(1);

        //        matrix.Add(new List<int>());
        //        matrix[y * 2].AddRange(lTop);

        //        matrix.Add(new List<int>());
        //        matrix[y * 2 + 1].AddRange(lLeft);
        //    }
        //    matrix.Add(new List<int>());
        //    matrix[(_height - 1) * 2 + 2].AddRange(lLast);

        //    //foreach (List<int> ll in matrix)
        //    //{
        //    //    ll.ForEach(items => Debug.Write(items));
        //    //    Debug.WriteLine("");
        //    //}
        //    return matrix;
        //}

        public void Display1()
        {
            var firstLine = string.Empty;
            for (var y = 0; y < _height; y++)
            {
                var sbTop = new StringBuilder();
                var sbMid = new StringBuilder();
                for (var x = 0; x < _width; x++)
                {
                    sbTop.Append(_cells[x, y].HasFlag(CellState.Top) ? "+-" : "+ ");
                    sbMid.Append(_cells[x, y].HasFlag(CellState.Left) ? "| " : "  ");
                }
                if (firstLine == string.Empty)
                {
                    firstLine = sbTop.ToString();
                }
                Debug.WriteLine(sbTop + "+");
                Debug.WriteLine(sbMid + "|");
            }
            Debug.Write(firstLine);
            Debug.WriteLine("+");
        }

        public void Display2()
        {
            for (var y = 0; y < Content._height * 2 + 1; y++)
            {
                for (var x = 0; x < Content._width * 2 + 1; x++)
                {
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
                        Debug.Write(" ");
                }
                Debug.WriteLine("");
            }
        }

        public void DisplayResult()
        {
            for (var y = 0; y < Content._height * 2 + 1; y++)
            {
                for (var x = 0; x < Content._width * 2 + 1; x++)
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
