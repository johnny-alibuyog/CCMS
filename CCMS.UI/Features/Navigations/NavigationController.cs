using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CCMS.UI.Features.CreditCards;
using CCMS.UI.Features.Navigations;
using NHibernate;
using NHibernate.Linq;

namespace CCMS.UI.Features.CreditCards
{
    public class NavigationController : ControllerBase<NavigationViewModel>
    {
        public NavigationController(NavigationViewModel viewModel) : base   (viewModel)
        {
            this.MessageBus.Listen<CreditCardRefereshMessage>().Subscribe(x => this.Populate());
            this.MessageBus.SendMessage(new CreditCardRefereshMessage());
        }

        public void Populate()
        {
            try
            {
                // populate items
                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var items = session.Query<Core.Entities.CreditCard>()
                        .Where(x => x.User.Id == App.Data.CurrentUser.Id)
                        .Select(x => new NavigationItemViewModel()
                        {
                            CreditCard = new CreditCardInfoViewModel()
                            {
                                Id = x.Id,
                                AccountName = x.AccountName,
                                AccountNumber = x.AccountNumber,
                                Provider = x.Provider.Id,
                                InterestRate = x.Provider.InterestRate,
                                CuttOff = x.CutOff,
                                TransactionCurrencyId = x.TransactionCurrency.Id,
                                CreditLimit = x.CreditLimit.Value,
                                CashAdvanceLimit = x.CashAdvanceLimit.Value,
                            }
                        })
                        .OrderBy(x => x.CreditCard.Provider)
                        .ToList();

                    ViewModel.Items = new ObservableCollection<NavigationItemViewModel>(items);

                    transaction.Commit();
                }

                var item = this.ViewModel.Items.FirstOrDefault();
                if (item != null)
                    item.SelectItem.Execute(null);
            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }
        }
    }
}
