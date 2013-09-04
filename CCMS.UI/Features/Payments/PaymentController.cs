using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using NHibernate;
using NHibernate.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.Payments
{
    public class PaymentController : ControllerBase<PaymentViewModel>
    {
        public PaymentController(PaymentViewModel viewModel) : base(viewModel)
        {
            this.ViewModel.Save = new ReactiveCommand(this.ViewModel
                .WhenAny(x => x.IsValid, x => x.Value == true));
            this.ViewModel.Save.Subscribe(x => Save());

            //this.ViewModel.ComputeNewPayment = new ReactiveCommand();
            //this.ViewModel.ComputeNewPayment.Subscribe(x => ComputeNewPayment());
        }

        private void Save()
        {
            var message = string.Format("Do you want to save Payment?");
            var result = this.MessageBox.ShowQuestion(message);
            if (result == MessageBoxResult.OK)
                this.ViewModel.ActionResult = true;
        }

        public virtual void ComputeNewPayment()
        {
            try
            {
                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var billing = session.Query<Billing>()
                        .Where(x =>
                            //x.StartDate < DateTime.Today &&
                            x.EndDate < DateTime.Today &&
                            x.BillingStatus == BillingStatus.Unpaid &&
                            x.CreditCard.Id == App.Data.SelectedCreditCard.Id
                        )
                        .OrderByDescending(x => x.DueDate)
                        .FirstOrDefault();

                    if (billing != null)
                    {
                        this.ViewModel.PaymentDueDate = billing.DueDate;
                        this.ViewModel.TotalAmountDue = billing.SettlementBalance.Value;
                        this.ViewModel.TotalMinimumAmountDue = 500M; // Todo: compute for minimum amount due
                        this.ViewModel.Date = DateTime.Today;
                        this.ViewModel.Amount = this.ViewModel.TotalAmountDue;
                    }

                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }
        }
    }
}
