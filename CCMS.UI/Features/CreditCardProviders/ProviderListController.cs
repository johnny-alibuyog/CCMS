using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using CCMS.UI.Bootstrappers.IoC;
using CCMS.UI.Common;
using CCMS.UI.Features.CreditCardProviders;
using NHibernate;
using NHibernate.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.CreditCardProviders
{
    public class ProviderListController : ControllerBase<ProviderListViewModel>
    {
        public ProviderListController(ProviderListViewModel viewModel)
            : base(viewModel)
        {
            this.PopulateList();

            this.ViewModel.Create = new ReactiveCommand();
            this.ViewModel.Create.Subscribe(x => this.Create());

            this.ViewModel.Edit = new ReactiveCommand();
            this.ViewModel.Edit.Subscribe(x => Edit((string)x));

            this.ViewModel.Delete = new ReactiveCommand();
            this.ViewModel.Delete.Subscribe(x => Delete((string)x));
        }

        public virtual void PopulateList()
        {
            try
            {
                using (var session = this.SessionFactory.OpenStatelessSession())
                using (var transaction = session.BeginTransaction())
                {
                    var items = session.Query<CreditCardProvider>()
                        .Select(x => new ProviderViewModel()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            InterestRate = x.InterestRate
                        })
                        .ToList();

                    ViewModel.Items = new ObservableCollection<ProviderViewModel>(items);
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                this.MessageBox.Warn(ex.Message, ex);
            }
        }

        public virtual void Create()
        {
            try
            {
                var dialog = IoC.Container.Resolve<ProviderDialogService>();
                var item = dialog.ShowModal(this, "Create Provider");
                if (item != null && item.HasAppliedChanges)
                {
                    this.ViewModel.Items.Add(item);
                    this.ViewModel.SelectedItem = item;
                    this.ViewModel.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                this.MessageBox.Warn(ex.Message, ex);
            }
        }

        public virtual void Edit(string id)
        {
            try
            {
                var item = this.ViewModel.Items.Single(x => x.Id == id);
                var dialog = IoC.Container.Resolve<ProviderDialogService>();
                //dialog.ViewModel.Populate.Execute(item.Id);
                dialog.ViewModel.Populate1(item.Id);

                var value = dialog.ShowModal(this, "Edit Provider");
                if (value != null && value.HasAppliedChanges)
                {
                    item.HydrateWith(value);

                    this.ViewModel.SelectedItem = item;
                    this.ViewModel.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                this.MessageBox.Warn(ex.Message, ex);
            }
        }

        public virtual void Delete(string id)
        {
            try
            {
                var item = this.ViewModel.Items.Single(x => x.Id == (string)id);
                var message = string.Format("Are you sure you want to delete provider: {0}?", item.Name);
                var result = this.MessageBox.Confirm(message);
                if (result != MessageBoxResult.OK)
                    return;

                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var itemToDelete = session.Get<CreditCardProvider>(item.Id);
                    session.Delete(itemToDelete);
                    transaction.Commit();
                }

                this.ViewModel.Items.Remove(item);
                this.ViewModel.SelectedItem = null;
                this.ViewModel.AcceptChanges();
            }
            catch (Exception ex)
            {
                this.MessageBox.Warn(ex.Message, ex);
            }
        }
    }
}
