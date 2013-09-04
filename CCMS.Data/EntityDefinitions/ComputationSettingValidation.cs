using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace CCMS.Data.EntityDefinitions
{
    public class ComputationSettingValidation : ValidationDef<ComputationSetting>
    {
        public ComputationSettingValidation()
        {
            Define(x => x.Id);

            Define(x => x.Provider)
                .NotNullable();

            Define(x => x.MinimumAmount);

            Define(x => x.Rate);
        }
    }
}
