using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CCMS.UI.Bootstrappers.IoC;
using CCMS.UI.Features;
using CCMS.UI.Features.CashAdvances;
using CCMS.UI.Infrastructure;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.CashAdvances
{
    public class CashAdvanceListViewModel : ViewModelBase
    {
        private readonly CashAdvanceListController _controller;

        public virtual CashAdvanceViewModel SelectedItem { get; set; }

        public virtual ObservableCollection<CashAdvanceViewModel> Items { get; set; }

        public virtual IReactiveCommand Create { get; set; }

        public virtual IReactiveCommand Delete { get; set; }

        public CashAdvanceListViewModel()
        {
            _controller = new CashAdvanceListController(this);
        }
    }
}
