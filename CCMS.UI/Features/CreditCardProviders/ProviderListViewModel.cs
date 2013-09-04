using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CCMS.UI.Bootstrappers.IoC;
using CCMS.UI.Features;
using CCMS.UI.Features.CreditCardProviders;
using CCMS.UI.Infrastructure;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.CreditCardProviders
{
    public class ProviderListViewModel : ViewModelBase
    {
        private readonly ProviderListController _controller;

        public virtual ProviderViewModel SelectedItem { get; set; }

        public virtual ObservableCollection<ProviderViewModel> Items { get; set; }

        public virtual IReactiveCommand Create { get; set; }

        public virtual IReactiveCommand Edit { get; set; }

        public virtual IReactiveCommand Delete { get; set; }

        public ProviderListViewModel()
        {
            _controller = new ProviderListController(this);
        }
    }
}
