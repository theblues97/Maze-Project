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

        private List<List<int>> matrix;
        public List<List<int>> Matrix { get => matrix; set => matrix = value; }

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
        }

        public CellState this[int x, int y]
        {
            get { return _cells[x, y]; }
            set { _cells[x, y] = value; }
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
            this[x, y] |= CellState.Visited;
            foreach (var p in GetNeighbours(new Point(x, y)).Shuffle(_rng).Where(z => !(this[z.Neighbour.X, z.Neighbour.Y].HasFlag(CellState.Visited))))
            {
                this[x, y] -= p.Wall;
                this[p.Neighbour.X, p.Neighbour.Y] -= p.Wall.OppositeWall();
                VisitCell(p.Neighbour.X, p.Neighbour.Y);
            }
        }

        public List<List<int>> Load()
        {
            matrix = new List<List<int>>();

            int[] b1 = { 1, 1 }, b2 = { 1, 0 }, b3 = { 0, 0 };
            List<int> B1 = new List<int>(b1), B2 = new List<int>(b2), B3 = new List<int>(b3);

            List<int> lLast = new List<int>();

            for (var y = 0; y < _height; y++)
            {
                List<int> lTop = new List<int>(), lLeft = new List<int>();
                
                for (var x = 0; x < _width; x++)
                {

                    if (this[x, y].HasFlag(CellState.Top))
                        lTop.AddRange(B1);
                    else
                        lTop.AddRange(B2);

                    if (this[x, y].HasFlag(CellState.Left))
                        lLeft.AddRange(B2);
                    else
                        lLeft.AddRange(B3);
                }
                if (lLast.Count() == 0)
                    lLast = lTop;

                lTop.Add(1);
                lLeft.Add(1);

                matrix.Add(new List<int>());
                matrix[y * 2].AddRange(lTop);

                matrix.Add(new List<int>());
                matrix[y * 2 + 1].AddRange(lLeft);
            }
            matrix.Add(new List<int>());
            matrix[(_height - 1) * 2 + 2].AddRange(lLast);

            //foreach (List<int> ll in matrix)
            //{
            //    ll.ForEach(items => Debug.Write(items));
            //    Debug.WriteLine("");
            //}
            return matrix;
        }

        public void Display()
        {
            var firstLine = string.Empty;
            for (var y = 0; y < _height; y++)
            {
                var sbTop = new StringBuilder();
                var sbMid = new StringBuilder();
                for (var x = 0; x < _width; x++)
                {
                    sbTop.Append(this[x, y].HasFlag(CellState.Top) ? "11" : "10");
                    sbMid.Append(this[x, y].HasFlag(CellState.Left) ? "10" : "00");
                }
                if (firstLine == string.Empty)
                {
                    firstLine = sbTop.ToString();
                }
                Debug.WriteLine(sbTop + "1");
                Debug.WriteLine(sbMid + "1");
            }
            Debug.Write(firstLine);
            Debug.WriteLine(1);
        }
    }
}
