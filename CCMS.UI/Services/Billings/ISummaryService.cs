using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features.Summaries;

namespace CCMS.UI.Features.Billings
{
    public interface ISummaryService
    {
        SummaryViewModel ViewModel { get; set; }
        void Populate(Guid creditCardId);
    }
}
