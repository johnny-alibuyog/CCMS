using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CCMS.UI.Bootstrappers.IoC;
using CCMS.UI.Features;
using CCMS.UI.Features.Billings;
using CCMS.UI.Features.Purchases;
using CCMS.UI.Infrastructure;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.Purchases
{
    public class PurchaseListViewModel : ViewModelBase
    {
        private readonly PurchaseListController _controller;

        public virtual PurchaseViewModel SelectedItem { get; set; }

        public virtual ObservableCollection<PurchaseViewModel> Items { get; set; }

        public virtual IReactiveCommand Create { get; set; }

        public virtual IReactiveCommand Delete { get; set; }

        public PurchaseListViewModel()
        {
            _controller = new PurchaseListController(this);
        }
    }
}
