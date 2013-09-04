using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CCMS.UI.Bootstrappers.IoC;
using CCMS.UI.Infrastructure;
using CCMS.UI.Features;
using CCMS.UI.Features.Billings;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.Billings
{
    public class BillingViewModel : ViewModelBase
    {
        private readonly BillingController _controller;

        public virtual Guid Id { get; set; }

        public virtual DateTime StartDate { get; set; }

        public virtual DateTime EndDate { get; set; }

        public virtual DateTime StatementDate { get; set; }

        public virtual DateTime DueDate { get; set; }

        public virtual decimal BillingAmount { get; set; }

        public virtual decimal PaymentAmount { get; set; }

        public virtual decimal SettlementBalance { get; set; }

        public virtual bool Editable { get; set; }

        public virtual int Count { get; set; }

        public virtual ChangeTrackingObservableCollection<BillingItemViewModel> Items { get; set; }

        public virtual string DateCoveredDisplay
        {
            get
            {
                return string.Format("{0} : {1}",
                    StartDate.ToString("yyyy-MM-dd"),
                    EndDate.ToString("yyyy-MM-dd")
                );
            }
        }

        public virtual string BillingAmountDisplay
        {
            get
            {
                return string.Format("{0} {1}",
                    BillingAmount.ToString("N2"),
                    App.Data.SelectedCreditCard.TransactionCurrencyId
                );
            }
        }

        public virtual string PaymentAmountDisplay
        {
            get
            {
                return string.Format("{0} {1}",
                    PaymentAmount.ToString("N2"),
                    App.Data.SelectedCreditCard.TransactionCurrencyId
                );
            }
        }

        public virtual string StatementBalanceDisplay
        {
            get
            {
                return string.Format("{0} {1}",
                    SettlementBalance.ToString("N2"),
                    App.Data.SelectedCreditCard.TransactionCurrencyId
                );
            }
        }

        public virtual BillingItemViewModel SelectedItem { get; set; }

        //public virtual IReactiveCommand Populate { get; set; }

        public virtual IReactiveCommand CreateAdjustment { get; set; }

        public virtual IReactiveCommand CreateInterest { get; set; }

        public virtual IReactiveCommand CreateCharge { get; set; }

        public virtual IReactiveCommand CreateFee { get; set; }

        public virtual IReactiveCommand Delete { get; set; }

        public virtual IReactiveCommand Save { get; set; }

        public BillingViewModel()
        {
            _controller = new BillingController(this);
        }

        public virtual void Populate(Guid billingId)
        {
            _controller.Populate(billingId);
            this.ActivateChangeTracking();
        }

        public virtual void HydrateWith(BillingViewModel value)
        {
            this.Id = value.Id;
            this.StartDate = value.StartDate;
            this.EndDate = value.EndDate;
            this.BillingAmount = value.BillingAmount;
            this.PaymentAmount = value.PaymentAmount;
            this.SettlementBalance = value.SettlementBalance;
            this.Count = value.Count;
        }
    }
}
