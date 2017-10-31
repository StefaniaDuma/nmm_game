﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace nmm_game
{
    class Mill
    {
        public int start, middle, stop;

        public Mill(int a, int b, int c)
        {
            start = a;
            middle = b;
            stop = c;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
                return false;

            Mill m = (Mill)obj;
            return (this.start == m.start) && (this.middle == m.middle) && (this.stop == m.stop);
        }

        public static List<Mill> getMills(int[,] a, int player)
        {
            List<Mill> mills = new List<Mill>();
            for (int ii = 1; ii < 25; ii++)
            {
                for (int jj = ii + 1; jj < 25; jj++)
                {
                    for (int k = jj + 1; k < 25; k++)
                    {
                        var numbers = new int[] { ii, jj, k };
                        Array.Sort(numbers);
                        Cell x = new Cell(numbers[0]);
                        Cell y = new Cell(numbers[1]);
                        Cell z = new Cell(numbers[2]);
                        // check if the stones are different, neighbours and on the same row or column
                        if ((k != ii) && (k != jj) && (Cell.neighbour(a, x, y)) && (Cell.neighbour(a, y, z))
                            && (a[x.i, x.j] == a[y.i, y.j])
                            && (a[x.i, x.j] == a[z.i, z.j]) && (a[x.i, x.j] == player) && Cell.consecutive(x, y, z))
                        {
                            Mill m = new Mill(numbers[0], numbers[1], numbers[2]);
                            if (!(mills.Contains(m)))
                                mills.Add(m);
                        }
                    }
                }
            }

            return mills;
        
        }

        public static List<Cell> checkDangerMill(int[,] a, int player)
        {
            List<Cell> dangerousMills = new List<Cell>();
            for (int ii = 1; ii < 25; ii++)
            {
                for (int jj = ii+1; jj < 25; jj++)
                {
                    for (int k = jj+1; k < 25; k++)
                    {
                        var numbers = new int[] { ii, jj, k };
                            Array.Sort(numbers);
                            Cell x = new Cell(numbers[0]);
                            Cell y = new Cell(numbers[1]);
                            Cell z = new Cell(numbers[2]);
                            // check if the stones are different, neighbours and on the same row or column
                            if ((k != ii) && (k!=jj) && (Cell.neighbour(a, x, y)) && (Cell.neighbour(a, y, z))
                            && Cell.consecutive(x, y, z))
                        {
                            Cell danger = Cell.freeCell_inside_incompleteMill(a, ii, jj, k, x, y, z, player);
                            if (danger != null)
                            {
                                dangerousMills.Add(danger);
                            }
                        }
                    }
                }
            }
            return dangerousMills;

        }

        public static Cell goMill(int[,] a)
        {
            // move a blue stone in the vecinity of another blue stone on an empty position
            for (int ii = 1; ii < 8; ii++)
            {
                for (int jj = 1; jj < 8; jj++)
                {
                    if (a[ii, jj] == 2)
                    {
                        for (int k = 1; k < 25; k++)
                        {
                            Cell x = new Cell(k);
                            Cell y = new Cell(ii, jj);
                            if ((a[x.i, x.j] == 0) && Cell.neighbour(a, x, y))
                            {
                                return x;
                            }
                        }
                    }
                }
            }
            return null;

        }

        public static bool StonesOnlyInMills(int[,]a, int player)
        {
            // check if all the player stones from the board are inside mills
            List<Mill> Mills = Mill.getMills(a, player);
            HashSet<int> StonesInMills = new HashSet<int>();
            foreach (Mill m in Mills)
            {
                    StonesInMills.Add(m.start);
                    StonesInMills.Add(m.middle);
                    StonesInMills.Add(m.stop);
            }
            // if the number of stones from player inside mills is equal to the total number
            // of stones from player from the board then all the stones from the player are inside mills
            if (Cell.count(a, player) == StonesInMills.Count)
                return true;
            else
                return false;       
        }

        public static Cell getBlueStoneThirdPhase(int[,] a)
        {
            // find the three blue stones from the board
            Cell x = new Cell(0), y= new Cell(0), z=new Cell(0);
            int ct = 0;
            for (int k = 1; k < 25; k++)
            {
                Cell c = new Cell(k);
                if ((a[c.i, c.j] == 2) && (ct == 0))
                {
                    ct++;
                    x = c;
                }
                else
                    if ((a[c.i, c.j] == 2) && (ct == 1))
                    {
                        ct++;
                        y = c;
                    }
                    else
                    {
                        z = c;
                    }
            }
            if (Cell.neighbour(a, x, y))
            {
                return z;
            }
            if (Cell.neighbour(a, x, z))
            {
                return y;
            }
            if (Cell.neighbour(a, y, z))
            {
                return x;
            }
            return x;
        }

       

        public static bool playerCanMove(int[,] a, int player)
        {
            // check if player has no options to move in second phase of the game (blocked by other stones)
            bool atLeastOneFree = false;
            for (int k = 1; k < 25; k++)
            {
                Cell c = new Cell(k);
                if (a[c.i, c.j] == player)
                {
                    // go through all neighbours and see if at least one position is free
                    for (int p = 1; p < 25; p++)
                    {
                        Cell n = new Cell(p);
                        if (Cell.neighbour(a, n, c) && (k != p) && (a[n.i, n.j] == 0))
                            atLeastOneFree = true;
                    }
                }
            }
                return atLeastOneFree;
        }

        public static Cell removeRedStone(int[,] a)
        {
            // returns what red stone should be removed in case blue formed a mill
            // check if red could create soon a mill (two red stones in vecinity)
            // else remove random red stone
            List<Cell> redDangerousMills = Mill.checkDangerMill(a, 1);
            List<Mill> redMills = Mill.getMills(a, 1);
            HashSet<int> redStonesInMills = new HashSet<int>();
            int stone = 0;
            foreach (Mill m in redMills)
            {
                    redStonesInMills.Add(m.start);
                    redStonesInMills.Add(m.middle);
                    redStonesInMills.Add(m.stop);
                    stone = m.start;
            }
            foreach (int s in redStonesInMills)
                Console.Out.WriteLine(s+" ");
            if (redDangerousMills.Count != 0)
            {
                // remove red stone that is inside an incomplete mill
                foreach (Cell incomplete in redDangerousMills)
                {
                    // find its neighbours that could form a mill
                    for (int i = 1; i < 25; i++)
                        for (int j = i+1; j < 25; j++)
                        {                          
                            int k = Helper.toIJ(incomplete.i,incomplete.j);
                            var numbers = new int[] { i, j, k };
                            Array.Sort(numbers);
                            Cell x = new Cell(numbers[0]);
                            Cell y = new Cell(numbers[1]);
                            Cell z = new Cell(numbers[2]);
                            // check if the stones are different, neighbours and on the same row or column
                            if ((k != i) && (k!=j) && (Cell.neighbour(a, x, y)) && (Cell.neighbour(a, y, z))
                            && Cell.consecutive(x, y, z))
                            {
                                if (a[x.i, x.j] == 1 && (!redStonesInMills.Contains(numbers[0])))
                                {
                                    return x;                                   
                                }
                                else
                                    if (a[y.i, y.j] == 1 && (!redStonesInMills.Contains(numbers[1])))
                                    {
                                        return y;
                                    }
                                    else
                                        if (a[z.i, z.j] == 1 && (!redStonesInMills.Contains(numbers[2])))
                                        {
                                            return z;
                                        }
                            }                      
                        }
                }
                // remove red stone that is in the vecinity of an incomplete mill, regardless of row or column
                foreach (Cell incomplete in redDangerousMills)
                {
                    for (int k = 1; k < 25; k++)
                    {
                        Cell c = new Cell(k);
                        if (Cell.neighbour(a, incomplete, c) && (a[c.i, c.j] == 1)
                            && (!redStonesInMills.Contains(k)))
                        {
                            return c;
                        }
                    }
                }
               
            }
            for (int index = 1; index < 25; index++)
            {
                Cell w = new Cell(index);
                if ((a[w.i, w.j] == 1) && (!redStonesInMills.Contains(index)))
                {
                    return w;
                }
               
            }
            return new Cell(stone);
        }
    }


}
