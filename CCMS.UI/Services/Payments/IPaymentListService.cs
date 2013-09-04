using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features.Payments;

namespace CCMS.UI.Features.Payments
{
    public interface IPaymentListService
    {
        PaymentListViewModel ViewModel { get; set; }
        PaymentViewModel Create();
        void Insert(PaymentViewModel item);
        void Delete(PaymentViewModel item);
        void PopulateList();
    }
}
