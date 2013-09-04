using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using FluentNHibernate.Mapping;

namespace CCMS.Data.EntityDefinitions
{
    public class CurrencyMapping : ClassMap<Currency>
    {
        public CurrencyMapping()
        {
            Id(x => x.Id);

            Map(x => x.Name);
        }
    }
}
