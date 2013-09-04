using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features.CreditCards;

namespace CCMS.UI.Features.CreditCards
{
    public class CreditCardDialogService : DialogService<CreditCardView, CreditCardViewModel>
    {
        public CreditCardDialogService(CreditCardView view, CreditCardViewModel viewModel)
            : base(view, viewModel) { }
    }
}
