using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using CCMS.UI.Bootstrappers.IoC;
using CCMS.UI.Features;
using CCMS.UI.Features.Billings;
using CCMS.UI.Infrastructure;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Transform;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.Billings
{
    public class BillingController : ControllerBase<BillingViewModel>
    {
        public BillingController(BillingViewModel viewModel)
            : base(viewModel)
        {
            this.ViewModel.ObservableForProperty(x => x.Items).Subscribe(args =>
            {
                var items = args.Value;
                if (items != null)
                    items.CollectionChanged += (sender, e) => this.Recompute();
            });

            //this.ViewModel.Populate = new ReactiveCommand();
            //this.ViewModel.Populate.Subscribe(x => Populate((Guid)x));

            this.ViewModel.CreateAdjustment = new ReactiveCommand();
            this.ViewModel.CreateAdjustment.Subscribe(x => CreateAdjustment());

            this.ViewModel.CreateInterest = new ReactiveCommand();
            this.ViewModel.CreateInterest.Subscribe(x => CreateInterest());

            this.ViewModel.CreateCharge = new ReactiveCommand();
            this.ViewModel.CreateCharge.Subscribe(x => CreateCharge());

            this.ViewModel.CreateFee = new ReactiveCommand();
            this.ViewModel.CreateFee.Subscribe(x => CreateFee());

            this.ViewModel.Delete = new ReactiveCommand();
            this.ViewModel.Delete.Subscribe(x => Delete((BillingItemViewModel)x));

            this.ViewModel.Save = new ReactiveCommand(this.ViewModel
                .WhenAny(x => x.IsValid, x => x.Value == true));
            this.ViewModel.Save.Subscribe(x => Save());
        }

        public virtual void CreateAdjustment()
        {
            try
            {
                var dialog = IoC.Container.Resolve<BillingItemDialogService>();
                dialog.ViewModel = IoC.Container.Resolve<AdjustmentBillingItemViewModel>();
                dialog.ViewModel.Date = this.ViewModel.EndDate;
                dialog.ViewModel.Create = new ReactiveCommand(dialog.ViewModel.WhenAny(x => x.IsValid, x => x.Value == true));
                dialog.ViewModel.Create.Subscribe(x => this.Insert(dialog.ViewModel));
                dialog.ShowModal(this, "Create" + dialog.ViewModel.Title);
            }
            catch (Exception ex)
            {
                this.MessageBox.Warn(ex.Message, ex);
            }
        }

        public virtual void CreateInterest()
        {
            try
            {
                var dialog = IoC.Container.Resolve<BillingItemDialogService>();
                dialog.ViewModel = IoC.Container.Resolve<InterestBillingItemViewModel>();
                dialog.ViewModel.Date = this.ViewModel.EndDate;
                dialog.ViewModel.Create = new ReactiveCommand(dialog.ViewModel.WhenAny(x => x.IsValid, x => x.Value == true));
                dialog.ViewModel.Create.Subscribe(x => this.Insert(dialog.ViewModel));
                dialog.ShowModal(this, "Create" + dialog.ViewModel.Title);
            }
            catch (Exception ex)
            {
                this.MessageBox.Warn(ex.Message, ex);
            }
        }

        public virtual void CreateCharge()
        {
            try
            {
                var dialog = IoC.Container.Resolve<BillingItemDialogService>();
                dialog.ViewModel = IoC.Container.Resolve<ChargeBillingItemViewModel>();
                dialog.ViewModel.Date = this.ViewModel.EndDate;
                dialog.ViewModel.Create = new ReactiveCommand(dialog.ViewModel.WhenAny(x => x.IsValid, x => x.Value == true));
                dialog.ViewModel.Create.Subscribe(x => this.Insert(dialog.ViewModel));
                dialog.ShowModal(this, "Create" + dialog.ViewModel.Title);
            }
            catch (Exception ex)
            {
                this.MessageBox.Warn(ex.Message, ex);
            }
        }

        public virtual void CreateFee()
        {
            try
            {
                var dialog = IoC.Container.Resolve<BillingItemDialogService>();
                dialog.ViewModel = IoC.Container.Resolve<FeeBillingItemViewModel>();
                dialog.ViewModel.Date = this.ViewModel.EndDate;
                dialog.ViewModel.Create = new ReactiveCommand(dialog.ViewModel.WhenAny(x => x.IsValid, x => x.Value == true));
                dialog.ViewModel.Create.Subscribe(x => this.Insert(dialog.ViewModel));
                dialog.ShowModal(this, "Create" + dialog.ViewModel.Title);
            }
            catch (Exception ex)
            {
                this.MessageBox.Warn(ex.Message, ex);
            }
        }

        public virtual void Insert(BillingItemViewModel item)
        {
            var message = string.Format("Do you want to create {0}?", item.Type);
            var result = this.MessageBox.Confirm(message);
            if (result != MessageBoxResult.OK)
                return;

            this.ViewModel.Items.Insert(0, item);
            this.ViewModel.SelectedItem = item;

            item.Close();
        }

        public virtual void Delete(BillingItemViewModel item)
        {
            try
            {
                var message = string.Format("Are you sure you want to delete all billing item: {0}?", item.Details);
                var result = this.MessageBox.Confirm(message);
                if (result != MessageBoxResult.OK)
                    return;

                this.ViewModel.Items.Remove(item);
                this.ViewModel.SelectedItem = null;
            }
            catch (Exception ex)
            {
                this.MessageBox.Warn(ex.Message, ex);
            }
        }

        public void Populate(Guid billingId)
        {
            try
            {
                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    /// Note: QueryOver will fetch joins defined in mapping while Query(Linq) won't
                    var billingAlias = (Billing)null;
                    var creditCardAlias = (CreditCard)null;
                    var currencyAlias = (Currency)null;
                    var billingItemAlias = (BillingItem)null;

                    var query = session.QueryOver<Billing>(() => billingAlias)
                        .Left.JoinAlias(() => billingAlias.CreditCard, () => creditCardAlias)
                        .Left.JoinAlias(() => billingAlias.BillingItems, () => billingItemAlias)
                        .Left.JoinAlias(() => creditCardAlias.TransactionCurrency, () => currencyAlias)
                        .Where(() => billingAlias.Id == billingId)
                        .TransformUsing(Transformers.DistinctRootEntity)
                        .FutureValue();

                    var billing = query.Value;

                    Map(ViewModel, billing);

                    transaction.Commit();

                    //var query = session.Query<Billing>()
                    //    .Where(x => x.Id == billingId)
                    //    .Fetch(x => x.CreditCard)
                    //    .ThenFetch(x => x.TransactionCurrency)
                    //    .FetchMany(x => x.BillingItems)
                    //    .ToFutureValue();

                    //var billing = query.Value; // query.Value;
                    //Map(ViewModel, billing);

                    //transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                this.MessageBox.Warn(ex.Message, ex);
            }
        }

        public void Save()
        {
            try
            {
                var message = string.Format("Are you sure you want save billing?");
                var result = this.MessageBox.Confirm(message);
                if (result != MessageBoxResult.OK)
                    return;

                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    /// Note: QueryOver will fetch joins defined in mapping while Query(Linq) won't
                    var billingAlias = (Billing)null;
                    var creditCardAlias = (CreditCard)null;
                    var currencyAlias = (Currency)null;
                    var billingItemAlias = (BillingItem)null;

                    var query = session.QueryOver<Billing>(() => billingAlias)
                        .Left.JoinAlias(() => billingAlias.CreditCard, () => creditCardAlias)
                        .Left.JoinAlias(() => billingAlias.BillingItems, () => billingItemAlias)
                        .Left.JoinAlias(() => creditCardAlias.TransactionCurrency, () => currencyAlias)
                        .Where(() => billingAlias.Id == this.ViewModel.Id)
                        .TransformUsing(Transformers.DistinctRootEntity)
                        .FutureValue();

                    var billing = query.Value;
                    Map(billing, ViewModel);

                    transaction.Commit();

                    //var query = session.Query<Billing>()
                    //    .Where(x => x.Id == this.ViewModel.Id)
                    //    .Fetch(x => x.CreditCard)
                    //    .ThenFetch(x => x.TransactionCurrency)
                    //    .FetchMany(x => x.BillingItems)
                    //    .ToFutureValue();

                    //var billing = query.Value;
                    //Map(billing, ViewModel);

                    //transaction.Commit();
                }

                this.MessageBox.Inform("Save has been successfully completed.");
            }
            catch (Exception ex)
            {
                this.MessageBox.Warn(ex.Message, ex);
            }
        }

        #region Routine Helpers

        public virtual void Recompute()
        {
            this.ViewModel.Count = this.ViewModel.Items.Count;
            this.ViewModel.BillingAmount = this.ViewModel.Items.Sum(x => x.Amount);
            this.ViewModel.SettlementBalance = this.ViewModel.BillingAmount - this.ViewModel.PaymentAmount;
        }

        private void Map(BillingViewModel target, Billing source)
        {
            target.Id = source.Id;
            target.StartDate = source.StartDate;
            target.EndDate = source.EndDate;
            target.StatementDate = source.StatementDate;
            target.DueDate = source.DueDate;
            target.BillingAmount = source.BillingAmount != null
                ? source.BillingAmount.Value : 0M;
            target.PaymentAmount = source.PaymentAmount != null
                ? source.PaymentAmount.Value : 0M;
            target.SettlementBalance = source.SettlementBalance != null
                ? source.SettlementBalance.Value : 0M;
            target.Count = source.BillingItems.Count();
            target.Items = new ChangeTrackingObservableCollection<BillingItemViewModel>(new List<BillingItemViewModel>()
                .Concat(source.BillingItems
                .OfType<AdjustmentBillingItem>()
                .Select(x => new AdjustmentBillingItemViewModel()
                {
                    Id = x.Id,
                    Date = x.Date,
                    Details = x.Details,
                    Amount = x.Amount.Value
                }))
                .Concat(source.BillingItems
                .OfType<CashAdvanceBillingItem>()
                .Select(x => new CashAdvanceBillingItemViewModel()
                {
                    Id = x.Id,
                    Date = x.Date,
                    Details = x.Details,
                    Amount = x.Amount.Value
                }))
                .Concat(source.BillingItems
                .OfType<ChargeBillingItem>()
                .Select(x => new ChargeBillingItemViewModel()
                {
                    Id = x.Id,
                    Date = x.Date,
                    Details = x.Details,
                    Amount = x.Amount.Value
                }))
                .Concat(source.BillingItems
                .OfType<FeeBillingItem>()
                .Select(x => new FeeBillingItemViewModel()
                {
                    Id = x.Id,
                    Date = x.Date,
                    Details = x.Details,
                    Amount = x.Amount.Value
                }))
                .Concat(source.BillingItems
                .OfType<InstallmentBillingItem>()
                .Select(x => new InstallmentBillingItemViewModel()
                {
                    Id = x.Id,
                    Date = x.Date,
                    Details = x.Details,
                    Amount = x.Amount.Value
                }))
                .Concat(source.BillingItems
                .OfType<InterestBillingItem>()
                .Select(x => new InterestBillingItemViewModel()
                {
                    Id = x.Id,
                    Date = x.Date,
                    Details = x.Details,
                    Amount = x.Amount.Value
                }))
                .Concat(source.BillingItems
                .OfType<PurchaseBillingItem>()
                .Select(x => new PurchaseBillingItemViewModel()
                {
                    Id = x.Id,
                    Date = x.Date,
                    Details = x.Details,
                    Amount = x.Amount.Value
                }))
                .OrderByDescending(x => x.Date)
            );
        }

        private void Map(Billing target, BillingViewModel source)
        {
            target.StartDate = source.StartDate;
            target.EndDate = source.EndDate;
            target.StatementDate = source.StatementDate;
            target.DueDate = source.DueDate;

            var deletedItemIds = source.Items.GetDeletedItems()
                .Select(x => x.Id)
                .ToArray();

            // delete items
            var itemsToDelete = target.BillingItems
                .Where(x => deletedItemIds.Contains(x.Id))
                .ToList();

            // insert items
            var itemsToInsert = source.Items.GetInsertedItems()
                .Where(x => x.Id == Guid.Empty)
                .Select(x =>
                {
                    BillingItem billingItem = null;
                    if (x is AdjustmentBillingItemViewModel)
                    {
                        // adjustment 
                        billingItem = new AdjustmentBillingItem(
                            new Adjustment()
                            {
                                CreditCard = target.CreditCard,
                                Date = x.Date,
                                Details = x.Details,
                                Amount = target.CreditCard.ResolveMoney(x.Amount),
                            }
                        );
                    }
                    else if (x is ChargeBillingItemViewModel)
                    {
                        // charge
                        billingItem = new ChargeBillingItem()
                        {
                            Date = x.Date,
                            Details = x.Details,
                            Amount = target.CreditCard.ResolveMoney(x.Amount),
                        };
                    }
                    else if (x is FeeBillingItemViewModel)
                    {
                        // fee
                        billingItem = new FeeBillingItem()
                        {
                            Date = x.Date,
                            Details = x.Details,
                            Amount = target.CreditCard.ResolveMoney(x.Amount),
                        };
                    }
                    else if (x is InstallmentBillingItemViewModel)
                    {
                        // installment
                        billingItem = new InstallmentBillingItem()
                        {
                            Date = x.Date,
                            Details = x.Details,
                            Amount = target.CreditCard.ResolveMoney(x.Amount),
                        };
                    }
                    else if (x is InterestBillingItemViewModel)
                    {
                        // interest
                        billingItem = new InterestBillingItem()
                        {
                            Date = x.Date,
                            Details = x.Details,
                            Amount = target.CreditCard.ResolveMoney(x.Amount),
                        };
                    }
                    else
                    {
                        // purchase
                        billingItem = new PurchaseBillingItem(
                            new Purchase()
                            {
                                CreditCard = target.CreditCard,
                                Date = x.Date,
                                Details = x.Details,
                                Amount = target.CreditCard.ResolveMoney(x.Amount),
                            }
                        );
                    }
                    return billingItem;
                })
                .ToList();

            foreach (var item in itemsToDelete)
                target.RemoveBillingItem(item);

            foreach (var item in itemsToInsert)
                target.AddBillingItem(item);
        }

        #endregion
    }
}