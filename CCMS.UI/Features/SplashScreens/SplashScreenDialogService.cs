using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features.SplashScreens;

namespace CCMS.UI.Features.SplashScreens
{
    public class SplashScreenDialogService : DialogService<SplashScreenView, SplashScreenViewModel>
    {
        public SplashScreenDialogService(SplashScreenView view, SplashScreenViewModel viewModel)
            : base(view, viewModel) { }
    }
}
