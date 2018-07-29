using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VersionOfficielle
{
    public class Position
    {
        public int X;
        public int Y;
        public Position(int _X, int _Y)
        {
            X = _X;
            Y = _Y;
        }

        static public Position operator+(Position _leftPos, Position _rightPos)
        {
            return new Position(_leftPos.X + _rightPos.X, _leftPos.Y + _rightPos.Y);
        }

        public override string ToString()
        {
            return "X: " + X.ToString() + "\nY: " + Y.ToString();
        }
    }
}
