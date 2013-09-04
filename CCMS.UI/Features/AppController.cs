using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using NHibernate.Transform;
using ReactiveUI;

namespace CCMS.UI.Features
{
    public class AppController : ControllerBase<AppViewModel>
    {
        public AppController(AppViewModel viewModel) : base(viewModel)
        {
            //this.ViewModel
            //    .ObservableForProperty(x => x.SelectedCreditCard)
            //    .ValueIfNotDefault()
            //    .Subscribe(x => ConsolidateBillings(x.Id));

            this.ViewModel.ObservableForProperty(x => x.SelectedCreditCard).Value()
                .Subscribe(x => ConsolidateBillings(x.Id));
        }

        private void ConsolidateBillings(Guid creditCardId)
        {
            // won't be necessary
            //using (var session = this.SessionFactory.OpenSession())
            //using (var transaction = session.BeginTransaction())
            //{
            //    var creditCardAlias = (CreditCard)null;
            //    var billingAlias = (Billing)null;
            //    var billingItemAlias = (BillingItem)null;

            //    var billingQuery = session.QueryOver<Billing>(() => billingAlias)
            //        .Left.JoinAlias(() => billingAlias.CreditCard, () => creditCardAlias)
            //        .Left.JoinAlias(() => billingAlias.BillingItems, () => billingItemAlias)
            //        .Where(() =>
            //            creditCardAlias.Id == creditCardId &&
            //            billingAlias.EndDate < DateTime.Today &&
            //            billingAlias.BillingStatus == BillingStatus.Unpaid
            //        )
            //        .OrderBy(() => billingAlias.EndDate).Desc
            //        .TransformUsing(Transformers.DistinctRootEntity)
            //        .Future();

            //    var currentBilling = billingQuery.FirstOrDefault();
            //    if (currentBilling == null)
            //        return;

            //    // merge previous to current billing
            //    var previousBillings = billingQuery.Where(x => x != currentBilling);
            //    currentBilling.MergeWith(previousBillings);

            //    transaction.Commit();
            //}
        }
    }
}
