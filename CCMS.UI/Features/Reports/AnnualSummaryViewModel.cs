using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.Reports
{
    public class AnnualSummaryViewModel : ViewModelBase
    {
        private AnnualSummaryController _controller;

        public virtual int Year { get; set; }

        public virtual KeyValuePair<Guid, string> SelectedCreditCard { get; set; }

        public virtual IEnumerable<KeyValuePair<Guid, string>> CreditCards { get; set; }

        public virtual IEnumerable<AnnualSummaryItemViewModel> Items { get; set; }

        public virtual IReactiveCommand Generate { get; set; }

        public virtual Action Render { get; set; }

        public AnnualSummaryViewModel()
        {
            _controller = new AnnualSummaryController(this);
        }
    }
}
