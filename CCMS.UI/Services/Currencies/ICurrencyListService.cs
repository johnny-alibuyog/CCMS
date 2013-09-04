using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features.Currencies;

namespace CCMS.UI.Features.Currencies
{
    public interface ICurrencyListService
    {
        CurrencyListViewModel ViewModel { get; set; }
        void Insert(CurrencyViewModel item);
        void Update(CurrencyViewModel item);
        void Delete(CurrencyViewModel item);
        void PopulateList();
    }
}
