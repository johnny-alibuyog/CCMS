using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features.CreditCards;

namespace CCMS.UI.Features.CreditCards
{
    public class CreditCardListDialogService : DialogService<CreditCardListView, CreditCardListViewModel>
    {
        public CreditCardListDialogService(CreditCardListView view, CreditCardListViewModel viewModel)
            : base(view, viewModel) { }
    }
}
