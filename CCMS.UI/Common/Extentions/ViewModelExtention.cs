using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features;
using ReactiveUI;

namespace CCMS.UI.Common.Extentions
{
    public static class ViewModelExtention
    {
        public static IObservable<bool> IsValidObservable(this ViewModelBase viewModel)
        {
            return viewModel.WhenAny(x => x.IsValid, x => x.Value == true);
        }
    }
}
