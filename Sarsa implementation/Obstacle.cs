using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sarsa_implementation
{
    internal class Obstacle
    {
        public string representation;
        public int x;
        public int y;
        public double value;
        public Obstacle(string representation, int x, int y, double value) 
        { 
            this.representation = representation;
            this.x = x;
            this.y = y;
            this.value = value;
        }

        public static Obstacle EmptySpace(int x, int y)
        {
            return (new Obstacle(".", x, y, 0.0));
        }

        public static Obstacle End(int x, int y)
        {
            return (new Obstacle("/", x, y, 1000.0));
        }
    }
}
