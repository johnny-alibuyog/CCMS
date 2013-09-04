using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features.Billings;

namespace CCMS.UI.Features.Billings
{
    public class BillingDialogService : DialogService<BillingView, BillingViewModel>
    {
        public BillingDialogService(BillingView view, BillingViewModel viewModel)
            : base(view, viewModel) { }
    }
}
