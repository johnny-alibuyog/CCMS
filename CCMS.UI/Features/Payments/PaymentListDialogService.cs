using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features.Payments;

namespace CCMS.UI.Features.Payments
{
    public class PaymentListDialogService : DialogService<PaymentListView, PaymentListViewModel>
    {
        public PaymentListDialogService(PaymentListView view, PaymentListViewModel viewModel) 
            : base(view, viewModel) { }
    }
}
