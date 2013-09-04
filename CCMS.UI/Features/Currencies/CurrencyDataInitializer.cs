using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using CCMS.UI.Bootstrappers.DataInitializers;
using NHibernate;
using NHibernate.Linq;

namespace CCMS.UI.Features.Currencies
{
    public class CurrencyDataInitializer : IDataInitializer
    {
        private readonly ISessionFactory _sessionFactory;

        public CurrencyDataInitializer(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void Execute()
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var entities = session.Query<Currency>().Cacheable().ToList();

                foreach (var toSave in Currency.All)
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
