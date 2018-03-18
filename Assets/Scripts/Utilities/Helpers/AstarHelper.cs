/*
Unity C# Port of Andrea Giammarchi's JavaScript A* algorithm (http://devpro.it/javascript_id_137.html)
Usage:
 
int[,] map = new int[,]
{
    { 0, 0, 0, 0, 0, 0, 0, 0},
    { 1, 1, 1, 1, 0, 0, 0, 0},
    { 0, 0, 0, 1, 0, 0, 0, 0},
    { 0, 0, 0, 1, 0, 0, 0, 0},
    { 0, 0, 0, 1, 0, 0, 0, 0},
    { 1, 0, 0, 0, 0, 0, 0, 0},
    { 1, 0, 0, 0, 0, 0, 0, 0},
    { 1, 1, 0, 1, 1, 1, 0, 0},
    { 1, 0, 1, 0, 0, 0, 0, 0},
    { 1, 0, 1, 0, 0, 0, 0, 0}
};
Vector2Int start = new Vector2Int(0, 0);
Vector2Int end = new Vector2Int(2, 3);

List<Vector2Int> path = AstarHelper.GetPath(map, start, end, AstarHelper.AstarPathType.Manhattan);
*/

using System;
using System.Collections.Generic;
using UnityEngine;

public static class AstarHelper
{
    public enum AstarPathType
    {
        Manhattan,
        Diagonal,
        DiagonalFree,
        Euclidean,
        EuclideanFree,
    }

    private class _Object
    {
        public int X
        {
            get;
            set;
        }
        public int Y
        {
            get;
            set;
        }
        public double F
        {
            get;
            set;
        }
        public double G
        {
            get;
            set;
        }
        public int V
        {
            get;
            set;
        }
        public _Object P
        {
            get;
            set;
        }
        public _Object(int _x, int _y)
        {
            X = _x;
            Y = _y;
        }
    }

    public static List<Vector2Int> GetPath(int[,] grid, Vector2Int _start, Vector2Int _end, AstarPathType _astarPathType)
    {
        int cols = grid.GetLength(1);
        int rows = grid.GetLength(0);
        int limit = cols * rows;
        int length = 1;

        List<_Object> open = new List<_Object>
        {
            new _Object(_start.x, _start.y)
        };

        open[0].F = 0;
        open[0].G = 0;
        open[0].V = _start.x + _start.y * cols;

        _Object current;

        List<int> list = new List<int>();

        double distanceS;
        double distanceE;

        int i;
        int j;

        double max;
        int min;

        _Object[] next;
        _Object adj = null;

        _Object end = new _Object(_end.x, _end.y)
        {
            V = _end.x + _end.y * cols
        };

        bool inList;

        List<Vector2Int> results = new List<Vector2Int>();

        do
        {
            max = limit;
            min = 0;

            for (i = 0; i < length; i++)
            {
                if (open[i].F < max)
                {
                    max = open[i].F;
                    min = i;
                }
            }

            current = open[min];
            open.RemoveAt(min);

            if (current.V != end.V)
            {
                --length;
                next = Successors(current.X, current.Y, grid, rows, cols, _astarPathType);

                for (i = 0, j = next.Length; i < j; ++i)
                {
                    if (next[i] == null)
                    {
                        continue;
                    }

                    (adj = next[i]).P = current;
                    adj.F = adj.G = 0;
                    adj.V = adj.X + adj.Y * cols;
                    inList = false;

                    foreach (int key in list)
                    {
                        if (adj.V == key)
                        {
                            inList = true;
                        }
                    }

                    if (!inList)
                    {
                        if (_astarPathType == AstarPathType.DiagonalFree || _astarPathType == AstarPathType.Diagonal)
                        {
                            distanceS = Diagonal(adj, current);
                            distanceE = Diagonal(adj, end);
                        }
                        else if (_astarPathType == AstarPathType.Euclidean || _astarPathType == AstarPathType.EuclideanFree)
                        {
                            distanceS = Euclidean(adj, current);
                            distanceE = Euclidean(adj, end);
                        }
                        else
                        {
                            distanceS = Manhattan(adj, current);
                            distanceE = Manhattan(adj, end);
                        }

                        adj.F = (adj.G = current.G + distanceS) + distanceE;
                        open.Add(adj);
                        list.Add(adj.V);
                        length++;
                    }
                }
            }
            else
            {
                i = length = 0;
                do
                {
                    results.Add(new Vector2Int(current.X, current.Y));
                }
                while ((current = current.P) != null);
                results.Reverse();
            }
        }
        while (length != 0);

        return results;
    }

