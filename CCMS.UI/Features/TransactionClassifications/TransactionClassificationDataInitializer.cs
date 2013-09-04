using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using CCMS.UI.Bootstrappers.DataInitializers;
using NHibernate;
using NHibernate.Linq;

namespace CCMS.UI.Features.TransactionClassifications
{
    public class TransactionClassificationDataInitializer : IDataInitializer
    {
        private readonly ISessionFactory _sessionFactory;

        public TransactionClassificationDataInitializer(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void Execute()
        {
            var list = new string[]
            {
                "Travel", 
                "Supplies",
                "Equipments", 
                "Medicines", 
                "Constructions", 
                "Furnitures & Fixtures", 
            };

            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var entities = session.Query<TransactionClassification>().Cacheable().ToList();

                foreach (var item in list)
                {
                    if (entities.Any(x => x.Name.Equals(item, StringComparison.InvariantCultureIgnoreCase)))
                        continue;

                    var entity = new TransactionClassification() { Name = item };

                    session.Save(entity);
                }

                transaction.Commit();
            }
        }
    }
}
