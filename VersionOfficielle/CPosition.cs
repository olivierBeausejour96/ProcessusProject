using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VersionOfficielle
{
    public class CPosition
    {
        public int X;
        public int Y;
        public CPosition(int _X, int _Y)
        {
            X = _X;
            Y = _Y;
        }

        static public CPosition operator+(CPosition _leftPos, CPosition _rightPos)
        {
            return new CPosition(_leftPos.X + _rightPos.X, _leftPos.Y + _rightPos.Y);
        }

        public override string ToString()
        {
            return "X: " + X.ToString() + "\nY: " + Y.ToString();
        }
    }
}
