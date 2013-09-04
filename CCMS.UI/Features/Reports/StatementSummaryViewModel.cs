using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using CCMS.UI.Features.Reports;

namespace CCMS.UI.Features.Reports
{
    public class StatementSummaryViewModel : ViewModelBase
    {
        private readonly StatementSummaryController _controller;

        public virtual ObservableCollection<StatementSummaryItemViewModel> Items { get; set; }

        public StatementSummaryViewModel()
        {
            _controller = new StatementSummaryController(this);
        }
    }
}
