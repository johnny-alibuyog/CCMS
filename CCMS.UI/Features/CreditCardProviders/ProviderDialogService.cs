using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features.CreditCardProviders;

namespace CCMS.UI.Features.CreditCardProviders
{
    public class ProviderDialogService : DialogService<ProviderView, ProviderViewModel>
    {
        public ProviderDialogService(ProviderView view, ProviderViewModel viewModel)
            : base(view, viewModel) { }
    }
}
