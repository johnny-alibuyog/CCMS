using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CCMS.UI.Bootstrappers.IoC;
using CCMS.UI.Features;
using CCMS.UI.Features.CreditCardProviders;
using CCMS.UI.Infrastructure;
using NHibernate.Validator.Constraints;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.CreditCardProviders
{
    public class ProviderViewModel : ViewModelBase
    {
        private readonly ProviderController _controller;

        [NotNullNotEmpty]
        public virtual string Id { get; set; }

        [NotNullNotEmpty]
        public virtual string Name { get; set; }

        public virtual decimal InterestRate { get; set; }

        public virtual bool IsEditMode { get; set; }

        public virtual ChangeTrackingObservableCollection<ComputationSettingViewModel> ComputationSettings { get; set; }

        public virtual ComputationSettingViewModel SelectedComputationSetting { get; set; }

        public virtual ObservableCollection<KeyValuePair<string, string>> Currencies { get; set; }

        public virtual IReactiveCommand Populate { get; set; }

        public virtual IReactiveCommand CreateFinnanceChargeSetting { get; set; }

        public virtual IReactiveCommand CreateInterestSetting { get; set; }

        public virtual IReactiveCommand CreateLateChargeSetting { get; set; }

        public virtual IReactiveCommand CreateMinimumPaymentSetting { get; set; }

        public virtual IReactiveCommand CreateOverlimitFeeSetting { get; set; }

        public virtual IReactiveCommand CreateServiceFeeSetting { get; set; }

        public virtual IReactiveCommand EditComputationSetting { get; set; }

        public virtual IReactiveCommand DeleteComputationSetting { get; set; }

        public virtual IReactiveCommand Save { get; set; }

        public ProviderViewModel()
        {
            _controller = new ProviderController(this);

            // finance charge
            //this.CreateFinnanceChargeSetting = new RelayCommand(_ =>
            //{
            //    var dialog = IoC.Container.Resolve<ComputationSettingDialogService>();
            //    dialog.ViewModel = IoC.Container.Resolve<FinanceChargeSettingViewModel>();
            //    dialog.ViewModel.Currencies = this.Currencies;

            //    var item = dialog.ShowModal(this, "Create" + dialog.ViewModel.Title);
            //    if (item != null)
            //    {
            //        this.ComputationSettings.Insert(0, item);
            //        this.SelectedComputationSetting = item;
            //    }
            //});

            // interest
            //this.CreateInterestSetting = new RelayCommand(_ =>
            //{
            //    var dialog = IoC.Container.Resolve<ComputationSettingDialogService>();
            //    dialog.ViewModel = IoC.Container.Resolve<InterestSettingViewModel>();
            //    dialog.ViewModel.Currencies = this.Currencies;

            //    var item = dialog.ShowModal(this, "Create" + dialog.ViewModel.Title);
            //    if (item != null)
            //    {
            //        this.ComputationSettings.Insert(0, item);
            //        this.SelectedComputationSetting = item;
            //    }
            //});

            // interest
            //this.CreateLateChargeSetting = new RelayCommand(_ =>
            //{
            //    var dialog = IoC.Container.Resolve<ComputationSettingDialogService>();
            //    dialog.ViewModel = IoC.Container.Resolve<LateChargeSettingViewModel>();
            //    dialog.ViewModel.Currencies = this.Currencies;

            //    var item = dialog.ShowModal(this, "Create" + dialog.ViewModel.Title);
            //    if (item != null)
            //    {
            //        this.ComputationSettings.Insert(0, item);
            //        this.SelectedComputationSetting = item;
            //    }

            //});

            // overlimit fee
            //this.CreateOverlimitFeeSetting = new RelayCommand(_ =>
            //{
            //    var dialog = IoC.Container.Resolve<ComputationSettingDialogService>();
            //    dialog.ViewModel = IoC.Container.Resolve<OverlimitFeeSettingViewModel>();
            //    dialog.ViewModel.Currencies = this.Currencies;

            //    var item = dialog.ShowModal(this, "Create" + dialog.ViewModel.Title);
            //    if (item != null)
            //    {
            //        this.ComputationSettings.Insert(0, item);
            //        this.SelectedComputationSetting = item;
            //    }
            //});

            // service fee
            //this.CreateInterestSetting = new RelayCommand(_ =>
            //{
            //    var dialog = IoC.Container.Resolve<ComputationSettingDialogService>();
            //    dialog.ViewModel = IoC.Container.Resolve<ServiceFeeSettingViewModel>();
            //    dialog.ViewModel.Currencies = this.Currencies;

            //    var item = dialog.ShowModal(this, "Create" + dialog.ViewModel.Title);
            //    if (item != null)
            //    {
            //        this.ComputationSettings.Insert(0, item);
            //        this.SelectedComputationSetting = item;
            //    }
            //});

            // edit
            //this.EditComputationSetting = new RelayCommand(param =>
            //{
            //    var item = (ComputationSettingViewModel)param;
            //    var dialog = IoC.Container.Resolve<ComputationSettingDialogService>();
            //    dialog.ViewModel = (ComputationSettingViewModel)IoC.Container.Resolve(param.GetType()); //(ComputationSettingViewModel)Activator.CreateInstance(param.GetType());
            //    dialog.ViewModel.HydrateWith(item);
            //    dialog.ViewModel.Currencies = this.Currencies;

            //    var value = dialog.ShowModal(this, "Edit" + dialog.ViewModel.Title);
            //    if (value != null)
            //    {
            //        item.HydrateWith(value);
            //        this.SelectedComputationSetting = item;
            //    }
            //});

            // delete computation setting
            //this.DeleteComputationSetting = new RelayCommand(param =>
            //{
            //    var item = (ComputationSettingViewModel)param;
            //    var messageBox = IoC.Container.Resolve<IMessageBoxService>();
            //    var message = string.Format("Are you sure you want to delete all item: {0}?", item.MinimumAmountCurrency.Value);
            //    var result = messageBox.ShowQuestion(message);
            //    if (result == MessageBoxResult.OK)
            //    {
            //        this.ComputationSettings.Remove(item);
            //        this.SelectedComputationSetting = null;
            //    }
            //});

            // save
            //this.Save = new RelayCommand(
            //    _ => this.IsValid,
            //    _ =>
            //    {
            //        var messageBox = IoC.Container.Resolve<IMessageBoxService>();
            //        var message = "Are you sure you want to save all changes?";
            //        var result = messageBox.ShowQuestion(message);
            //        if (result == MessageBoxResult.OK)
            //        {
            //            _controller.Save();
            //            this.AcceptChanges();
            //            messageBox.ShowInformation("Changes has been saved?");
            //        }
            //    }
            //);
        }


        #region Methods

        public virtual void Populate1(string providerId)
        {
            _controller.Populate(providerId);
            this.ActivateChangeTracking();
        }

        public virtual void HydrateWith(ProviderViewModel value)
        {
            Id = value.Id;
            Name = value.Name;
            InterestRate = value.InterestRate;
        }

        #endregion
    }
}
