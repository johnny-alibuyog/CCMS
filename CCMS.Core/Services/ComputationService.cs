using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;

namespace CCMS.Core.Services
{
    public class ComputationService : IComputationService
    {
        private readonly CreditCard _creditCard;

        public ComputationService(CreditCard creditCard)
        {
            _creditCard = creditCard;
        }

        public virtual Money Compute<T>(decimal amount) where T : ComputationSetting
        {
            return _creditCard.Compute<T>(new Money(_creditCard.TransactionCurrency, amount));
        }

        public virtual Money Compute<T>(Money amount) where T : ComputationSetting
        {
            return _creditCard.Compute<T>(amount);
        }
    }
}
