using System;
using System.Collections.Generic;
using System.Text;

namespace nmm_game
{
    class Cell
    {
        public int i, j;

        public Cell(int x, int y)
        {
            i = x;
            j = y;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
                return false;

            Cell m = (Cell)obj;
            return (this.i == m.i) && (this.j == m.j);
        }

        public Cell(int index)
        {
            int k = 1;
            bool line4_2part = false;
            while (index > 3)
            {
                if (k == 4)
                    if (index > 6) index -= 6;
                    else
                    {
                        k--;
                        index -= 3;
                        line4_2part = true;
                    }
                else
                    index -= 3;
                k++;
            }
            i = k;
            if (k == 3 || k == 5) j = index + 2;
            else
                if (k == 2 || k == 6) j = index * 2;
                else
                    if (k == 1 || k == 7) j = index + (index - 1) * 2;
                    else
                        // k == 4
                        if (line4_2part) j = 4 + index;
                        else
                            j = index;
        }

        public static bool consecutive(Cell x, Cell y, Cell z)
        {
            // check if three stones are on the same row or on same column
            if (((x.i == y.i) && (y.i == z.i)) || ((x.j == y.j) && (y.j == z.j)))
                return true;
            else return false;
        }

        public static bool neighbour(int[,] a, Cell previousCell, Cell currentCell)
        {
            // check if two stones are neighbours
            if ((Math.Abs(previousCell.i - currentCell.i) == 0) &&
             !(previousCell.j == 3 && currentCell.j == 5) && !(previousCell.j == 5 && currentCell.j == 3))
                if (previousCell.j < currentCell.j)
                {
                    for (int jj = previousCell.j + 1; jj < currentCell.j; jj++)
                        if (a[previousCell.i, jj] != 8)
                            return false;
                    return true;
                }
                else
                {
                    for (int jj = currentCell.j + 1; jj < previousCell.j; jj++)
                        if (a[previousCell.i, jj] != 8)
                            return false;
                    return true;
                }
            if ((Math.Abs(previousCell.j - currentCell.j) == 0) &&
                !(previousCell.i == 3 && currentCell.i == 5) && !(previousCell.i == 5 && currentCell.i == 3))
                if (previousCell.i < currentCell.i)
                {
                    for (int ii = previousCell.i + 1; ii < currentCell.i; ii++)
                        if (a[ii, previousCell.j] != 8)
                            return false;
                    return true;
                }
                else
                {
                    for (int ii = currentCell.i + 1; ii < previousCell.i; ii++)
                        if (a[ii, previousCell.j] != 8)
                            return false;
                    return true;
                }
            return false;

        }

       public static int count(int[,] a, int player)
       {
        // count how many stones the given player has on the board
           int ct = 0;
           for (int k = 1; k < 25; k++)
           {
               Cell c = new Cell(k);
               if (a[c.i, c.j] == player)
                   ct++;
           }
           return ct;
       }
        

        public static Cell freeCell_inside_incompleteMill(int[,] a, int ii, int jj, int k, Cell x, Cell y, Cell z, int player)
        {
            // check if two stones are from the same player and the other one is free
            if ((a[x.i, x.j] == a[y.i, y.j]) && (a[x.i, x.j] == player) && (a[z.i, z.j] == 0))
            {
                return z;
            }
            if ((a[x.i, x.j] == a[z.i, z.j]) && (a[x.i, x.j] == player) && (a[y.i, y.j] == 0))
            {
                return y;
            }
            if ((a[z.i, z.j] == a[y.i, y.j]) && (a[z.i, z.j] == player) && (a[x.i, x.j] == 0))
            {
                return x;
            }
            else
                return null;
        }
    }
}
