using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features;

namespace CCMS.UI.Features
{
    public class MainDialogService : DialogService<MainView, MainViewModel>
    {
        public MainDialogService(MainView view, MainViewModel viewModel) 
            : base(view, viewModel) { }
    }
}
