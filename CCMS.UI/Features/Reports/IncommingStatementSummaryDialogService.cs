using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features.Reports;

namespace CCMS.UI.Features.Reports
{
    public class IncommingStatementSummaryDialogService : DialogService<IncommingStatementSummaryView, IncommingStatementSummaryViewModel>
    {
        public IncommingStatementSummaryDialogService(IncommingStatementSummaryView view, IncommingStatementSummaryViewModel viewModel) 
            :base(view, viewModel) { }
    }
}
