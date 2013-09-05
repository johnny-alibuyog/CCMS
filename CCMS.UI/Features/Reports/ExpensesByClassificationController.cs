using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using CCMS.Core.Common.Extentions;
using ReactiveUI;
using ReactiveUI.Xaml;
using NHibernate;
using NHibernate.Linq;

namespace CCMS.UI.Features.Reports
{
    public class ExpensesByClassificationController : ControllerBase<ExpensesByClassificationViewModel>
    {
        public ExpensesByClassificationController(ExpensesByClassificationViewModel viewModel)
            : base(viewModel)
        {
            this.ViewModel.FromDate = new DateTime(DateTime.Today.Year, 1, 1);
            this.ViewModel.ToDate = new DateTime(DateTime.Today.Year, 12, 31);

            this.PopulateLookup();

            this.ViewModel.Generate = new ReactiveCommand();
            this.ViewModel.Generate.Subscribe(x => Populate());

            this.ViewModel.WhenAny(x => x.SelectedCreditCard, x => true)
                .Subscribe(x => Populate());
        }

        private void Populate()
        {
            try
            {
                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<Billing>();

                    if (this.ViewModel.FromDate < this.ViewModel.ToDate)
                        query = query.Where(x =>
                            x.StatementDate >= this.ViewModel.FromDate &&
                            x.StatementDate <= this.ViewModel.ToDate);

                    if (this.ViewModel.SelectedCreditCard.Key != Guid.Empty)
                        query = query.Where(x => x.CreditCard.Id == this.ViewModel.SelectedCreditCard.Key);

                    var items = query
                        .Fetch(x => x.CreditCard)
                        .ThenFetch(x => x.Provider)
                        .FetchMany(x => x.BillingItems)
                        .ToList();

                    this.ViewModel.Items = items
                        .Select(x => new ExpensesByClassificationItemViewModel()
                        {
                            AccountNumber = x.CreditCard.AccountNumber,
                            Provider = x.CreditCard.Provider.Name,
                            StatementDate = x.StatementDate,
                            Medecine = x.BillingItems
                                .Where(o => o.GetTransactionClassifiaction() == "Medicines")
                                .Sum(o => o.Amount.GetValue()),
                            Construction = x.BillingItems
                                .Where(o => o.GetTransactionClassifiaction() == "Constructions")
                                .Sum(o => o.Amount.GetValue()),
                            Travel = x.BillingItems
                                .Where(o => o.GetTransactionClassifiaction() == "Travel")
                                .Sum(o => o.Amount.GetValue()),
                            FurnitureAndFixture = x.BillingItems
                                .Where(o => o.GetTransactionClassifiaction() == "Furnitures & Fixtures")
                                .Sum(o => o.Amount.GetValue()),
                            Equipment = x.BillingItems
                                .Where(o => o.GetTransactionClassifiaction() == "Equipments")
                                .Sum(o => o.Amount.GetValue()),
                            Supplies = x.BillingItems
                                .Where(o => o.GetTransactionClassifiaction() == "Supplies")
                                .Sum(o => o.Amount.GetValue()),
                            Others = x.BillingItems
                                .Where(o => o.GetTransactionClassifiaction() == string.Empty)
                                .Sum(o => o.Amount.GetValue()),
                            TotalExpenses = x.BillingAmount.GetValue(),
                            TotalPayments = x.PaymentAmount.GetValue() + x.PreviousBillingAmount.GetValue()
                        })
                        .ToList();


                    transaction.Commit();
                }

                if (this.ViewModel.Render != null)
                    this.ViewModel.Render.Invoke();
            }
            catch (Exception ex)
            {
                this.MessageBox.Warn(ex.Message, ex);
            }
        }

        private void PopulateLookup()
        {
            try
            {
                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var items = session.Query<CreditCard>()
                        .Select(x => new
                        {
                            Key = x.Id,
                            Value = string.Format("[{0}] {1}", x.AccountNumber, x.Provider.Id)
                        })
                        .ToList();

                    items.Insert(0, new { Key = Guid.Empty, Value = "<< All >>" });

                    this.ViewModel.CreditCards = items.Select(x => new KeyValuePair<Guid, string>(x.Key, x.Value)).ToList();
                    this.ViewModel.SelectedCreditCard = this.ViewModel.CreditCards.FirstOrDefault();

                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                this.MessageBox.Warn(ex.Message, ex);
            }
        }


    }
}
