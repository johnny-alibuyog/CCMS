using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features.Billings;

namespace CCMS.UI.Features.Billings
{
    public class BillingItemDialogService : DialogService<BillingItemView, BillingItemViewModel>
    {
        public BillingItemDialogService(BillingItemView view, BillingItemViewModel viewModel)
            : base(view, viewModel) { }
    }
}
