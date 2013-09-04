using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.Reports
{
    public class ExpensesByClassificationViewModel : ViewModelBase
    {
        private readonly ExpensesByClassificationController _controller;

        public virtual DateTime FromDate { get; set; }

        public virtual DateTime ToDate { get; set; }

        public virtual KeyValuePair<Guid, string> SelectedCreditCard { get; set; }

        public virtual IEnumerable<KeyValuePair<Guid, string>> CreditCards { get; set; }

        public virtual IEnumerable<ExpensesByClassificationItemViewModel> Items { get; set; }

        public virtual IReactiveCommand Generate { get; set; }

        public virtual Action Render { get; set; }

        public ExpensesByClassificationViewModel()
        {
            _controller = new ExpensesByClassificationController(this);
        }
    }
}
