using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features.Payments;

namespace CCMS.UI.Features.Payments
{
    public class PaymentDialogService : DialogService<PaymentView, PaymentViewModel>
    {
        public PaymentDialogService(PaymentView view, PaymentViewModel viewModel)
            : base(view, viewModel) { }
    }
}
