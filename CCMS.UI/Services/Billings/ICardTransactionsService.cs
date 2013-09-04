using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features.Contents;

namespace CCMS.UI.Services.Billings
{
    public interface ICardTransactionsService
    {
        CardTransactionsViewModel ViewModel { get; set; }
        void Populate(Guid creditCardId);
   }
}
