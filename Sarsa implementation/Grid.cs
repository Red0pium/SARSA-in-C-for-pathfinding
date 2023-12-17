using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sarsa_implementation
{
    internal class Grid
    {
        public int nrow;
        public int ncol;
        public int xbegining;
        public int ybegining;
        public int xending;
        public int yending;
        public List<Obstacle> obstacles;
        public Obstacle[,] Content;

        public Grid(int nrow,int ncol, int xbegining, int ybegining, int xending, int yending, List<Obstacle> obstacles) 
        {
            this.nrow = nrow;
            this.ncol = ncol;
            this.xbegining = xbegining;
            this.ybegining = ybegining;
            this.xending = xending;
            this.yending = yending;
            this.obstacles = obstacles;
            this.Content = GridObstacles(obstacles, nrow, ncol, xending, yending);
        }

        private bool isObject(int x, int y)
        {
            foreach (Obstacle o in this.obstacles) 
            {
                if (o.x==x&& o.y==y) return true;
            }
            return false;
        }

        private Obstacle ObstacleOnPos(int x, int y)
        {
            foreach (Obstacle o in this.obstacles)
            {
                if (o.x == x && o.y == y) return o;
            }
            return null;
        }

        private static Obstacle ObstacleOnPos(List<Obstacle> obstacles, int x, int y)
        {
            foreach (Obstacle o in obstacles)
            {
                if (o.x == x && o.y == y) return o;
            }
            return null;
        }

        private Obstacle[,] GridObstacles(List<Obstacle> obstacles, int nrow, int ncol, int xending, int yending)
        {
            Obstacle[,] list = new Obstacle[nrow,ncol];
            for (int x1 = 0; x1 < nrow; x1++)
            {
                for (int y1 = 0; y1 < ncol; y1++)
                {
                    Obstacle o = ObstacleOnPos(obstacles, x1, y1);
                    if (o != null)
                    {
                        list[x1,y1] = o;
                    }
                    else if (x1 == xending && y1 == yending)
                    {
                        list[x1, y1] = o = Obstacle.End(x1, y1);
                    }
                    else
                    {
                        list[x1, y1] = Obstacle.EmptySpace(x1, y1);
                    }
                }
            }
            return list;
        }

        public bool IsTerminalState(State state)
        {
            if (state.X==this.xending && state.Y == this.yending) return true; 
            return false;
        }

        public void PrintGrid() 
        { 
            for (int x1 = 0; x1<this.nrow; x1++)
            {
                for (int y1 = 0; y1<this.ncol; y1++) 
                {
                    Console.Write(Content[x1,y1].representation);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void PrintGrid(State state)
        {
            for (int x1 = 0; x1 < this.nrow; x1++)
            {
                for (int y1 = 0; y1 < this.ncol; y1++)
                {
                    if (x1 != state.X || y1 != state.Y)
                    {
                        Console.Write(Content[x1, y1].representation);
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write("A");
                        Console.Write(" ");
                    }
                    
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
