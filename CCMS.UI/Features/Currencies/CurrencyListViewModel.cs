using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Windows.Input;
using CCMS.UI.Bootstrappers.IoC;
using CCMS.UI.Features;
using CCMS.UI.Features.Currencies;
using CCMS.UI.Infrastructure;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.Currencies
{
    public class CurrencyListViewModel : ViewModelBase
    {
         private readonly CurrencyListController _controller;

        public virtual CurrencyViewModel SelectedItem { get; set; }

        public virtual ObservableCollection<CurrencyViewModel> Items { get; set; }

        public virtual ObservableCollection<KeyValuePair<string, string>> Providers { get; set; }

        public virtual ObservableCollection<KeyValuePair<string, string>> Currencies { get; set; }

        public virtual IReactiveCommand Create { get; set; }

        public virtual IReactiveCommand Edit { get; set; }

        public virtual IReactiveCommand Delete { get; set; }

        public CurrencyListViewModel()
        {
            _controller = new CurrencyListController(this);
        }
   }
}
