using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using CCMS.Core.Services;
using CCMS.Data;
using CCMS.Data.Configurations;
using CCMS.UI.Features;
using CCMS.UI.Infrastructure;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Validator.Engine;
using Ninject;
using Ninject.Modules;

namespace CCMS.UI.Bootstrappers.IoC.Modules
{
    public class PersistenceModule : NinjectModule
    {
        public override void Load()
        {
            SessionProvider.Instance.AuditResolver = new UserAuditResolver();

            Bind<ISessionProvider>()
                .ToMethod(x => SessionProvider.Instance)
                .InSingletonScope();

            Bind<ISessionFactory>()
                .ToMethod(x => SessionProvider.Instance.SessionFactory)
                .InSingletonScope();

            Bind<ValidatorEngine>()
                .ToMethod(x => SessionProvider.Instance.ValidatorEngine)
                .InSingletonScope();

            Bind<AuditResolver>()
                .ToMethod(x => SessionProvider.Instance.AuditResolver)
                .InSingletonScope();

            Bind<IComputationService>().ToMethod(_ =>
            {
                if (App.Data.SelectedCreditCard == null)
                    return null;

                var service = (IComputationService)null;

                using (var session = this.KernelInstance.Get<ISessionFactory>().OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    //var query = session.Query<CreditCard>()
                    //    .Where(x => x.Id == App.Data.SelectedCreditCard.Id)
                    //    .Fetch(x => x.Provider)
                    //    .ThenFetchMany(x => x.ComputationSettings)
                    //    .ThenFetch(x => x.MinimumAmount)
                    //    .ThenFetch(x => x.Currency)
                    //    .ToFutureValue();

                    var creditCardAlias = (CreditCard)null;
                    var providerAlias = (CreditCardProvider)null;
                    var settingsAlias = (ComputationSetting)null;
                    var minimumAmountAlias = (Money)null;
                    var currencyAlias = (Currency)null;

                    var query = session.QueryOver<CreditCard>(() => creditCardAlias)
                        .Left.JoinAlias(() => creditCardAlias.Provider, () => providerAlias)
                        .Left.JoinAlias(() => providerAlias.ComputationSettings, () => settingsAlias)
                        .Left.JoinAlias(() => settingsAlias.MinimumAmount, () => minimumAmountAlias)
                        .Left.JoinAlias(() => minimumAmountAlias.Currency, () => currencyAlias)
                        .Where(() => creditCardAlias.Id == App.Data.SelectedCreditCard.Id)
                        .FutureValue();

                    var creditCard = query.Value;
                    service = new ComputationService(creditCard);

                    transaction.Commit();
                }

                return service;
            });

        }
    }
}
