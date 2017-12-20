using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Utils
{

    public static class NumericHelperMethods
    {

        public static bool IsInRange(this double value, double low, double up)
        {
            if (low < up)
            {
                return (value >= low && value <= up);
            }
            else
            {
                return (value >= up && value <= low);
            }
        }

        public static bool IsInRange(this int value, int low, int up)
        {
            if (low < up)
            {
                return (value >= low && value <= up);
            }
            else
            {
                return (value >= up && value <= low);
            }
        }

        public static bool IsEqual(this double value, double compared)
        {
            const double MAX_DIFFERENCE = 0.00001d;
            double absDifference = Math.Abs((double)value - (double)compared);
            return (absDifference <= MAX_DIFFERENCE);
        }

        public static bool IsEqual(this double? value, double? compared)
        {
            if (value == null)
            {
                return (compared == null);
            }
            else
            {
                if (compared != null)
                {
                    return ((double)value).IsEqual((double)compared);
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool IsEqual(this int? value, int? compared)
        {
            if (value == null)
            {
                return (compared == null);
            }
            else
            {
                if (compared != null)
                {
                    return (value == compared);
                }
                else
                {
                    return false;
                }
            }
        }

    }

}
