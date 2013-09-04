using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features.Contents;

namespace CCMS.UI.Features.CreditCards
{
    public interface ICardInfoService
    {
        CardInfoViewModel ViewModel { get; set; }
        void Populate(Guid creditCardId);
    }
}
