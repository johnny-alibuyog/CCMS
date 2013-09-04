using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features;
using CCMS.UI.Features.Billings;
using CCMS.UI.Features.CashAdvances;
using CCMS.UI.Features.CreditCardProviders;
using CCMS.UI.Features.CreditCards;
using CCMS.UI.Features.Currencies;
using CCMS.UI.Features.Installments;
using CCMS.UI.Features.Payments;
using CCMS.UI.Features.Purchases;
using CCMS.UI.Features.Reports;
using CCMS.UI.Features.SplashScreens;
using CCMS.UI.Features.Users;
using Ninject.Modules;

namespace CCMS.UI.Bootstrappers.IoC.Modules
{
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IMessageBoxService>()
                .To<MessageBoxService>();

            //Bind<IDialogService<AuthenticationView, AuthenticationViewModel>>()
            //    .To<AuthenticationDialogService>();

            //Bind<ILoginService>()
            //    .To<LoginController>();

            //Bind<IRegistrationService>()
            //    .To<RegistrationController>();

            //Bind<IDialogService<SplashScreenView, SplashScreenViewModel>>()
            //    .To<SplashScreenDialogService>();

            //Bind<IDialogService<MainView, MainViewModel>>()
            //    .To<MainDialogService>();

            //Bind<ICardInfoService>()
            //    .To<CardInfoController>();

            //Bind<INavigationService>()
            //    .To<NavigationController>();

            //Bind<ISummaryService>()
            //    .To<SummaryController>();

            //Bind<IDialogService<TransactionItemView, TransactionItemViewModel>>()
            //    .To<TransactionItemDialogService>();

            //Bind<IPurchaseListService>()
            //    .To<PurchaseListController>();

            //Bind<IDialogService<PurchaseView, PurchaseViewModel>>()
            //    .To<PurchaseDialogService>();

            //Bind<IDialogService<PurchaseListView, PurchaseListViewModel>>()
            //    .To<PurchaseListDialogService>();

            //Bind<IInstallmentListService>()
            //    .To<InstallmentListController>();

            //Bind<IDialogService<InstallmentView, InstallmentViewModel>>()
            //    .To<InstallmentDialogService>();

            //Bind<IDialogService<InstallmentListView, InstallmentListViewModel>>()
            //    .To<InstallmentListDialogService>();

            //Bind<IBillingService>()
            //    .To<BillingController>();

            //Bind<IBillingListService>()
            //    .To<BillingListController>();

            //Bind<IDialogService<BillingView, BillingViewModel>>()
            //    .To<BillingDialogService>();

            //Bind<IDialogService<BillingItemView, BillingItemViewModel>>()
            //    .To<BillingItemDialogService>();

            //Bind<IDialogService<BillingListView, BillingListViewModel>>()
            //    .To<BillingListDialogService>();

            //Bind<IPaymentListService>()
            //    .To<PaymentListController>();

            //Bind<IDialogService<PaymentView, PaymentViewModel>>()
            //    .To<PaymentDialogService>();

            //Bind<IDialogService<PaymentListView, PaymentListViewModel>>()
            //    .To<PaymentListDialogService>();

            //Bind<ICashAdvanceListService>()
            //    .To<CashAdvanceListController>();

            //Bind<IDialogService<CashAdvanceView, CashAdvanceViewModel>>()
            //    .To<CashAdvanceDialogService>();

            //Bind<IDialogService<CashAdvanceListView, CashAdvanceListViewModel>>()
            //    .To<CashAdvanceListDialogService>();

            //Bind<IProviderService>()
            //    .To<ProviderService>();

            //Bind<IProviderListService>()
            //    .To<ProviderListController>();

            //Bind<IDialogService<ProviderView, ProviderViewModel>>()
            //    .To<ProviderDialogService>();

            //Bind<IDialogService<ProviderListView, ProviderListViewModel>>()
            //    .To<ProviderListDialogService>();

            //Bind<ICreditCardListService>()
            //    .To<CreditCardListController>();

            //Bind<IDialogService<CreditCardView, CreditCardViewModel>>()
            //    .To<CreditCardDialogService>();

            //Bind<IDialogService<CreditCardListView, CreditCardListViewModel>>()
            //    .To<CreditCardListDialogService>();

            //Bind<ICurrencyListService>()
            //    .To<CurrencyListController>();

            //Bind<IDialogService<CurrencyView, CurrencyViewModel>>()
            //    .To<CurrencyDialogService>();

            //Bind<IDialogService<CurrencyListView, CurrencyListViewModel>>()
            //    .To<CurrencyListDialogService>();

            //Bind<ICardTransactionsService>()
            //    .To<CardTransactionsController>();

            //Bind<IStatementSummaryService>()
            //    .To<StatementSummaryController>();

            //Bind<IDialogService<StatementSummaryView, StatementSummaryViewModel>>()
            //    .To<StatementSummaryDialogService>();

            //Bind<IIncommingStatementSummaryService>()
            //    .To<IncommingStatementSummaryController>();

            //Bind<IDialogService<IncommingStatementSummaryView, IncommingStatementSummaryViewModel>>()
            //    .To<IncommingStatementSummaryDialogService>();
        }
    }
}
