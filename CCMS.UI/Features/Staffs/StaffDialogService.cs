using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.UI.Features.Staffs
{
    public class StaffDialogService : DialogService<StaffView, StaffViewModel>
    {
        public StaffDialogService(StaffView view, StaffViewModel viewModel) : base(view, viewModel) { }
    }
}
