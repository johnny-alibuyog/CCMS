using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using CCMS.UI.Features.Billings;

namespace CCMS.UI.Features.Billings
{
    public class BillingListDialogService : DialogService<BillingListView, BillingListViewModel>
    {
        public BillingListDialogService(BillingListView view, BillingListViewModel viewModel)
            : base(view, viewModel) { }
    }
}
