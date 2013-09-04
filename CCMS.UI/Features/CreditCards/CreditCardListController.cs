using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CCMS.UI.Bootstrappers.IoC;
using CCMS.UI.Common;
using CCMS.UI.Common.Extentions;
using CCMS.UI.Features.CreditCards;
using NHibernate;
using NHibernate.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.CreditCards
{
    public class CreditCardListController : ControllerBase<CreditCardListViewModel>
    {
        public CreditCardListController(CreditCardListViewModel viewModel)
            : base(viewModel)
        {
            PopulateList();

            this.ViewModel.Create = new ReactiveCommand();
            this.ViewModel.Create.Subscribe(x => Create());

            this.ViewModel.Edit = new ReactiveCommand();
            this.ViewModel.Edit.Subscribe(x => Edit((Guid)x));

            this.ViewModel.Delete = new ReactiveCommand();
            this.ViewModel.Delete.Subscribe(x => Delete((Guid)x));
        }

        public virtual void Create()
        {
            try
            {
                var dialog = IoC.Container.Resolve<CreditCardDialogService>();
                dialog.ViewModel.Providers = this.ViewModel.Providers;
                dialog.ViewModel.Currencies = this.ViewModel.Currencies;
                dialog.ViewModel.Save = new ReactiveCommand(dialog.ViewModel.IsValidObservable());
                dialog.ViewModel.Save.Subscribe(x => this.Insert(dialog.ViewModel));
                dialog.ShowModal(this, "Create Credit Card");

                //var item = dialog.ShowModal(this, "Create Credit Card");
                //if (item != null)
                //{
                //    if (this.Insert(item))
                //    {
                //        this.ViewModel.Items.Insert(0, item);
                //        this.ViewModel.SelectedItem = item;
                //        this.ViewModel.AcceptChanges();
                //    }
                //}
            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }
        }

        public virtual void Insert(CreditCardViewModel item)
        {
            try
            {
                var message = string.Format("Do you want to save Credit Card?");
                var result = this.MessageBox.ShowQuestion(message);
                if (result != MessageBoxResult.OK)
                    return;
 
                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var creditCard = new Core.Entities.CreditCard()
                    {
                        AccountNumber = item.AccountNumber,
                        AccountName = item.AccountName,
                        CutOff = item.CutOff,
                        IssueDate = item.IssueDate,
                        ExpiryDate = item.ExpiryDate,
                        User = session.Load<Core.Entities.User>(App.Data.CurrentUser.Id),
                        Provider = session.Load<Core.Entities.CreditCardProvider>(item.Provider.Key),
                        TransactionCurrency = session.Load<Core.Entities.Currency>(item.TransactionCurrency.Key),
                        CreditLimit = new Core.Entities.Money(
                            currency: session.Load<Core.Entities.Currency>(item.TransactionCurrency.Key),
                            value: item.CreditLimit
                        ),
                        CashAdvanceLimit = new Core.Entities.Money(
                            currency: session.Load<Core.Entities.Currency>(item.TransactionCurrency.Key),
                            value: item.CashAdvanceLimit
                        ),
                        AvailableCredit = new Core.Entities.Money(
                            currency: session.Load<Core.Entities.Currency>(item.TransactionCurrency.Key),
                            value: item.CreditLimit
                        ),
                        OutstandingBalance = new Core.Entities.Money(
                            currency: session.Load<Core.Entities.Currency>(item.TransactionCurrency.Key),
                            value: 0M
                        ),

                    };

                    session.Save(creditCard);

                    transaction.Commit();

                    item.Id = creditCard.Id;
                }

                this.MessageBox.ShowInformation("Save has been successfully completed.");

                this.ViewModel.Items.Insert(0, item);
                this.ViewModel.SelectedItem = item;
                this.ViewModel.AcceptChanges();

                item.Close();
            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }
        }

        public virtual void Edit(Guid id)
        {
            try
            {
                var item = this.ViewModel.Items.Single(x => x.Id == id);
                this.ViewModel.SelectedItem = item;

                var dialog = IoC.Container.Resolve<CreditCardDialogService>();
                dialog.ViewModel.HydrateWith(item);
                dialog.ViewModel.Providers = this.ViewModel.Providers;
                dialog.ViewModel.Currencies = this.ViewModel.Currencies;
                dialog.ViewModel.Save = new ReactiveCommand(dialog.ViewModel.IsValidObservable());
                dialog.ViewModel.Save.Subscribe(x => Update(dialog.ViewModel));
                dialog.ShowModal(this, "Edit Credit Card");

                //var value = dialog.ShowModal(this, "Edit Credit Card");
                //if (value != null)
                //{
                //    if (this.Update(value))
                //    {
                //        item.HydrateWith(value);

                //        this.ViewModel.SelectedItem = item;
                //        this.ViewModel.AcceptChanges();
                //    }
                //}
            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }
        }

        public virtual void Update(CreditCardViewModel item)
        {
            try
            {
                var message = string.Format("Do you want to save Credit Card?");
                var result = this.MessageBox.ShowQuestion(message);
                if (result != MessageBoxResult.OK)
                    return;

                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var creditCard = session.Get<Core.Entities.CreditCard>(item.Id);
                    creditCard.AccountNumber = item.AccountNumber;
                    creditCard.AccountName = item.AccountName;
                    creditCard.CutOff = item.CutOff;
                    creditCard.IssueDate = item.IssueDate;
                    creditCard.ExpiryDate = item.ExpiryDate;
                    creditCard.User = session.Load<Core.Entities.User>(App.Data.CurrentUser.Id);
                    creditCard.Provider = session.Load<Core.Entities.CreditCardProvider>(item.Provider.Key);
                    creditCard.TransactionCurrency = session.Load<Core.Entities.Currency>(item.TransactionCurrency.Key);
                    creditCard.CreditLimit = new Core.Entities.Money(
                        currency: session.Load<Core.Entities.Currency>(item.TransactionCurrency.Key),
                        value: item.CreditLimit
                    );
                    creditCard.CashAdvanceLimit = new Core.Entities.Money(
                        currency: session.Load<Core.Entities.Currency>(item.TransactionCurrency.Key),
                        value: item.CashAdvanceLimit
                    );

                    transaction.Commit();
                }

                this.MessageBox.ShowInformation("Save has been successfully completed.");

                this.ViewModel.SelectedItem.HydrateWith(item);
                this.ViewModel.AcceptChanges();

                item.Close();
            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }
        }

        public virtual void Delete(Guid id)
        {
            try
            {
                var item = this.ViewModel.Items.Single(x => x.Id == id);
                var message = string.Format("Are you sure you want to delete account: {0}?", item.AccountName);
                var result = this.MessageBox.ShowQuestion(message);
                if (result != MessageBoxResult.OK)
                    return;

                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var creditCard = session.Get<Core.Entities.CreditCard>(item.Id);
                    session.Delete(creditCard);
                    transaction.Commit();
                }

                this.MessageBox.ShowInformation("Delete has been successfully completed.");

                this.ViewModel.Items.Remove(item);
                this.ViewModel.SelectedItem = null;
                this.ViewModel.AcceptChanges();
            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }
        }

        public virtual void PopulateList()
        {
            try
            {
                using (var session = this.SessionFactory.OpenStatelessSession())
                using (var transaction = session.BeginTransaction())
                {
                    var currencies = session.Query<Core.Entities.Currency>().Cacheable().ToList()
                        .Select(x => new KeyValuePair<string, string>(x.Id, x.Name));

                    var providers = session.Query<Core.Entities.CreditCardProvider>().Cacheable().ToList()
                        .Select(x => new KeyValuePair<string, string>(x.Id, x.Name));

                    ViewModel.Providers = new ObservableCollection<KeyValuePair<string, string>>(providers);
                    ViewModel.Currencies = new ObservableCollection<KeyValuePair<string, string>>(currencies);

                    var items = session.Query<Core.Entities.CreditCard>()
                        .Where(x => x.User.Id == App.Data.CurrentUser.Id)
                        .Select(x => new CreditCardViewModel()
                        {
                            Id = x.Id,
                            AccountNumber = x.AccountNumber,
                            AccountName = x.AccountName,
                            CutOff = x.CutOff,
                            IssueDate = x.IssueDate,
                            ExpiryDate = x.ExpiryDate,
                            Provider = ViewModel.Providers.Get(x.Provider.Id),
                            TransactionCurrency = ViewModel.Currencies.Get(x.TransactionCurrency.Id),
                            CreditLimit = x.CreditLimit.Value,
                            CashAdvanceLimit = x.CashAdvanceLimit.Value,
                        })
                        .ToList();

                    ViewModel.Items = new ObservableCollection<CreditCardViewModel>(items);

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
