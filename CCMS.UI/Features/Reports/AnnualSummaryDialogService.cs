using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.UI.Features.Reports
{
    public class AnnualSummaryDialogService : DialogService<AnnualSummaryView, AnnualSummaryViewModel>
    {
        public AnnualSummaryDialogService(AnnualSummaryView view, AnnualSummaryViewModel viewModel)
            : base(view, viewModel) { }
    }
}
