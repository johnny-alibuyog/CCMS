using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features;
using CCMS.UI.Features.Contents;
using CCMS.UI.Features.CreditCardProviders;
using CCMS.UI.Features.CreditCards;
using CCMS.UI.Features.Currencies;
using CCMS.UI.Features.Installments;
using CCMS.UI.Features.Navigations;
using CCMS.UI.Features.Payments;
using CCMS.UI.Features.Purchases;
using CCMS.UI.Features.Reports;
using CCMS.UI.Features.SplashScreens;
using CCMS.UI.Features.Summaries;
using CCMS.UI.Features.Users;
using Ninject.Modules;

namespace CCMS.UI.Bootstrappers.IoC.Modules
{
    public class ViewModelModules : NinjectModule
    {
        public override void Load()
        {
            Bind<LoginViewModel>()
                .ToSelf();

            Bind<RegistrationViewModel>()
                .ToSelf();

            Bind<AuthenticationViewModel>()
                .ToSelf();

            Bind<SplashScreenViewModel>()
                .ToSelf();

            Bind<MainViewModel>()
                .ToSelf();

            Bind<MenuViewModel>()
                .ToSelf();

            Bind<SummaryViewModel>()
                .ToSelf();

            Bind<NavigationViewModel>()
                .ToSelf();

            Bind<NavigationItemViewModel>()
                .ToSelf();

            Bind<ContentViewModel>()
                .ToSelf();

            Bind<CardInfoViewModel>()
                .ToSelf();

            Bind<CardTransactionsViewModel>()
                .ToSelf();

            Bind<InstallmentViewModel>()
                .ToSelf();

            Bind<InstallmentListViewModel>()
                .ToSelf();

            Bind<PurchaseViewModel>()
                .ToSelf();

            Bind<PurchaseListViewModel>()
                .ToSelf();

            Bind<PaymentViewModel>()
                .ToSelf();
            
            Bind<PaymentListViewModel>()
                .ToSelf();

            Bind<ProviderViewModel>()
                .ToSelf();

            Bind<ProviderListViewModel>()
                .ToSelf();

            Bind<CreditCardViewModel>()
                .ToSelf();

            Bind<CreditCardListViewModel>()
                .ToSelf();

            Bind<CurrencyViewModel>()
                .ToSelf();

            Bind<CurrencyListViewModel>()
                .ToSelf();

            Bind<StatementSummaryViewModel>()
                .ToSelf();

            Bind<IncommingStatementSummaryViewModel>()
                .ToSelf();
        }
    }
}
