using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.UI.Features.TransactionClassifications
{
    public class ClassificationListDialogService :  DialogService<ListView, ClassificationListViewModel>
    {
        public ClassificationListDialogService(ListView view, ClassificationListViewModel viewModel) : base(view, viewModel) { }
    }
}
