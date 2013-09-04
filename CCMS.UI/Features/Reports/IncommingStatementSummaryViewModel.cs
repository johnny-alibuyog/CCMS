using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using CCMS.UI.Features.Reports;

namespace CCMS.UI.Features.Reports
{
    public class IncommingStatementSummaryViewModel : ViewModelBase
    {
        private readonly IncommingStatementSummaryController _controller;

        public virtual ObservableCollection<IncommingStatementSummaryItemViewModel> Items { get; set; }

        public IncommingStatementSummaryViewModel()
        {
            _controller = new IncommingStatementSummaryController(this);
        }
    }
}
