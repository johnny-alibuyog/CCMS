using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features.CashAdvances;

namespace CCMS.UI.Features.CashAdvances
{
    public class CashAdvanceDialogService : DialogService<CashAdvanceView, CashAdvanceViewModel>
    {
        public CashAdvanceDialogService(CashAdvanceView view, CashAdvanceViewModel viewModel)
            : base(view, viewModel) { }
    }
}
