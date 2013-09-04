using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.UI.Features.Reports
{
    public class IncommingStatementSummaryItemViewModel : ViewModelBase
    {
        #region Properties

        public virtual string Provider { get; set; }

        public virtual string Currency { get; set; }

        public virtual string AccountName { get; set; }

        public virtual string AccountNumber { get; set; }

        public virtual DateTime ExpectedDueDate { get; set; }

        public virtual decimal ExpectedAmountDue { get; set; } 

        #endregion
    }
}
