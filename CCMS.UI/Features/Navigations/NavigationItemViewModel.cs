using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CCMS.UI.Features.CreditCards;
using CCMS.UI.Infrastructure;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.Navigations
{
    public class NavigationItemViewModel : ViewModelBase
    {
        private readonly NavigationItemController _controller;

        public virtual CreditCardInfoViewModel CreditCard { get; set; }

        public IReactiveCommand SelectItem { get; set; }

        public NavigationItemViewModel()
        {
            _controller = new NavigationItemController(this);
        }
    }
}
