using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CCMS.UI.Infrastructure;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.Billings
{
    public class BillingItemViewModel : ViewModelBase
    {
        public virtual Guid Id { get; set; }

        public virtual DateTime Date { get; set; }

        public virtual string Type { get; set; }

        public virtual string Details { get; set; }

        public virtual decimal Amount { get; set; }

        public virtual string Title { get; set; }

        public virtual string AmountDisplay
        {
            get
            {
                return string.Format("{0} {1}",
                    Amount.ToString("N2"),
                    App.Data.SelectedCreditCard.TransactionCurrencyId
                );
            }
        }

        public virtual IReactiveCommand Create { get; set; }

        #region Constructors

        public BillingItemViewModel()
        {
            this.Id = Guid.Empty;
            this.Date = DateTime.Today;
            this.Type = string.Empty;
            this.Details = string.Empty;
            this.Amount = 0M;
            this.Title = string.Empty;
        }

        public BillingItemViewModel(BillingItemViewModel value)
            : this()
        {
            this.Id = value.Id;
            this.Date = value.Date;
            this.Type = value.Type;
            this.Details = value.Details;
            this.Amount = value.Amount;
        }

        public BillingItemViewModel(string title, string type)
            : this()
        {
            this.Title = title;
            this.Type = type;
        } 

        #endregion
    }
}