    private static _Object[] DiagonalSuccessors(bool xN, bool xS, bool xE, bool xW, int N, int S, int E, int W, int[,] grid, int rows, int cols, _Object[] result, int i)
    {
        if (xN)
        {
            if (xE && grid[N, E] == 0)
            {
                result[i++] = new _Object(E, N);
            }
            if (xW && grid[N, W] == 0)
            {
                result[i++] = new _Object(W, N);
            }
        }
        if (xS)
        {
            if (xE && grid[S, E] == 0)
            {
                result[i++] = new _Object(E, S);
            }
            if (xW && grid[S, W] == 0)
            {
                result[i++] = new _Object(W, S);
            }
        }
        return result;
    }

    private static _Object[] DiagonalSuccessorsFree(bool xN, bool xS, bool xE, bool xW, int N, int S, int E, int W, int[,] grid, int rows, int cols, _Object[] result, int i)
    {
        xN = N > -1;
        xS = S < rows;
        xE = E < cols;
        xW = W > -1;

        if (xE)
        {
            if (xN && grid[N, E] == 0)
            {
                result[i++] = new _Object(E, N);
            }
            if (xS && grid[S, E] == 0)
            {
                result[i++] = new _Object(E, S);
            }
        }
        if (xW)
        {
            if (xN && grid[N, W] == 0)
            {
                result[i++] = new _Object(W, N);
            }
            if (xS && grid[S, W] == 0)
            {
                result[i++] = new _Object(W, S);
            }
        }
        return result;
    }

    private static _Object[] NothingToDo(bool xN, bool xS, bool xE, bool xW, int N, int S, int E, int W, int[, ] grid, int rows, int cols, _Object[] result, int i)
    {
        return result;
    }

    private static _Object[] Successors(int x, int y, int[,] grid, int rows, int cols, AstarPathType astarPathType)
    {
        int N = y - 1;
        int S = y + 1;
        int E = x + 1;
        int W = x - 1;

        bool xN = N > -1 && grid[N, x] == 0;
        bool xS = S < rows && grid[S, x] == 0;
        bool xE = E < cols && grid[y, E] == 0;
        bool xW = W > -1 && grid[y, W] == 0;

        int i = 0;

        _Object[] result = new _Object[8];

        if (xN)
        {
            result[i++] = new _Object(x, N);
        }
        if (xE)
        {
            result[i++] = new _Object(E, y);
        }
        if (xS)
        {
            result[i++] = new _Object(x, S);
        }
        if (xW)
        {
            result[i++] = new _Object(W, y);
        }

        _Object[] obj =
            (astarPathType == AstarPathType.Diagonal || astarPathType == AstarPathType.Euclidean) ? DiagonalSuccessors(xN, xS, xE, xW, N, S, E, W, grid, rows, cols, result, i) :
            (astarPathType == AstarPathType.DiagonalFree || astarPathType == AstarPathType.EuclideanFree) ? DiagonalSuccessorsFree(xN, xS, xE, xW, N, S, E, W, grid, rows, cols, result, i) :
                                                                                     NothingToDo(xN, xS, xE, xW, N, S, E, W, grid, rows, cols, result, i);

        return obj;
    }

    private static double Diagonal(_Object start, _Object end)
    {
        return Math.Max(Math.Abs(start.X - end.X), Math.Abs(start.Y - end.Y));
    }

    private static double Euclidean(_Object start, _Object end)
    {
        var x = start.X - end.X;
        var y = start.Y - end.Y;

        return Math.Sqrt(x * x + y * y);
    }

    private static double Manhattan(_Object start, _Object end)
    {
        return Math.Abs(start.X - end.X) + Math.Abs(start.Y - end.Y);
    }

}