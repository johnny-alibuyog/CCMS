using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using NHibernate.Caches.SysCache2;
using NHibernate.Cfg;

namespace CCMS.Data.Configurations
{
    internal static class CacheConfiguration
    {
        public static void Configure(this Configuration config)
        {
            config
                .SetProperty(NHibernate.Cfg.Environment.UseSecondLevelCache, "true")
                .SetProperty(NHibernate.Cfg.Environment.UseQueryCache, "true")
                .Cache(c => c.Provider<SysCacheProvider>())
                .EntityCache<TransactionClassification>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = "TransactionClassification";
                })
                .EntityCache<CreditCardProvider>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = "CreditCardProvider";
                })
                .EntityCache<Currency>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = "Currency";
                })
                .EntityCache<Staff>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = "Staff";
                });
        }
    }
}
