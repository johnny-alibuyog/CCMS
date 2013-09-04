using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using CCMS.UI.Features.Purchases;

namespace CCMS.UI.Features.Purchases
{
    public class PurchaseAddedMessage
    {
        public virtual Purchase Purchase { get; private set; }

        public PurchaseAddedMessage(Purchase purchase)
        {
            this.Purchase = purchase;
        }
    }
}
