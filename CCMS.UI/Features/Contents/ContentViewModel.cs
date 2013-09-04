using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using ReactiveUI;
using NHibernate;

namespace CCMS.UI.Features.Contents
{
    public class ContentViewModel : ViewModelBase
    {
        public virtual CardInfoViewModel CardInfo { get;set; }

        public virtual CardTransactionsViewModel CardTransactions { get; set; }

        public ContentViewModel() { }

        public ContentViewModel(CardInfoViewModel cardInfo, CardTransactionsViewModel cardTransactions)
        {
            CardInfo = cardInfo;
            CardTransactions = cardTransactions;
        }
    }
}
