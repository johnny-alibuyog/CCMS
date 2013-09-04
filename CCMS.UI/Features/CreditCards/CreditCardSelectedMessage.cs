using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features.CreditCards;
using CCMS.UI.Features.Navigations;

namespace CCMS.UI.Features.CreditCards
{
    public class CreditCardSelectedMessage
    {
        public virtual CreditCardInfoViewModel CreditCard 
        {
            get { return App.Data.SelectedCreditCard; }
            set { App.Data.SelectedCreditCard = value; } 
        }

        public CreditCardSelectedMessage(CreditCardInfoViewModel creditCard)
        {
            CreditCard = creditCard;
        }
    }
}
