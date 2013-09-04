using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features.CreditCards;

namespace CCMS.UI.Features.CreditCards
{
    public interface ICreditCardListService
    {
        CreditCardListViewModel ViewModel { get; set; }
        void Insert(CreditCardViewModel item);
        void Update(CreditCardViewModel item);
        void Delete(CreditCardViewModel item);
        void PopulateList();
    }
}
