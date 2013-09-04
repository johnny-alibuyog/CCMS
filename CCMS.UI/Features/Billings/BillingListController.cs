using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using CCMS.UI.Bootstrappers.IoC;
using CCMS.UI.Features;
using CCMS.UI.Features.Billings;
using NHibernate;
using NHibernate.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.Billings
{
    public class BillingListController : ControllerBase<BillingListViewModel>
    {
        public BillingListController(BillingListViewModel viewModel)
            : base(viewModel)
        {
            this.PopulateList();

            this.ViewModel.Edit = new ReactiveCommand();
            this.ViewModel.Edit.Subscribe(x => Edit((Guid)x));

            this.ViewModel.Delete = new ReactiveCommand();
            this.ViewModel.Delete.Subscribe(x => Delete((BillingViewModel)x));
        }

        #region IBillingService Members

        public virtual void PopulateList()
        {
            try
            {
                using (var session = this.SessionFactory.OpenStatelessSession())
                using (var transaction = session.BeginTransaction())
                {
                    var items = session.Query<Billing>()
                        .Where(x => x.CreditCard.Id == App.Data.SelectedCreditCard.Id)
                        .Select(x => new BillingViewModel()
                        {
                            Id = x.Id,
                            StartDate = x.StartDate,
                            EndDate = x.EndDate,
                            BillingAmount = x.BillingAmount != null
                                ? x.BillingAmount.Value : 0M,
                            PaymentAmount = x.PaymentAmount != null
                                ? x.PaymentAmount.Value : 0M,
                            SettlementBalance = x.SettlementBalance != null
                                ? x.SettlementBalance.Value : 0M,
                            Editable = x.BillingStatus == BillingStatus.Unpaid,
                            Count = x.BillingItems.Count(),
                        })
                        .OrderByDescending(x => x.StartDate)
                        .ToList();

                    this.ViewModel.Items = new ObservableCollection<BillingViewModel>(items);

                    transaction.Commit();
                }
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
                var item = this.ViewModel.Items.Single(x => x.Id == (Guid)id);
                var dialog = IoC.Container.Resolve<BillingDialogService>();
                dialog.ViewModel.Populate(item.Id);
                //dialog.ViewModel.Populate.Execute(item.Id);

                var value = dialog.ShowModal(this, "Edit Billing");
                if (value == null || value.HasAppliedChanges != true)
                    return;

                item.HydrateWith(value);

                this.ViewModel.SelectedItem = item;
                this.ViewModel.AcceptChanges();
            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }
        }

        public virtual void Delete(BillingViewModel item)
        {
            try
            {
                var message = string.Format("Are you sure you want to delete all billings dated: {0}?", item.DateCoveredDisplay);
                var result = this.MessageBox.ShowQuestion(message);
                if (result != MessageBoxResult.OK)
                    return;

                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var billing = session.Get<Core.Entities.Billing>(item.Id);
                    session.Delete(billing);
                    transaction.Commit();

                    this.ViewModel.Items.Remove(item);
                    this.ViewModel.SelectedItem = null;
                    this.ViewModel.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }
        }

        #endregion
    }
}
