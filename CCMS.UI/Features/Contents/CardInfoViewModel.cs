using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CCMS.Core.Entities;
using CCMS.UI.Features;
using CCMS.UI.Features.CreditCards;

namespace CCMS.UI.Features.Contents
{
    public class CardInfoViewModel : ViewModelBase
    {
        private readonly CardInfoController _controller;

        public virtual string AccountName { get; set; }

        public virtual string AccountNumber { get; set; }

        public virtual string Provider { get; set; }

        public virtual Nullable<DateTime> IssueDate { get; set; }

        public virtual Nullable<DateTime> ExpiryDate { get; set; }

        public virtual DateTime NextCutOffDate { get; set; }

        public virtual decimal InterestRate { get; set; }

        public virtual string CreditLimit { get; set; }

        public virtual string CashAdvanceLimit { get; set; }

        public virtual string OutstandingBalance { get; set; }

        public virtual string AvailableCredit { get; set; }

        public virtual int CutOff { get; set; }

        public CardInfoViewModel()
        {
            _controller = new CardInfoController(this);
        }
    }
}
