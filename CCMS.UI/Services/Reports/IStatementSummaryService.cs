using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features.Reports;

namespace CCMS.UI.Features.Reports
{
    public interface IStatementSummaryService
    {
        StatementSummaryViewModel ViewModel { get; set; }
        void Populate();
    }
}
