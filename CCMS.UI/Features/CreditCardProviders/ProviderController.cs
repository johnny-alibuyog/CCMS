using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using CCMS.UI.Bootstrappers.IoC;
using CCMS.UI.Common;
using CCMS.UI.Common.Extentions;
using CCMS.UI.Features.CreditCardProviders;
using CCMS.UI.Infrastructure;
using NHibernate;
using NHibernate.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.CreditCardProviders
{
    public class ProviderController : ControllerBase<ProviderViewModel>
    {
        #region Routine Helpers

        private void Map(ProviderViewModel target, CreditCardProvider source)
        {
            target.Id = source.Id;
            target.Name = source.Name;
            target.InterestRate = source.InterestRate;
            target.ComputationSettings = new ChangeTrackingObservableCollection<ComputationSettingViewModel>(new List<ComputationSettingViewModel>()
                .Concat(source.ComputationSettings
                .OfType<Core.Entities.FinanceChargeSetting>()
                .Select(x => new FinanceChargeSettingViewModel()
                {
                    Id = x.Id,
                    MinimumAmountCurrency = ViewModel.Currencies.Get(x.MinimumAmount.Currency.Id),
                    MinimumAmountValue = x.MinimumAmount.Value,
                    Rate = x.Rate
                }))
                .Concat(source.ComputationSettings
                .OfType<Core.Entities.InterestSetting>()
                .Select(x => new InterestSettingViewModel()
                {
                    Id = x.Id,
                    MinimumAmountCurrency = ViewModel.Currencies.Get(x.MinimumAmount.Currency.Id),
                    MinimumAmountValue = x.MinimumAmount.Value,
                    Rate = x.Rate
                }))
                .Concat(source.ComputationSettings
                .OfType<Core.Entities.LateChargeSetting>()
                .Select(x => new LateChargeSettingViewModel()
                {
                    Id = x.Id,
                    MinimumAmountCurrency = ViewModel.Currencies.Get(x.MinimumAmount.Currency.Id),
                    MinimumAmountValue = x.MinimumAmount.Value,
                    Rate = x.Rate
                }))
                .Concat(source.ComputationSettings
                .OfType<Core.Entities.MinimumPaymentSetting>()
                .Select(x => new MinimumPaymentSettingViewModel()
                {
                    Id = x.Id,
                    MinimumAmountCurrency = ViewModel.Currencies.Get(x.MinimumAmount.Currency.Id),
                    MinimumAmountValue = x.MinimumAmount.Value,
                    Rate = x.Rate
                }))
                .Concat(source.ComputationSettings
                .OfType<Core.Entities.OverlimitFeeSetting>()
                .Select(x => new OverlimitFeeSettingViewModel()
                {
                    Id = x.Id,
                    MinimumAmountCurrency = ViewModel.Currencies.Get(x.MinimumAmount.Currency.Id),
                    MinimumAmountValue = x.MinimumAmount.Value,
                    Rate = x.Rate
                }))
                .Concat(source.ComputationSettings
                .OfType<Core.Entities.ServiceFeeSetting>()
                .Select(x => new ServiceFeeSettingViewModel()
                {
                    Id = x.Id,
                    MinimumAmountCurrency = ViewModel.Currencies.Get(x.MinimumAmount.Currency.Id),
                    MinimumAmountValue = x.MinimumAmount.Value,
                    Rate = x.Rate
                }))
                .OrderBy(x => x.MinimumAmountCurrency.Key)
                .ThenBy(x => x.Details)
                //.ThenBy(x => x.MinimumAmountValue)
            );

        }

        private void Map(CreditCardProvider target, ProviderViewModel source)
        {
            var session = this.SessionProvider.GetSharedSession();

            target.Name = source.Name;
            target.InterestRate = source.InterestRate;
            target.ComputationSettings = source.ComputationSettings
                .Select(x =>
                {
                    var computationSetting = (ComputationSetting)null;
                    if (x is FinanceChargeSettingViewModel)
                    {
                        // finance charge
                        computationSetting = new FinanceChargeSetting()
                        {
                            Id = x.Id,
                            Provider = target,
                            MinimumAmount = new Money(
                                currency: session.Load<Currency>(x.MinimumAmountCurrency.Key),
                                value: x.MinimumAmountValue
                            ),
                            Rate = x.Rate
                        };
                    }
                    else if (x is InterestSettingViewModel)
                    {
                        // interest
                        computationSetting = new InterestSetting()
                        {
                            Id = x.Id,
                            Provider = target,
                            MinimumAmount = new Money(
                                currency: session.Load<Currency>(x.MinimumAmountCurrency.Key),
                                value: x.MinimumAmountValue
                            ),
                            Rate = x.Rate
                        };
                    }
                    else if (x is LateChargeSettingViewModel)
                    {
                        // late charge
                        computationSetting = new LateChargeSetting()
                        {
                            Id = x.Id,
                            Provider = target,
                            MinimumAmount = new Money(
                                currency: session.Load<Currency>(x.MinimumAmountCurrency.Key),
                                value: x.MinimumAmountValue
                            ),
                            Rate = x.Rate
                        };
                    }
                    else if (x is MinimumPaymentSettingViewModel)
                    {
                        // late charge
                        computationSetting = new MinimumPaymentSetting()
                        {
                            Id = x.Id,
                            Provider = target,
                            MinimumAmount = new Money(
                                currency: session.Load<Currency>(x.MinimumAmountCurrency.Key),
                                value: x.MinimumAmountValue
                            ),
                            Rate = x.Rate
                        };
                    }
                    else if (x is OverlimitFeeSettingViewModel)
                    {
                        // over limit
                        computationSetting = new OverlimitFeeSetting()
                        {
                            Id = x.Id,
                            Provider = target,
                            MinimumAmount = new Money(
                                currency: session.Load<Currency>(x.MinimumAmountCurrency.Key),
                                value: x.MinimumAmountValue
                            ),
                            Rate = x.Rate
                        };
                    }
                    else
                    {
                        // service fee
                        computationSetting = new OverlimitFeeSetting()
                        {
                            Id = x.Id,
                            Provider = target,
                            MinimumAmount = new Money(
                                currency: session.Load<Currency>(x.MinimumAmountCurrency.Key),
                                value: x.MinimumAmountValue
                            ),
                            Rate = x.Rate
                        };
                    }

                    return computationSetting;
                })
                .ToList();
        }

        #endregion

        public ProviderController(ProviderViewModel viewModel)
            : base(viewModel)
        {
            this.ViewModel.Populate = new ReactiveCommand();
            this.ViewModel.Populate.Subscribe(x => this.Populate((string)x));

            this.ViewModel.CreateFinnanceChargeSetting = new ReactiveCommand();
            this.ViewModel.CreateFinnanceChargeSetting.Subscribe(x => this.CreateFinnanceChargeSetting());

            this.ViewModel.CreateInterestSetting = new ReactiveCommand();
            this.ViewModel.CreateInterestSetting.Subscribe(x => this.CreateInterestSetting());

            this.ViewModel.CreateLateChargeSetting = new ReactiveCommand();
            this.ViewModel.CreateLateChargeSetting.Subscribe(x => this.CreateLateChargeSetting());

            this.ViewModel.CreateMinimumPaymentSetting = new ReactiveCommand();
            this.ViewModel.CreateMinimumPaymentSetting.Subscribe(x => this.CreateLateChargeSetting());

            this.ViewModel.CreateOverlimitFeeSetting = new ReactiveCommand();
            this.ViewModel.CreateOverlimitFeeSetting.Subscribe(x => this.CreateOverlimitFeeSetting());

            this.ViewModel.CreateServiceFeeSetting = new ReactiveCommand();
            this.ViewModel.CreateServiceFeeSetting.Subscribe(x => this.CreateServiceFeeSetting());

            this.ViewModel.EditComputationSetting = new ReactiveCommand();
            this.ViewModel.EditComputationSetting.Subscribe(x => this.EditComputationSetting((ComputationSettingViewModel)x));

            this.ViewModel.DeleteComputationSetting = new ReactiveCommand();
            this.ViewModel.DeleteComputationSetting.Subscribe(x => this.DeleteComputationSetting((ComputationSettingViewModel)x));

            this.ViewModel.Save = new ReactiveCommand(this.ViewModel
                .WhenAny(x => x.IsValid, x => x.Value == true));
            this.ViewModel.Save.Subscribe(x => Save());
        }

        public virtual void Populate(string providerId)
        {
            try
            {
                this.ViewModel.IsEditMode = true;

                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<CreditCardProvider>()
                        .Where(x => x.Id == providerId)
                        .FetchMany(x => x.ComputationSettings)
                        .ThenFetch(x => x.MinimumAmount)
                        .ThenFetch(x => x.Currency)
                        .ToFutureValue();

                    var currencies = session.Query<Core.Entities.Currency>().Cacheable().ToList()
                        .Select(x => new KeyValuePair<string, string>(x.Id, x.Name));

                    var provider = query.Value;

                    // populate lookup
                    ViewModel.Currencies = new ObservableCollection<KeyValuePair<string, string>>(currencies);

                    // populate list
                    Map(ViewModel, provider);

                    transaction.Commit();
                }

                this.ViewModel.ActivateChangeTracking();
            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }
        }

        public virtual void CreateFinnanceChargeSetting()
        {
            try
            {
                var dialog = IoC.Container.Resolve<ComputationSettingDialogService>();
                dialog.ViewModel = IoC.Container.Resolve<FinanceChargeSettingViewModel>();
                dialog.ViewModel.Currencies = this.ViewModel.Currencies;
                dialog.ViewModel.Save = new ReactiveCommand(dialog.ViewModel.IsValidObservable());
                dialog.ViewModel.Save.Subscribe(x => InsertComputationSetting(dialog.ViewModel));
                dialog.ShowModal(this, "Create" + dialog.ViewModel.Title);
            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }
        }

        public virtual void CreateInterestSetting()
        {
            try
            {
                var dialog = IoC.Container.Resolve<ComputationSettingDialogService>();
                dialog.ViewModel = IoC.Container.Resolve<InterestSettingViewModel>();
                dialog.ViewModel.Currencies = this.ViewModel.Currencies;
                dialog.ViewModel.Save = new ReactiveCommand(dialog.ViewModel.IsValidObservable());
                dialog.ViewModel.Save.Subscribe(x => InsertComputationSetting(dialog.ViewModel));
                dialog.ShowModal(this, "Create" + dialog.ViewModel.Title);
            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }
        }

        public virtual void CreateLateChargeSetting()
        {
            try
            {
                var dialog = IoC.Container.Resolve<ComputationSettingDialogService>();
                dialog.ViewModel = IoC.Container.Resolve<LateChargeSettingViewModel>();
                dialog.ViewModel.Currencies = this.ViewModel.Currencies;
                dialog.ViewModel.Save = new ReactiveCommand(dialog.ViewModel.IsValidObservable());
                dialog.ViewModel.Save.Subscribe(x => InsertComputationSetting(dialog.ViewModel));
                dialog.ShowModal(this, "Create" + dialog.ViewModel.Title);
            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }
        }

        public virtual void CreateMinimumPaymentSetting()
        {
            try
            {
                var dialog = IoC.Container.Resolve<ComputationSettingDialogService>();
                dialog.ViewModel = IoC.Container.Resolve<MinimumPaymentSettingViewModel>();
                dialog.ViewModel.Currencies = this.ViewModel.Currencies;
                dialog.ViewModel.Save = new ReactiveCommand(dialog.ViewModel.IsValidObservable());
                dialog.ViewModel.Save.Subscribe(x => InsertComputationSetting(dialog.ViewModel));
                dialog.ShowModal(this, "Create" + dialog.ViewModel.Title);
            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }
        }

        public virtual void CreateOverlimitFeeSetting()
        {
            try
            {
                var dialog = IoC.Container.Resolve<ComputationSettingDialogService>();
                dialog.ViewModel = IoC.Container.Resolve<OverlimitFeeSettingViewModel>();
                dialog.ViewModel.Currencies = this.ViewModel.Currencies;
                dialog.ViewModel.Save = new ReactiveCommand(dialog.ViewModel.IsValidObservable());
                dialog.ViewModel.Save.Subscribe(x => InsertComputationSetting(dialog.ViewModel));
                dialog.ShowModal(this, "Create" + dialog.ViewModel.Title);
            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }
        }

        public virtual void CreateServiceFeeSetting()
        {
            try
            {
                var dialog = IoC.Container.Resolve<ComputationSettingDialogService>();
                dialog.ViewModel = IoC.Container.Resolve<ServiceFeeSettingViewModel>();
                dialog.ViewModel.Currencies = this.ViewModel.Currencies;
                dialog.ViewModel.Save = new ReactiveCommand(dialog.ViewModel.IsValidObservable());
                dialog.ViewModel.Save.Subscribe(x => InsertComputationSetting(dialog.ViewModel));
                dialog.ShowModal(this, "Create" + dialog.ViewModel.Title);
            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }
        }

        public virtual void InsertComputationSetting(ComputationSettingViewModel item)
        {
            try
            {
                var message = string.Format("Do you want to save Computation Settings?");
                var result = this.MessageBox.ShowQuestion(message);
                if (result != MessageBoxResult.OK)
                    return;

                this.ViewModel.ComputationSettings.Insert(0, item);
                this.ViewModel.SelectedComputationSetting = item;

                item.Close();
            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }
        }

        public virtual void EditComputationSetting(ComputationSettingViewModel item)
        {
            try
            {
                this.ViewModel.SelectedComputationSetting = item;

                var dialog = IoC.Container.Resolve<ComputationSettingDialogService>();
                dialog.ViewModel = (ComputationSettingViewModel)IoC.Container.Resolve(item.GetType());
                dialog.ViewModel.HydrateWith(item);
                dialog.ViewModel.Currencies = this.ViewModel.Currencies;
                dialog.ViewModel.Save = new ReactiveCommand(dialog.ViewModel.IsValidObservable());
                dialog.ViewModel.Save.Subscribe(x => UpdateComputationSetting(dialog.ViewModel));
                dialog.ShowModal(this, "Edit" + dialog.ViewModel.Title);
            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }
        }

        public virtual void UpdateComputationSetting(ComputationSettingViewModel item)
        {
            try
            {
                var message = string.Format("Do you want to save Computation Settings?");
                var result = this.MessageBox.ShowQuestion(message);
                if (result != MessageBoxResult.OK)
                    return;

                this.ViewModel.SelectedComputationSetting.HydrateWith(item);
                item.Close();
            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }
        }

        public virtual void DeleteComputationSetting(ComputationSettingViewModel item)
        {
            try
            {
                var message = string.Format("Are you sure you want to delete all item: {0}?", item.MinimumAmountCurrency.Value);
                var result = this.MessageBox.ShowQuestion(message);
                if (result != MessageBoxResult.OK)
                    return;

                this.ViewModel.ComputationSettings.Remove(item);
                this.ViewModel.SelectedComputationSetting = null;
            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }
        }

        public virtual void Save()
        {
            try
            {
                var message = "Are you sure you want to save all changes?";
                var result = this.MessageBox.ShowQuestion(message);
                if (result != MessageBoxResult.OK)
                    return;

                using (var session = this.SessionProvider.GetSharedSession())
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<Core.Entities.CreditCardProvider>()
                        .Where(x => x.Id == this.ViewModel.Id)
                        .FetchMany(x => x.ComputationSettings)
                        .ThenFetch(x => x.MinimumAmount)
                        .ThenFetch(x => x.Currency)
                        .ToFutureValue();

                    var provider = query.Value;

                    Map(provider, ViewModel);

                    transaction.Commit();

                    this.SessionProvider.ReleaseSharedSession();
                }

                this.MessageBox.ShowInformation("Changes has been saved?");

                this.ViewModel.AcceptChanges();
            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }

        }
    }
}