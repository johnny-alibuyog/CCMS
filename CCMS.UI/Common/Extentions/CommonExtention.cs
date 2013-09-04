using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.UI.Common.Extentions
{
    public static class CommonExtention
    {
        public static KeyValuePair<string, string> Get(this IEnumerable<KeyValuePair<string, string>> list, string key)
        {
            return list.SingleOrDefault(x => x.Key == key);
        }
    }
}
