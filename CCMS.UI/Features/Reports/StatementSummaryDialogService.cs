using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features.Reports;

namespace CCMS.UI.Features.Reports
{
    public class StatementSummaryDialogService : DialogService<StatementSummaryView, StatementSummaryViewModel>
    {
        public StatementSummaryDialogService(StatementSummaryView view, StatementSummaryViewModel viewModel) 
            : base(view, viewModel) { }
    }
}
