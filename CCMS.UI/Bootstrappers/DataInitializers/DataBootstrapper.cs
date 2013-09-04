using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features.CreditCardProviders;
using CCMS.UI.Features.Currencies;
using CCMS.UI.Features.TransactionClassifications;

namespace CCMS.UI.Bootstrappers.DataInitializers
{
    public class DataBootstrapper
    {

        public static void Initialize()
        {
            var initializer = (IDataInitializer)null;

            initializer = IoC.IoC.Container.Resolve<CurrencyDataInitializer>();
            initializer.Execute();

            initializer = IoC.IoC.Container.Resolve<CreditCardProviderDataInitializer>();
            initializer.Execute();

            initializer = IoC.IoC.Container.Resolve<TransactionClassificationDataInitializer>();
            initializer.Execute();
        }
    }
}
