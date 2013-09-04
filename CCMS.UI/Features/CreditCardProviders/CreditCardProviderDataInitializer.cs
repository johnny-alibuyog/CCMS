using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using CCMS.UI.Bootstrappers.DataInitializers;
using NHibernate;
using NHibernate.Linq;

namespace CCMS.UI.Features.CreditCardProviders
{
    public class CreditCardProviderDataInitializer : IDataInitializer
    {
     private readonly ISessionFactory _sessionFactory;

     public CreditCardProviderDataInitializer(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void Execute()
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var entities = session.Query<CreditCardProvider>()
                    .FetchMany(x => x.ComputationSettings)
                    .ToFuture();

                //var entities = query.ToList();

                foreach (var toSave in CreditCardProvider.All)
                {
                    if (entities.Contains(toSave))
                        continue;

                    session.Save(toSave);
                }

                transaction.Commit();
            }
        }
    }
}
