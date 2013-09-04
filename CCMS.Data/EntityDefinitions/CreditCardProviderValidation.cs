using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace CCMS.Data.EntityDefinitions
{
    public class CreditCardProviderValidation : ValidationDef<CreditCardProvider>
    {
        public CreditCardProviderValidation()
        {
            Define(x => x.Id)
                .NotNullableAndNotEmpty()
                .And.MaxLength(5);

            Define(x => x.Version);

            Define(x => x.Audit);

            Define(x => x.Name)
                .NotNullableAndNotEmpty()
                .And.MaxLength(150);

            Define(x => x.InterestRate);

            Define(x => x.ComputationSettings)
                .HasValidElements();

            //Define(x => x.FinanceChargeSetting);

            //Define(x => x.InterestSetting);

            //Define(x => x.LateChargeSetting);

            //Define(x => x.OverlimitFeeSetting);

            //Define(x => x.ServiceFeeSetting);
        }
    }
}
