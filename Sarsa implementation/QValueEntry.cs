using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Sarsa_implementation
{
    internal class QValueEntry
    {
        public int X;

        public int Y;
        public Action Action { get; set; }
        public double QValue { get; set; }

        public QValueEntry(int X, int Y, Action action, double qValue)
        {

            this.X = X;
            this.Y = Y;
            Action = action;
            QValue = qValue;
        }
    }
}
