using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VersionOfficielle.Helpers
{
    static public class CPokerGame
    {
        public enum GGLimits
        {
            BBNL25 = 0,
            BBNL100 = 1
        }

        static private double BBNL25 = 0.25;
        static private double BBNL100 = 1.00;

        public static double getLimit(this GGLimits value)
        {                        
            if (value == GGLimits.BBNL25)
                return BBNL25;
            else
                return BBNL100;
        }

        static private byte ConvertToBBNL25(double _stack)
        {            
            return (byte)Math.Round(_stack / BBNL25, MidpointRounding.ToEven);
        }

        static private byte convertToBBNL100(double _stack)
        {
            return (byte)Math.Round(_stack / BBNL100, MidpointRounding.ToEven);
        }

        static public byte ConvertToBB(GGLimits _limit, double _stack)
        {
            switch (_limit)
            {
                case GGLimits.BBNL25:
                    return ConvertToBBNL25(_stack);
                case GGLimits.BBNL100:
                    return convertToBBNL100(_stack);
                default:
                    throw new Exception("Bad Limit Specified");
            }
        }
    }
}
