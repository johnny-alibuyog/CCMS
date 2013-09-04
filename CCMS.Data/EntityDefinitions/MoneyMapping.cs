using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using FluentNHibernate.Mapping;

namespace CCMS.Data.EntityDefinitions
{
    public class MoneyMapping : ComponentMap<Money>
    {
        // NOTE: ColumnPrefix does not apply to this.
        public MoneyMapping()
        {
            References(x => x.Currency);

            Map(x => x.Value);
        }

        // work around
        internal static Action<ComponentPart<Money>> Map(string columnPrefix = "")
        {
            return mapping =>
            {
                mapping.References(x => x.Currency, columnPrefix + "CurrencyId");

                mapping.Map(x => x.Value, columnPrefix + "Value");

                mapping.Access.CamelCaseField(Prefix.Underscore);
            };
        }
    }
}
