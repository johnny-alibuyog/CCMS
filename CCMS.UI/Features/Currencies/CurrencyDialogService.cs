using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features.Currencies;

namespace CCMS.UI.Features.Currencies
{
    public class CurrencyDialogService : DialogService<CurrencyView, CurrencyViewModel>
    {
        public CurrencyDialogService(CurrencyView view, CurrencyViewModel viewModel)
            : base(view, viewModel) { }
    }
}
