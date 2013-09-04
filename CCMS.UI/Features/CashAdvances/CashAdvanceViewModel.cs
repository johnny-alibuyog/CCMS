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

namespace CCMS.UI.Features.CashAdvances
{
    public class CashAdvanceViewModel : ViewModelBase
    {
        public virtual Guid Id { get; set; }

        public virtual DateTime Date { get; set; }

        [Length(Max = 250)]
        public virtual string Details { get; set; }

        [NotNull]
        public virtual KeyValuePair<Guid, string> Staff { get; set; }

        [NotNull]
        public virtual KeyValuePair<Guid, string> TransactionClassification { get; set; }

        public virtual decimal ServiceFee { get; set; }

        [Min(Value = 1)]
        public virtual decimal Amount { get; set; }

        public virtual string AmountDisplay { get { return Amount.ToString("N2") + " " + App.Data.SelectedCreditCard.TransactionCurrencyId; } }

        public virtual ObservableCollection<KeyValuePair<Guid, string>> Staffs { get; set; }

        public virtual ObservableCollection<KeyValuePair<Guid, string>> TransactionClassifications { get; set; }

        public virtual ObservableCollection<KeyValuePair<string, string>> Currencies { get; set; }

        public virtual IReactiveCommand Save { get; set; } 

        public CashAdvanceViewModel()
        {
            this.Id = Guid.Empty;
            this.Date = DateTime.Today;
            this.Details = string.Empty;
            //this.Staff = null;
            this.ServiceFee = 0M;
            this.Amount = 0M;
        } 
    }
}
