using System.Collections.ObjectModel;
using CCMS.UI.Features.Billings;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.Billings
{
    public class BillingListViewModel : ViewModelBase
    {
        private readonly BillingListController _controller;

        public virtual BillingViewModel SelectedItem { get; set; }

        public virtual ObservableCollection<BillingViewModel> Items { get; set; }

        public virtual IReactiveCommand Edit { get; set; }

        public virtual IReactiveCommand Delete { get; set; }

        public BillingListViewModel()
        {
            _controller = new BillingListController(this);
        }
    }
}
