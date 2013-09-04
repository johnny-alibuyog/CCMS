using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features.Billings;

namespace CCMS.UI.Services.Billings
{
    public interface IBillingService
    {
        BillingViewModel ViewModel { get; set; }
        void Populate(Guid billingId);
        //void InsertItem(BillingItemViewModel item);
        //void DeleteItem(BillingItemViewModel item);
        void Save();
    }
}
