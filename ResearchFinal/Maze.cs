using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;

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
        private readonly CellState[,] cells;
        private readonly int width;
        private readonly int height;
        private readonly Random rng;

        private readonly CellState[,] matrix;
        private CellState[,] dirMatrix;

        public CellState[,] Matrix => matrix;

        public CellState[,] DirMatrix => dirMatrix;

        public Maze()
        {
            width = Content._width;
            height = Content._height;
            cells = new CellState[width, height];
            for (var x = 0; x < width; x++)
                for (var y = 0; y < height; y++)
                    cells[x, y] = CellState.Initial;
            rng = new Random();
            VisitCell(rng.Next(width), rng.Next(height));

            //Display1();

            matrix = new CellState[width * 2 + 1, height * 2 + 1];
            for (var x = 0; x < width * 2 + 1; x++)
                for (var y = 0; y < height * 2 + 1; y++)
                    matrix[x, y] = CellState.Initial;

            for (var x = 0; x < width; x++)
                for (var y = 0; y < height; y++)
                {
                    matrix[x * 2 + 1, y * 2 + 1] = cells[x, y];
                    if (!cells[x, y].HasFlag(CellState.Top)) matrix[x * 2 + 1, y * 2] -= CellState.Bottom;
                    if (!cells[x, y].HasFlag(CellState.Left)) matrix[x * 2, y * 2 + 1] -= CellState.Right;
                    if (!cells[x, y].HasFlag(CellState.Right)) matrix[x * 2 + 2, y * 2 + 1] -= CellState.Left;
                    if (!cells[x, y].HasFlag(CellState.Bottom)) matrix[x * 2 + 1, y * 2 + 2] -= CellState.Top;
                }
        }

        public void CreateDirMatrix()
        {
            dirMatrix = new CellState[width * 2 + 1, height * 2 + 1];
            for (var x = 0; x < width * 2 + 1; x++)
                for (var y = 0; y < height * 2 + 1; y++)
                    dirMatrix[x, y] = this[x, y];
        }

        public CellState this[int x, int y]
        {
            get { return matrix[x, y]; }
        }

        public IEnumerable<RemoveWallAction> GetNeighbours(Point p)
        {
            if (p.X > 0) yield return new RemoveWallAction { Neighbour = new Point(p.X - 1, p.Y), Wall = CellState.Left };
            if (p.Y > 0) yield return new RemoveWallAction { Neighbour = new Point(p.X, p.Y - 1), Wall = CellState.Top };
            if (p.X < width - 1) yield return new RemoveWallAction { Neighbour = new Point(p.X + 1, p.Y), Wall = CellState.Right };
            if (p.Y < height - 1) yield return new RemoveWallAction { Neighbour = new Point(p.X, p.Y + 1), Wall = CellState.Bottom };
        }

        public void VisitCell(int x, int y)
        {
            cells[x, y] |= CellState.Visited;
            foreach (var p in GetNeighbours(new Point(x, y)).Shuffle(rng).Where(z => !(cells[z.Neighbour.X, z.Neighbour.Y].HasFlag(CellState.Visited))))
            {
                cells[x, y] -= p.Wall;
                cells[p.Neighbour.X, p.Neighbour.Y] -= p.Wall.OppositeWall();
                VisitCell(p.Neighbour.X, p.Neighbour.Y);
            }
        }
        public void Display1()
        {
            var firstLine = string.Empty;
            for (var y = 0; y < height; y++)
            {
                var sbTop = new StringBuilder();
                var sbMid = new StringBuilder();
                for (var x = 0; x < width; x++)
                {
                    sbTop.Append(cells[x, y].HasFlag(CellState.Top) ? "+-" : "+ ");
                    sbMid.Append(cells[x, y].HasFlag(CellState.Left) ? "| " : "  ");
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
            for (var y = 0; y < height * 2 + 1; y++)
            {
                for (var x = 0; x < width * 2 + 1; x++)
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
    }
}
