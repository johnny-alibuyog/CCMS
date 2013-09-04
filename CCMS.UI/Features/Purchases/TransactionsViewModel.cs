using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CCMS.Data;
using CCMS.UI.Bootstrappers.IoC;
using CCMS.UI.Features;
using CCMS.UI.Features.CreditCards;
using CCMS.UI.Features.Purchases;
using CCMS.UI.Infrastructure;
using NHibernate;
using NHibernate.Linq;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.Purchases
{
    public class TransactionsViewModel : ViewModelBase
    {
        private readonly TransactionsController _cotroller;

        public virtual bool IsItemSelected { get { return SelectedItem != null; } }

        public virtual TransactionItemViewModel SelectedItem { get; set; }

        public virtual ObservableCollection<TransactionItemViewModel> Items { get; set; }

        public virtual IReactiveCommand Create { get; set; }

        public virtual IReactiveCommand Insert { get; set; }

        public virtual IReactiveCommand Edit { get; set; }

        public virtual IReactiveCommand Update { get; set; }

        public virtual IReactiveCommand Delete { get; set; }

        public TransactionsViewModel()
        {
            _cotroller = new TransactionsController(this);
        }
    }
}
