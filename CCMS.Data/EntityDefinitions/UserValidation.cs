using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace CCMS.Data.EntityDefinitions
{
    public class UserValidation : ValidationDef<User>
    {
        public UserValidation()
        {
            Define(x => x.Id);

            Define(x => x.Version);

            Define(x => x.Audit);

            Define(x => x.Username)
                .NotNullableAndNotEmpty()
                .And.MaxLength(25);

            Define(x => x.Password)
                .NotNullableAndNotEmpty()
                .And.MaxLength(25);

            Define(x => x.Person)
                .NotNullable()
                .And.IsValid();

            Define(x => x.CreditCards)
                .HasValidElements();
        }
    }
}
