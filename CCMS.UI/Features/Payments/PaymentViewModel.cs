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

namespace CCMS.UI.Features.Payments
{
    public class PaymentViewModel : ViewModelBase
    {
        public virtual Guid Id { get; set; }

        public virtual DateTime PaymentDueDate { get; set; }

        public virtual decimal TotalAmountDue { get; set; }

        public virtual decimal TotalMinimumAmountDue { get; set; }

        public virtual DateTime Date { get; set; }

        [NotNull]
        public virtual KeyValuePair<Guid, string> Staff { get; set; }

        [NotNull]
        public virtual KeyValuePair<Guid, string> TransactionClassification { get; set; }

        [Min(Value = 1)]
        public virtual decimal Amount { get; set; }

        public virtual string DateDisplay
        {
            get { return Date.ToString("yyyy-MM-dd"); }
        }

        public virtual string AmountDisplay
        {
            get
            {
                return string.Format("{0} {1}", Amount.ToString("N2"),
                    App.Data.SelectedCreditCard.TransactionCurrencyId);
            }
        }

        public virtual ObservableCollection<KeyValuePair<Guid, string>> Staffs { get; set; }

        public virtual ObservableCollection<KeyValuePair<Guid, string>> TransactionClassifications { get; set; }

        public virtual ObservableCollection<KeyValuePair<string, string>> Currencies { get; set; }

        public virtual IReactiveCommand Save { get; set; }

        public PaymentViewModel()
        {
            this.Date = DateTime.Today;
        }
    }
}
