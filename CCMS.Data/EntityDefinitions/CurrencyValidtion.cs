using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace CCMS.Data.EntityDefinitions
{
    public class CurrencyValidtion : ValidationDef<Currency>
    {
        public CurrencyValidtion()
        {
            Define(x => x.Id)
                .NotNullableAndNotEmpty()
                .And.MaxLength(5);

            Define(x => x.Name)
                .NotNullableAndNotEmpty()
                .And.MaxLength(30);
        }
    }
}
