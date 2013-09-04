using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CCMS.Core.Entities;
using CCMS.UI.Infrastructure;
using CCMS.UI.Features.Purchases;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Validator.Constraints;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.Purchases
{
    public class TransactionItemViewModel : ViewModelBase
    {
        private readonly TransactionItemController _controller;

        public virtual Guid Id { get; set; }

        public virtual DateTime Date { get; set; }

        [Length(Max = 10)]
        public virtual string Details { get; set; }

        [NotNullNotEmpty]
        public virtual string CurrencyId { get; set; }

        [Range(Min = 1)]
        public virtual decimal Amount { get; set; }

        public virtual string AmountDisplay { get { return Amount.ToString("N2") + " " + CurrencyId; } }

        public virtual ObservableCollection<KeyValuePair<string, string>> Currencies { get; set; }

        public virtual IReactiveCommand Save { get; set; }

        public TransactionItemViewModel()
        {
            this.Date = DateTime.Now;
            this.Details = string.Empty;
            this.CurrencyId = Core.Entities.Currency.PHP.Id;
            this.Amount = 0M;

            _controller = new TransactionItemController(this);
        }
    }
}
