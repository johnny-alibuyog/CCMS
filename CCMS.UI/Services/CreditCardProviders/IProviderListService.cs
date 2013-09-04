using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features.CreditCardProviders;

namespace CCMS.UI.Features.CreditCardProviders
{
    public interface IProviderListService
    {
        ProviderListViewModel ViewModel { get; set; }
        void Delete(ProviderViewModel item);
        void PopulateList();
    }
}
