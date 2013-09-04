using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;

namespace CCMS.Core.Common.Extentions
{
    public static class MoneyExtention
    {
        public static decimal GetValue(this Money money)
        {
            return money != null ? money.Value : 0M;
        }
    }
}
