using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features.CreditCardProviders;

namespace CCMS.UI.Features.CreditCardProviders
{
    public class ComputationSettingDialogService : DialogService<ComputationSettingView, ComputationSettingViewModel>
    {
        public ComputationSettingDialogService(ComputationSettingView view, ComputationSettingViewModel viewModel)
            : base(view, viewModel) { }
    }
}
