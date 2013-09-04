using System;
using CCMS.UI.Features.Billings;

namespace CCMS.UI.Services.Billings
{
    public interface IBillingListService
    {
        BillingListViewModel ViewModel { get; set; }
        void PopulateList();
        void Delete(BillingViewModel item);
    }
}
