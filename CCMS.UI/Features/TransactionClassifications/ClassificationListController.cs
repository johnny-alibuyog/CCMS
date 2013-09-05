using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using CCMS.UI.Bootstrappers.IoC;
using NHibernate;
using NHibernate.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.TransactionClassifications
{
    public class ClassificationListController : ControllerBase<ClassificationListViewModel>
    {
        public ClassificationListController(ClassificationListViewModel viewModel) : base(viewModel)
        {
            this.ViewModel.ObservableForProperty(x => x.NewItem).Subscribe(x =>
            {
                var matchedItem = this.ViewModel.Items
                    .Where(o => o.Name == this.ViewModel.NewItem)
                    .FirstOrDefault();

                if (matchedItem == null)
                {
                    matchedItem = this.ViewModel.Items
                        .Where(o => o.Name.Contains(this.ViewModel.NewItem))
                        .FirstOrDefault();
                }

                this.ViewModel.SelectedItem = matchedItem;
            });

            this.ViewModel.Insert = new ReactiveCommand(this.ViewModel
                .WhenAny(
                    x => x.NewItem,
                    x =>
                        !string.IsNullOrWhiteSpace(x.Value) &&
                        !this.ViewModel.Items.Any(o => o.Name == x.Value)
                )
            );
            this.ViewModel.Insert.Subscribe(x => Insert());

            this.ViewModel.Delete = new ReactiveCommand();
            this.ViewModel.Delete.Subscribe(x => Delete((ListItemViewModel)x));

            this.ViewModel.Save = new ReactiveCommand();
            this.ViewModel.Save.Subscribe(x => Save());

            this.Populate();
        }

        public virtual void Populate()
        {
            try
            {
                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<TransactionClassification>()
                        .Select(x => new ListItemViewModel()
                        {
                            Id = x.Id,
                            Name = x.Name
                        })
                        .ToFuture();

                    this.ViewModel.Items = new ReactiveList<ListItemViewModel>(query.ToList());

                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                this.MessageBox.Warn(ex.Message, ex);
            }
        }

        public virtual void Insert()
        {
            try
            {
                var exists = this.ViewModel.Items.Any(x => String.Equals(x.Name, this.ViewModel.NewItem, StringComparison.InvariantCultureIgnoreCase));
                if (exists)
                {
                    var message = string.Format("Transaction classification {0} already exists.", this.ViewModel.NewItem);
                    this.MessageBox.Inform(message);
                    return;
                }

                var item = new ListItemViewModel() { Name = this.ViewModel.NewItem };
                this.ViewModel.Items.Insert(0, item);
                this.ViewModel.SelectedItem = item;
                this.ViewModel.NewItem = string.Empty;
            }
            catch (Exception ex)
            {
                this.MessageBox.Warn(ex.Message, ex);
            }
        }

        public virtual void Delete(ListItemViewModel item)
        {
            this.ViewModel.Items.Remove(item);
            this.ViewModel.SelectedItem = null;
        }

        public virtual void Save()
        {
            try
            {
                var result = this.MessageBox.Confirm("Do you want to save changes?", "Transaction Classification");
                if (result != MessageBoxResult.OK)
                    return;

                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var itemsFromUI = this.ViewModel.Items.Select(x => new TransactionClassification() { Name = x.Name }).ToList();
                    var itemsFromDb = session.Query<TransactionClassification>().ToList();

                    var itemsToInsert = itemsFromUI.Except(itemsFromDb).ToList();
                    var itemsToRemove = itemsFromDb.Except(itemsFromUI).ToList();

                    // insert
                    foreach (var item in itemsToInsert)
                        session.Save(item);

                    // delete
                    foreach (var item in itemsToRemove)
                        session.Delete(item);

                    transaction.Commit();
                }

                this.MessageBox.Inform("Changes has been saved.", "Transaction Classification");
                this.ViewModel.Close();
            }
            catch (Exception ex)
            {
                this.MessageBox.Warn(ex.Message, ex);
            }
        }
    }
}
