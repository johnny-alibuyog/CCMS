using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.Billings
{
    public class BillingItemController : ControllerBase<BillingItemViewModel>
    {
        public BillingItemController(BillingItemViewModel viewModel)
            : base(viewModel)
        {
            this.ViewModel.Create = new ReactiveCommand(this.ViewModel
                .WhenAny(x => x.IsValid, x => x.Value == true));
            this.ViewModel.Create.Subscribe(x => Create());
        }

        private void Create()
        {
            var message = string.Format("Do you want to create {0}?", this.ViewModel.Type);
            var result = this.MessageBox.ShowQuestion(message);
            if (result == MessageBoxResult.OK)
                this.ViewModel.ActionResult = true;
        }
    }
}
