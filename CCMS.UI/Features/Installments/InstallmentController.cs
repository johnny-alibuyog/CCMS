using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.Installments
{
    public class InstallmentController : ControllerBase<InstallmentViewModel>
    {
        public InstallmentController(InstallmentViewModel viewModel) : base(viewModel)
        {
            this.Initialize();

            this.ViewModel.WhenAny(
                x => x.Terms,
                x => x.InterestRate,
                x => x.Amount,
                (x, y, z) => true
            )
            .Subscribe(x => this.Compute());

            this.ViewModel.Save = new ReactiveCommand(this.ViewModel
                .WhenAny(x => x.IsValid, x => x.Value == true));
            this.ViewModel.Save.Subscribe(x => Save());
        }

        private void Save()
        {
            var message = string.Format("Do you want to save Installment?");
            var result = this.MessageBox.ShowQuestion(message);
            if (result == MessageBoxResult.OK)
                this.ViewModel.ActionResult = true;
        }

        private void Compute()
        {
            if (this.ViewModel.Terms == 0)
                return;

            this.ViewModel.Interest = this.ViewModel.Terms * this.ViewModel.Amount * this.ViewModel.InterestRate;
            this.ViewModel.Amortization = (this.ViewModel.Amount + this.ViewModel.Interest) / this.ViewModel.Terms;
            this.ViewModel.Balance = this.ViewModel.Amount + this.ViewModel.Interest;
        }

        private void Initialize()
        {
            this.ViewModel.Id = Guid.Empty;
            this.ViewModel.Date = DateTime.Today;
            this.ViewModel.Details = string.Empty;
            this.ViewModel.Terms = 6;
            this.ViewModel.InterestRate = App.Data.SelectedCreditCard.InterestRate;
            this.ViewModel.Amount = 0M;
            this.ViewModel.Amortization = 0M;
            this.ViewModel.Interest = 0M;
            this.ViewModel.Balance = 0M;
        }
    }
}
