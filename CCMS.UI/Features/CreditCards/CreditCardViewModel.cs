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

namespace CCMS.UI.Features.CreditCards
{
    public class CreditCardViewModel : ViewModelBase
    {
        public virtual Guid Id { get; set; }

        [NotNullNotEmpty]
        public virtual string AccountNumber { get; set; }

        [NotNullNotEmpty]
        public virtual string AccountName { get; set; }

        [Range(Min = 1, Max = 31)]
        public virtual int CutOff { get; set; }

        [NotNull]
        public virtual Nullable<DateTime> IssueDate { get; set; }

        [NotNull]
        public virtual Nullable<DateTime> ExpiryDate { get; set; }

        [NotNull]
        public virtual KeyValuePair<string, string> Provider { get; set; }

        [NotNull]
        public virtual KeyValuePair<string, string> TransactionCurrency { get; set; }

        public virtual decimal CreditLimit { get; set; }

        public virtual decimal CashAdvanceLimit { get; set; }

        public virtual ObservableCollection<KeyValuePair<string, string>> Providers { get; set; }

        public virtual ObservableCollection<KeyValuePair<string, string>> Currencies { get; set; }

        public virtual IReactiveCommand Save { get; set; }

        public virtual void HydrateWith(CreditCardViewModel value)
        {
            this.Id = value.Id;
            this.AccountNumber = value.AccountNumber;
            this.AccountName = value.AccountName;
            this.CutOff = value.CutOff;
            this.IssueDate = value.IssueDate;
            this.ExpiryDate = value.ExpiryDate;
            this.Provider = value.Provider;
            this.TransactionCurrency = value.TransactionCurrency;
            this.CreditLimit = value.CreditLimit;
            this.CashAdvanceLimit = value.CashAdvanceLimit;
        }
    }
}
