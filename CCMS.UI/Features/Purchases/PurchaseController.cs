using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.Purchases
{
    public class PurchaseController : ControllerBase<PurchaseViewModel>
    {
        public PurchaseController(PurchaseViewModel viewModel)
            : base(viewModel)
        {
            this.Initialize();

            this.ViewModel.Save = new ReactiveCommand(this.ViewModel
                .WhenAny(x => x.IsValid, x => x.Value == true));
            this.ViewModel.Save.Subscribe(x => Save());
        }

        private void Save()
        {
            var message = string.Format("Do you want to save Purchase?");
            var result = this.MessageBox.ShowQuestion(message);
            if (result == MessageBoxResult.OK)
                this.ViewModel.ActionResult = true;
        }

        public virtual void Initialize()
        {
            this.ViewModel.Id = Guid.Empty;
            this.ViewModel.Date = DateTime.Today;
            this.ViewModel.Details = string.Empty;
            this.ViewModel.Amount = 0M;
        }
    }
}
