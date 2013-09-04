using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CCMS.UI.Features;
using CCMS.UI.Features.CreditCards;
using CCMS.UI.Features.Billings;
using CCMS.UI.Features.Purchases;

namespace CCMS.UI.Features.Contents
{
    public class CardTransactionsViewModel : ViewModelBase
    {
        private readonly CardTransactionsController _controller;

        public virtual string AmountHeader { get; set; }

        public virtual ObservableCollection<CardTransactionsItemViewModel> Items { get; set; }

        public CardTransactionsViewModel()
        {
            _controller = new CardTransactionsController(this);
        }
    }
}