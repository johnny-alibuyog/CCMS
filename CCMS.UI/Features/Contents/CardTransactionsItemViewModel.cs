using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.UI.Features.Contents
{
    public class CardTransactionsItemViewModel : ViewModelBase
    {
        public virtual Nullable<DateTime> Date { get; set; }

        public virtual string Details { get; set; }

        public virtual string TransactionClassification { get; set; }

        public virtual string Staff { get; set; }

        public virtual Nullable<decimal> Amount { get; set; } 
    }
}
