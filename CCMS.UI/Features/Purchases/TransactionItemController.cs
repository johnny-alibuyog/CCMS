using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.Purchases
{
    public class TransactionItemController : ControllerBase<TransactionItemViewModel>
    {
        public TransactionItemController(TransactionItemViewModel viewModel) : base(viewModel)
        {
            this.Initialize();

            this.ViewModel.Save = new ReactiveCommand(this.ViewModel
                .WhenAny(x => x.IsValid, x => x.Value == true));
            this.ViewModel.Save.Subscribe(x => Save());
        }

        private void Save()
        {
            var message = string.Format("Do you want to save Transaction?");
            var result = this.MessageBox.ShowQuestion(message);
            if (result == MessageBoxResult.OK)
                this.ViewModel.ActionResult = true;
        }

        public virtual void Initialize()
        {
            this.ViewModel.Date = DateTime.Now;
            this.ViewModel.Details = string.Empty;
            this.ViewModel.CurrencyId = Core.Entities.Currency.PHP.Id;
            this.ViewModel.Amount = 0M;
        }
    }
}
