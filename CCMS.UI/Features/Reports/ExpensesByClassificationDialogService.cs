using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.UI.Features.Reports
{
    public class ExpensesByClassificationDialogService: DialogService<ExpensesByClassificationView, ExpensesByClassificationViewModel>
    {
        public ExpensesByClassificationDialogService(ExpensesByClassificationView view, ExpensesByClassificationViewModel viewModel)
            : base(view, viewModel) { }
    }
}
