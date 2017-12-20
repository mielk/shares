using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Utils
{

    public static class StringHelperMethods
    {

        public static bool Compare(this string value, string compared)
        {
            if (value == null)
            {
                return (compared == null);
            }
            else
            {
                return value.Equals(compared);
            }
        }

    }

}