using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features.CashAdvances;

namespace CCMS.UI.Features.CashAdvances
{
    public class CashAdvanceListDialogService : DialogService<CashAdvanceListView, CashAdvanceListViewModel>
    {
        public CashAdvanceListDialogService(CashAdvanceListView view, CashAdvanceListViewModel viewModel)
            : base(view, viewModel) { }
    }
}
