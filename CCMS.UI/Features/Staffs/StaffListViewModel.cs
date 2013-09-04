using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.Staffs
{
    public class StaffListViewModel : ViewModelBase
    {
        private readonly StaffListController _controller;

        public virtual StaffViewModel SelectedItem { get; set; }

        public virtual ObservableCollection<StaffViewModel> Items { get; set; }

        public virtual IReactiveCommand Create { get; set; }

        public virtual IReactiveCommand Edit { get; set; }

        public virtual IReactiveCommand Delete { get; set; }

        public virtual IReactiveCommand Save { get; set; }

        public StaffListViewModel()
        {
            _controller = new StaffListController(this);
        }
    }
}
