using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CCMS.UI.Bootstrappers.IoC;
using CCMS.UI.Features;
using CCMS.UI.Features.Payments;
using CCMS.UI.Infrastructure;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.Payments
{
    public class PaymentListViewModel : ViewModelBase
    {
        private readonly PaymentListController _controller;

        public virtual PaymentViewModel SelectedItem { get; set; }

        public virtual ObservableCollection<PaymentViewModel> Items { get; set; }

        public virtual IReactiveCommand Create { get; set; }

        public virtual IReactiveCommand Delete { get; set; }

        public PaymentListViewModel()
        {
            _controller = new PaymentListController(this);
        }
    }
}
