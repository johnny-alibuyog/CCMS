using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features.CreditCardProviders;

namespace CCMS.UI.Features.CreditCardProviders
{
    public interface IProviderService
    {
        ProviderViewModel ViewModel { get; set; }
        void Populate(string providerId);
        void Save();
    }
}
