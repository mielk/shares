using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.Domain.Entities;
using Stock.Domain.Enums;
using Stock.DAL.TransferObjects;
using Stock.Core;
using Stock.Utils;

namespace Stock_UnitTest.Helpers
{

    public class UTDefaulter
    {

        public const int DEFAULT_ID = 1;
        public const int DEFAULT_ASSET_ID = 1;
        public const int DEFAULT_TIMEFRAME_ID = 1;
        public const int DEFAULT_SIMULATION_ID = 1;
        public const int DEFAULT_START_INDEX = 84;
        public const double DEFAULT_START_LEVEL = 1.1654;
        public const int DEFAULT_FOOTHOLD_INDEX = 134;
        public const double DEFAULT_FOOTHOLD_LEVEL = 1.1754;
        public const double DEFAULT_VALUE = 1.234;
        public const int DEFAULT_LAST_UPDATE_INDEX = 154;
        public const int DEFAULT_FOOTHOLD_SLAVE_INDEX = 135;
        public const int DEFAULT_FOOTHOLD_IS_PEAK = 1;
        public const int DEFAULT_FOOTHOLD_TYPE = 1;
        public const bool DEFAULT_INITIAL_IS_PEAK = true;
        public const bool DEFAULT_CURRENT_IS_PEAK = false;

    }

}
