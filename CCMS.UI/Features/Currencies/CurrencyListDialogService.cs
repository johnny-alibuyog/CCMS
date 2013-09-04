using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features.Currencies;

namespace CCMS.UI.Features.Currencies
{
    public class CurrencyListDialogService : DialogService<CurrencyListView, CurrencyListViewModel>
    {
        public CurrencyListDialogService(CurrencyListView view, CurrencyListViewModel viewModel)
            : base(view, viewModel) { }
    }
}
