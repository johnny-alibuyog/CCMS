using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CCMS.UI.Infrastructure;
using NHibernate.Validator.Constraints;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.Installments
{
    public class InstallmentViewModel : ViewModelBase
    {
        public virtual Guid Id { get; set; }

        public virtual DateTime Date { get; set; }

        [Length(Max = 250)]
        public virtual string Details { get; set; }

        [NotNull]
        public virtual KeyValuePair<Guid, string> Staff { get; set; }

        [NotNull]
        public virtual KeyValuePair<Guid, string> TransactionClassification { get; set; }

        [Min(Value = 1)]
        public virtual int Terms { get; set; }

        public virtual decimal InterestRate { get; set; }

        public virtual decimal Amortization { get; set; }

        public virtual decimal Interest { get; set; }

        public virtual decimal Amount { get; set; }

        public virtual decimal Balance { get; set; }

        public virtual string AmortizationDisplay { get { return FormatValue(Amortization); } }

        public virtual string InterestDisplay { get { return FormatValue(Interest); } }

        public virtual string AmountDisplay { get { return FormatValue(Amount); } }

        public virtual string BalanceDisplay { get { return FormatValue(Balance); } }

        public virtual ObservableCollection<KeyValuePair<Guid, string>> Staffs { get; set; }

        public virtual ObservableCollection<KeyValuePair<Guid, string>> TransactionClassifications { get; set; }

        public virtual ObservableCollection<KeyValuePair<string, string>> Currencies { get; set; }

        public virtual IReactiveCommand Save { get; set; }

        private string FormatValue(decimal value)
        {
            return string.Format("{0} {1}",
                value.ToString("N2"),
                App.Data.SelectedCreditCard.TransactionCurrencyId
            );
        }

        public InstallmentViewModel()
        {
            this.InitializeView();

            this.WhenAny(
                x => x.Terms,
                x => x.InterestRate,
                x => x.Amount,
                (x, y, z) => true
            )
            .Subscribe(x => this.Compute());
        }

        private void Compute()
        {
            if (this.Terms == 0)
                return;

            this.Interest = this.Terms * this.Amount * this.InterestRate;
            this.Amortization = (this.Amount + this.Interest) / this.Terms;
            this.Balance = this.Amount + this.Interest;
        }

        private void InitializeView()
        {
            this.Id = Guid.Empty;
            this.Date = DateTime.Today;
            this.Details = string.Empty;
            this.Terms = 6;
            this.InterestRate = App.Data.SelectedCreditCard.InterestRate;
            this.Amount = 0M;
            this.Amortization = 0M;
            this.Interest = 0M;
            this.Balance = 0M;
        }
    }
}
