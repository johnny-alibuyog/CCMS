using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features.CreditCards;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.Navigations
{
    public class NavigationItemController : ControllerBase<NavigationItemViewModel>
    {
        public NavigationItemController(NavigationItemViewModel viewModel) : base(viewModel)
        {
            this.ViewModel.SelectItem = new ReactiveCommand(App.Data
                .WhenAny(x => x.SelectedCreditCard, x => x.Value != this.ViewModel.CreditCard));
            this.ViewModel.SelectItem.Subscribe(x => App.Data.SelectedCreditCard = this.ViewModel.CreditCard);
        }
    }
}
