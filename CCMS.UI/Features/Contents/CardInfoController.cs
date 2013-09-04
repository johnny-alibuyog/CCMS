using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using CCMS.UI.Features.Contents;
using CCMS.UI.Features;
using CCMS.UI.Features.CreditCards;
using NHibernate;
using NHibernate.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.Contents
{
    public class CardInfoController : ControllerBase<CardInfoViewModel>
    {
        public CardInfoController(CardInfoViewModel viewModel) : base(viewModel)
        {
            this.MessageBus.Listen<RefreshMessage>()
                .Subscribe(x => Populate(App.Data.SelectedCreditCard.Id));

            this.MessageBus.Listen<CreditCardSelectedMessage>()
                .Subscribe(x => Populate(x.CreditCard.Id));
        }

        public void Populate(Guid creditCardId)
        {
            try
            {
                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var creditCard = session.Query<CreditCard>()
                        .Where(x => x.Id == creditCardId)
                        .Select(x => new
                        {
                            AccountName = x.AccountName,
                            AccountNumber = x.AccountNumber,
                            Provider = new
                            {
                                Name = x.Provider.Name,
                                InterestRate = x.Provider.InterestRate
                            },
                            CutOff = x.CutOff,
                            IssueDate = x.IssueDate,
                            ExpiryDate = x.ExpiryDate,
                            CreditLimit = x.CreditLimit,
                            CashAdvanceLimit = x.CashAdvanceLimit,
                            OutstandingBalance = x.OutstandingBalance,
                            AvailableCredit = x.AvailableCredit,
                            NextCutOffDate = DateTime.Today.AddMonths(1)
                        })
                        .FirstOrDefault();

                    ViewModel.AccountName = creditCard.AccountName;
                    ViewModel.AccountNumber = creditCard.AccountNumber;
                    ViewModel.Provider = creditCard.Provider.Name;
                    ViewModel.CutOff = creditCard.CutOff;
                    ViewModel.IssueDate = creditCard.IssueDate;
                    ViewModel.ExpiryDate = creditCard.ExpiryDate;
                    ViewModel.InterestRate = creditCard.Provider.InterestRate;
                    ViewModel.CreditLimit = creditCard.CreditLimit.ToString();
                    ViewModel.CashAdvanceLimit = creditCard.CashAdvanceLimit.ToString();
                    ViewModel.OutstandingBalance = creditCard.OutstandingBalance.ToString();
                    ViewModel.AvailableCredit = creditCard.AvailableCredit.ToString();
                    ViewModel.NextCutOffDate = DateTime.Today.AddMonths(1);

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
