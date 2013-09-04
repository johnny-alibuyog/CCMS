using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CCMS.UI.Bootstrappers.IoC;
using CCMS.UI.Common.Extentions;
using CCMS.UI.Features.Currencies;
using NHibernate;
using NHibernate.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.Currencies
{
    public class CurrencyListController : ControllerBase<CurrencyListViewModel>
    {
        public CurrencyListController(CurrencyListViewModel viewModel) : base(viewModel)
        {
            PopulateList();

            this.ViewModel.Create = new ReactiveCommand();
            this.ViewModel.Create.Subscribe(x => Create());

            this.ViewModel.Edit = new ReactiveCommand();
            this.ViewModel.Edit.Subscribe(x => Edit((string)x));

            this.ViewModel.Delete = new ReactiveCommand();
            this.ViewModel.Delete.Subscribe(x => Delete((string)x));

        }

        public virtual void Create()
        {
            try
            {
                var dialog = IoC.Container.Resolve<CurrencyDialogService>();
                dialog.ViewModel.Save = new ReactiveCommand(dialog.ViewModel.IsValidObservable());
                dialog.ViewModel.Save.Subscribe(x => Insert(dialog.ViewModel));
                dialog.ShowModal(this, "Create Currency");
            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }
        }

        public virtual void Insert(CurrencyViewModel item)
        {
            try
            {
                var message = string.Format("Do you want to save currency?");
                var result = this.MessageBox.ShowQuestion(message);
                if (result != MessageBoxResult.OK)
                    return;
                
                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var currency = new Core.Entities.Currency()
                    {
                        Id = item.Id,
                        Name = item.Name
                    };
                    session.Save(currency);
                    transaction.Commit();
                }

                this.MessageBox.ShowInformation("Save has been successfully completed.");

                this.ViewModel.Items.Insert(0, item);
                this.ViewModel.SelectedItem = item;
                this.ViewModel.AcceptChanges();

                item.Close();
            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }
        }

        public virtual void Edit(string id)
        {
            try
            {
                var item = this.ViewModel.Items.Single(x => x.Id == id);
                this.ViewModel.SelectedItem = item;

                var dialog = IoC.Container.Resolve<CurrencyDialogService>();
                dialog.ViewModel.HydrateWith(item);
                dialog.ViewModel.Save = new ReactiveCommand(dialog.ViewModel.IsValidObservable());
                dialog.ViewModel.Save.Subscribe(x => Update(dialog.ViewModel));
                dialog.ShowModal(this, "Edit Currency");
            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }
        }

        public virtual void Update(CurrencyViewModel item)
        {
            try
            {
                var message = string.Format("Do you want to save currency?");
                var result = this.MessageBox.ShowQuestion(message);
                if (result != MessageBoxResult.OK)
                    return;


                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var currency = session.Get<Core.Entities.Currency>(item.Id);
                    currency.Name = item.Name;
                    transaction.Commit();
                }

                this.MessageBox.ShowInformation("Save has been successfully completed.");

                this.ViewModel.SelectedItem.HydrateWith(item);
                this.ViewModel.AcceptChanges();

                item.Close();
            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }
        }

        public virtual void Delete(string id)
        {
            try
            {
                var item = this.ViewModel.Items.Single(x => x.Id == id);
                var message = string.Format("Are you sure you want to delete currency: {0}?", item.Name);
                var result = this.MessageBox.ShowQuestion(message);
                if (result != MessageBoxResult.OK)
                    return;

                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var currency = session.Get<Core.Entities.Currency>(item.Id);
                    session.Delete(currency);
                    transaction.Commit();
                }

                this.MessageBox.ShowInformation("Delete has been successfully completed.");

                this.ViewModel.Items.Remove(item);
                this.ViewModel.SelectedItem = null;
                this.ViewModel.AcceptChanges();
            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }
        }
        
        public virtual void PopulateList()
        {
            try
            {
                using (var session = this.SessionFactory.OpenStatelessSession())
                using (var transaction = session.BeginTransaction())
                {
                    var items = session.Query<Core.Entities.Currency>()
                        .Select(x => new CurrencyViewModel()
                        {
                            Id = x.Id,
                            Name = x.Name
                        })
                        .ToList();

                    this.ViewModel.Items = new ObservableCollection<CurrencyViewModel>(items);

                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                this.MessageBox.ShowError(ex.Message, ex);
            }
        }
    }
}
