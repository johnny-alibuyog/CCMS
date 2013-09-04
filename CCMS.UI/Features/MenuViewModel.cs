using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CCMS.UI.Bootstrappers.IoC;
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
using CCMS.UI.Features.Staffs;
using CCMS.UI.Features.TransactionClassifications;
using CCMS.UI.Infrastructure;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features
{
    public class MenuViewModel : ViewModelBase
    {
        #region Properties

        public virtual ObservableCollection<MenuItemViewModel> Transactions { get; set; }

        public virtual ObservableCollection<MenuItemViewModel> Maintenance { get; set; }

        public virtual ObservableCollection<MenuItemViewModel> Reports { get; set; }

        #endregion

        #region Routine Helpers

        private void FireCreditCardRefreshMessage()
        {
            this.MessageBus.SendMessage(new CreditCardRefereshMessage());
        }

        private void FireRefreshMessage()
        {
            this.MessageBus.SendMessage(new RefreshMessage());
        }

        private ObservableCollection<MenuItemViewModel> CreateTransactionMenuItems()
        {
            return new ObservableCollection<MenuItemViewModel>()
            {
                new MenuItemViewModel()
                { 
                    Text =  "_Purchases",
                    Command =  ReactiveUI.Legacy.ReactiveCommand.Create(x => true, x => 
                    {
                        var dialog = IoC.Container.Resolve<PurchaseListDialogService>();
                        dialog.ShowModal();
                        if (dialog.ViewModel.HasAppliedChanges)
                            FireRefreshMessage();
                    })
                },
                new MenuItemViewModel()
                {
                    Text = "_Installments",
                    Command =  ReactiveUI.Legacy.ReactiveCommand.Create(x => true, x => 
                    {
                        var dialog = IoC.Container.Resolve<InstallmentListDialogService>();
                        dialog.ShowModal();
                        if (dialog.ViewModel.HasAppliedChanges)
                            FireRefreshMessage();
                    })
                },
                new MenuItemViewModel()
                {
                    Text = "_Cash Advances",
                    Command =  ReactiveUI.Legacy.ReactiveCommand.Create(x => true, x => 
                    {
                        var dialog = IoC.Container.Resolve<CashAdvanceListDialogService>();
                        dialog.ShowModal();
                        if (dialog.ViewModel.HasAppliedChanges)
                            FireRefreshMessage();
                    })
                },
                new MenuItemViewModel()
                {
                    Text = "_Billings",
                    Command =  ReactiveUI.Legacy.ReactiveCommand.Create(x => true, x => 
                    {
                        var dialog = IoC.Container.Resolve<BillingListDialogService>();
                        dialog.ShowModal();
                        if (dialog.ViewModel.HasAppliedChanges)
                            FireRefreshMessage();
                    })
                },
                new MenuItemViewModel()
                {
                    Text = "Pa_yments",
                    Command =  ReactiveUI.Legacy.ReactiveCommand.Create(x => true, x => 
                    {
                        var dialog = IoC.Container.Resolve<PaymentListDialogService>();
                        dialog.ShowModal();
                        if (dialog.ViewModel.HasAppliedChanges)
                            FireRefreshMessage();
                    })
                },
            };
        }

        private ObservableCollection<MenuItemViewModel> CreateMaintenanceMenuItems()
        {
            return new ObservableCollection<MenuItemViewModel>()
            {
                new MenuItemViewModel()
                {
                    Text = "_Staff",
                    Command =  ReactiveUI.Legacy.ReactiveCommand.Create(x => true, x => 
                    {
                        var dialog = IoC.Container.Resolve<StaffListDialogService>();
                        dialog.ShowModal();
                        if (dialog.ViewModel.HasAppliedChanges)
                            FireCreditCardRefreshMessage();
                    })
                },
                new MenuItemViewModel()
                {
                    Text = "Cur_rency",
                    Command =  ReactiveUI.Legacy.ReactiveCommand.Create(x => true, x => 
                    {
                        var dialog = IoC.Container.Resolve<CurrencyListDialogService>();
                        dialog.ShowModal();
                        //if (dialog.ViewModel.HasAppliedChanges)
                        //    FireRefreshMessage();
                    })
                },
                new MenuItemViewModel()
                {
                    Text = "Credit _Card",
                    Command =  ReactiveUI.Legacy.ReactiveCommand.Create(x => true, x => 
                    {
                        var dialog = IoC.Container.Resolve<CreditCardListDialogService>();
                        dialog.ShowModal();
                        if (dialog.ViewModel.HasAppliedChanges)
                            FireCreditCardRefreshMessage();
                    })
                },
                new MenuItemViewModel()
                {
                    Text = "Credit Card _Providers",
                    Command =  ReactiveUI.Legacy.ReactiveCommand.Create(x => true, x => 
                    {
                        var dialog = IoC.Container.Resolve<ProviderListDialogService>();
                        dialog.ShowModal();
                        if (dialog.ViewModel.HasAppliedChanges)
                            FireRefreshMessage();
                    })
                },
                new MenuItemViewModel()
                {
                    Text = "Transaction Classifications",
                    Command =  ReactiveUI.Legacy.ReactiveCommand.Create(x => true, x => 
                    {
                        var dialog = IoC.Container.Resolve<ClassificationListDialogService>();
                        dialog.ShowModal();
                        //if (dialog.ViewModel.HasAppliedChanges)
                        //    FireRefreshMessage();
                    })
                },
            };
        }

        private ObservableCollection<MenuItemViewModel> CreateReportsMenuItems()
        {
            return new ObservableCollection<MenuItemViewModel>()
            {
                new MenuItemViewModel()
                {
                    Text = "_Annual Summary Report",
                    Command =  ReactiveUI.Legacy.ReactiveCommand.Create(x => true, x => 
                    {
                        var dialog = IoC.Container.Resolve<AnnualSummaryDialogService>();
                        dialog.ShowModal();
                    })
                },
                new MenuItemViewModel()
                {
                    Text = "_Expenses By Classification Report",
                    Command =  ReactiveUI.Legacy.ReactiveCommand.Create(x => true, x => 
                    {
                        var dialog = IoC.Container.Resolve<ExpensesByClassificationDialogService>();
                        dialog.ShowModal();
                    })
                },
                new MenuItemViewModel()
                {
                    Text = "Statement _Summary Report",
                    Command =  ReactiveUI.Legacy.ReactiveCommand.Create(x => true, x => 
                    {
                        var dialog = IoC.Container.Resolve<StatementSummaryDialogService>();
                        dialog.ShowModal();
                    })
                },
                new MenuItemViewModel()
                {
                    Text = "_Incomming Statement Summary Report",
                    Command =  ReactiveUI.Legacy.ReactiveCommand.Create(x => true, x => 
                    {
                        var dialog = IoC.Container.Resolve<IncommingStatementSummaryDialogService>();
                        dialog.ShowModal();
                    })
                },
            };
        }

        #endregion

        #region Constructors

        public MenuViewModel()
        {
            this.Transactions = CreateTransactionMenuItems();
            this.Maintenance = CreateMaintenanceMenuItems();
            this.Reports = CreateReportsMenuItems();
        }

        #endregion
    }
}
