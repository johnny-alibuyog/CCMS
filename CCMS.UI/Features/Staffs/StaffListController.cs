using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using CCMS.UI.Bootstrappers.IoC;
using CCMS.UI.Common.Extentions;
using CCMS.UI.Features.Users;
using NHibernate;
using NHibernate.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.Staffs
{
    public class StaffListController : ControllerBase<StaffListViewModel>
    {
        public StaffListController(StaffListViewModel viewModel) : base(viewModel)
        {
            PopulateList();

            this.ViewModel.Create = new ReactiveCommand();
            this.ViewModel.Create.Subscribe(x => Create());

            this.ViewModel.Edit = new ReactiveCommand();
            this.ViewModel.Edit.Subscribe(x => Edit((Guid)x));

            this.ViewModel.Delete = new ReactiveCommand();
            this.ViewModel.Delete.Subscribe(x => Delete((Guid)x));

            //this.ViewModel.Save = new ReactiveCommand();
            //this.ViewModel.Save.Subscribe(x => Save());
        }

        public virtual void Create()
        {
            try
            {
                var dialog = IoC.Container.Resolve<StaffDialogService>();
                dialog.ViewModel.Save = new ReactiveCommand(dialog.ViewModel.IsValidObservable());
                dialog.ViewModel.Save.Subscribe(x => Insert(dialog.ViewModel));
                dialog.ShowModal(this, "Create Credit Card");
            }
            catch (Exception ex)
            {
                this.MessageBox.Warn(ex.Message, ex);
            }
        }

        public virtual void Insert(StaffViewModel item)
        {
            try
            {
                var message = string.Format("Do you want to save Staff?");
                var result = this.MessageBox.Confirm(message);
                if (result != MessageBoxResult.OK)
                    return;

                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var staff = new Staff()
                    {
                        Person = new Person(
                            firstName: item.Person.FirstName,
                            middleName: item.Person.MiddleName,
                            lastName: item.Person.LastName,
                            birthDate: item.Person.BirthDate
                        ),
                        User = session.Load<User>(App.Data.CurrentUser.Id)
                    };

                    session.Save(staff);

                    transaction.Commit();

                    item.Id = staff.Id;
                }

                this.MessageBox.Inform("Save has been successfully completed.");

                this.ViewModel.Items.Insert(0, item);
                this.ViewModel.SelectedItem = item;
                this.ViewModel.AcceptChanges();

                item.Close();
            }
            catch (Exception ex)
            {
                this.MessageBox.Warn(ex.Message, ex);
            }
        }

        public virtual void Edit(Guid id)
        {
            try
            {
                var item = this.ViewModel.Items.Single(x => x.Id == id);
                this.ViewModel.SelectedItem = item;

                var dialog = IoC.Container.Resolve<StaffDialogService>();
                dialog.ViewModel.HydrateWith(item);
                dialog.ViewModel.Save = new ReactiveCommand(dialog.ViewModel.IsValidObservable());
                dialog.ViewModel.Save.Subscribe(x => Update(dialog.ViewModel));
                dialog.ShowModal(this, "Edit Credit Card");
            }
            catch (Exception ex)
            {
                this.MessageBox.Warn(ex.Message, ex);
            }
        }

        public virtual void Update(StaffViewModel item)
        {
            try
            {
                var message = string.Format("Do you want to save Staff?");
                var result = this.MessageBox.Confirm(message);
                if (result != MessageBoxResult.OK)
                    return;

                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var staff = session.Get<Staff>(item.Id);
                    staff.Person = new Person(
                        firstName: item.Person.FirstName,
                        middleName: item.Person.MiddleName,
                        lastName: item.Person.LastName,
                        birthDate: item.Person.BirthDate
                    );
                    staff.User = session.Load<User>(App.Data.CurrentUser.Id);

                    transaction.Commit();
                }

                this.MessageBox.Inform("Save has been successfully completed.");

                this.ViewModel.SelectedItem.HydrateWith(item);
                this.ViewModel.AcceptChanges();

                item.Close();
            }
            catch (Exception ex)
            {
                this.MessageBox.Warn(ex.Message, ex);
            }
        }

        public virtual void Delete(Guid id)
        {
            try
            {
                var item = this.ViewModel.Items.Single(x => x.Id == id);
                var message = string.Format("Are you sure you want to delete staff: {0}?", item.Person.FullName);
                var result = this.MessageBox.Confirm(message);
                if (result != MessageBoxResult.OK)
                    return;

                using (var session = this.SessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var staff = session.Get<Staff>(item.Id);
                    session.Delete(staff);
                    transaction.Commit();
                }

                this.MessageBox.Inform("Delete has been successfully completed.");

                this.ViewModel.Items.Remove(item);
                this.ViewModel.SelectedItem = null;
                this.ViewModel.AcceptChanges();
            }
            catch (Exception ex)
            {
                this.MessageBox.Warn(ex.Message, ex);
            }
        }

        //public virtual void Save()
        //{
        //    try
        //    {
        //        var result = _messageBox.ShowQuestion("Do you want to save changes?");
        //        if (result != MessageBoxResult.OK)
        //            return;

        //        using (var session = this.SessionFactory.OpenSession())
        //        using (var transaction = session.BeginTransaction())
        //        {
        //            var itemsFromDb = session.Query<Staff>().ToList();
        //            var itemsFromUI = this.ViewModel.Items
        //                .Select(x => new Staff()
        //                {
        //                    Person = new Person(
        //                        firstName: x.Person.FirstName,
        //                        middleName: x.Person.MiddleName,
        //                        lastName: x.Person.LastName,
        //                        birthDate: x.Person.BirthDate
        //                    )
        //                })
        //                .ToList();

        //            var itemsToInsert = itemsFromUI.Except(itemsFromDb).ToList();
        //            var itemsToUpdate = itemsFromDb.Where(x => itemsFromUI.Contains(x)).ToList();
        //            var itemsToRemove = itemsFromDb.Except(itemsFromUI).ToList();

        //            // insert
        //            foreach (var item in itemsToInsert)
        //            {
        //                session.Save(item);
        //            }

        //            // update
        //            foreach (var item in itemsToUpdate)
        //            {
        //                var value = itemsFromUI.FirstOrDefault(x => x == item);
        //                item.SerializeWith(value);
        //            }

        //            // delete
        //            foreach (var item in itemsToRemove)
        //            {
        //                session.Delete(item);
        //            }

        //            transaction.Commit();
        //        }

        //        _messageBox.ShowQuestion("Changes successfully saved.");
        //        this.ViewModel.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        _messageBox.ShowError(ex.Message, "Error");
        //    }
        //}

        public virtual void PopulateList()
        {
            try
            {
                using (var session = this.SessionFactory.OpenStatelessSession())
                using (var transaction = session.BeginTransaction())
                {
                    var items = session.Query<Staff>()
                        .Select(x => new StaffViewModel()
                        {
                            Id = x.Id,
                            Person = new PersonViewModel()
                            {
                                FirstName = x.Person.FirstName,
                                MiddleName = x.Person.MiddleName,
                                LastName = x.Person.LastName,
                                BirthDate = x.Person.BirthDate,
                            }
                        })
                        .ToList();

                    ViewModel.Items = new ObservableCollection<StaffViewModel>(items);

                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                this.MessageBox.Warn(ex.Message, ex);
            }
        }
    }
}
