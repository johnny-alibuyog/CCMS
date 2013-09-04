using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.UI.Features.Billings
{
    public class BillingModifiedMessage
    {
        public virtual IList<Guid> BillingIds { get; set; }

        public BillingModifiedMessage(Guid billingId)
        {
            BillingIds = new List<Guid>() { billingId };
        }

        public BillingModifiedMessage(IList<Guid> billingIds)
        {
            BillingIds = new List<Guid>(billingIds);
        }

    }
}
