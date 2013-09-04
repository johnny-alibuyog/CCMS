using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.UI.Features.Staffs
{
    public class StaffListDialogService : DialogService<StaffListView, StaffListViewModel>
    {
        public StaffListDialogService(StaffListView view, StaffListViewModel viewModel) : base(view, viewModel) { }
    }
}
