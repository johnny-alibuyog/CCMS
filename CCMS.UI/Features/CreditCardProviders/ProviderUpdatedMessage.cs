using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features.CreditCardProviders;

namespace CCMS.UI.Features.CreditCardProviders
{
    public class ProviderUpdatedMessage
    {
        public virtual ProviderViewModel Provider { get; set; }
    }
}
