using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Domain.Models;
using NHibernate.Validator.Cfg.Loquacious;

namespace CCMS.Data.Validations
{
    public class PaymentStatusValidation : ValidationDef<PaymentStatus>
    {
        public PaymentStatusValidation()
        {
            Define(x => x.Id)
                .NotNullableAndNotEmpty()
                .And.MaxLength(2);

            Define(x => x.Name)
                .NotNullableAndNotEmpty()
                .And.MaxLength(150);
        }
    }
}
