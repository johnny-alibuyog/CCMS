using System;
using CCMS.UI.Features.Purchases;

namespace CCMS.UI.Features.Purchases
{
    public interface IPurchaseListService
    {
        PurchaseListViewModel ViewModel { get; set; }
        void Insert(PurchaseViewModel item);
        void Delete(PurchaseViewModel item);
        void PopulateList();
    }
}
