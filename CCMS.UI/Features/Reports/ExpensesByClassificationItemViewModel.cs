using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.UI.Features.Reports
{
    public class ExpensesByClassificationItemViewModel
    {
        public virtual string AccountNumber { get; set; }

        public virtual string Provider { get; set; }

        public virtual DateTime StatementDate { get; set; }

        public virtual decimal Medecine { get; set; }

        public virtual decimal Construction { get; set; }

        public virtual decimal Travel { get; set; }

        public virtual decimal FurnitureAndFixture { get; set; }

        public virtual decimal Equipment { get; set; }

        public virtual decimal Supplies { get; set; }

        public virtual decimal Others { get; set; }

        public virtual decimal TotalExpenses { get; set; }

        public virtual decimal TotalPayments { get; set; }
    }
}
