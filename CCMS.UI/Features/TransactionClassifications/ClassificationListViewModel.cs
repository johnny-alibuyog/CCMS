using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.TransactionClassifications
{
    public class ClassificationListViewModel : ViewModelBase
    {
        private readonly ClassificationListController _controller;

        public virtual string NewItem { get; set; }

        public virtual ListItemViewModel SelectedItem { get; set; }

        public virtual ReactiveList<ListItemViewModel> Items { get; set; }

        public virtual IReactiveCommand Insert { get; set; }

        public virtual IReactiveCommand Delete { get; set; }

        public virtual IReactiveCommand Save { get; set; }

        public ClassificationListViewModel()
        {
            _controller = new ClassificationListController(this);
        }
    }
}
