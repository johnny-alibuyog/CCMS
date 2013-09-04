using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features.CreditCardProviders;

namespace CCMS.UI.Features.CreditCardProviders
{
    public class ProviderListDialogService : DialogService<ProviderListView, ProviderListViewModel>
    {
        public ProviderListDialogService(ProviderListView view, ProviderListViewModel viewModel)
            : base(view, viewModel) { }
    }
}
