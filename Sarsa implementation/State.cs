using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Sarsa_implementation
{
    internal class State:IEquatable<State>
    {
        public int X;
        public int Y;
        public State(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public override bool Equals(object? obj)
        {
            return this.Equals((State)obj);
        }

        public bool Equals(State other)
        { return (other.X == this.X && other.Y == this.Y); }
    }
}
