using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CCMS.UI.Features.Contents;
using CCMS.UI.Features.Navigations;
using CCMS.UI.Features.Summaries;

namespace CCMS.UI.Features
{
    public class MainViewModel : ViewModelBase
    {
        #region Properties

        public virtual MenuViewModel Menu { get; private set; }

        public virtual SummaryViewModel Summary { get; private set; }

        public virtual ContentViewModel Content { get; private set; }

        public virtual NavigationViewModel Navigation { get; private set; }

        #endregion

        #region Constructos

        public MainViewModel() { }

        public MainViewModel(MenuViewModel menu, SummaryViewModel summary, ContentViewModel content, NavigationViewModel navigation)
        {
            Menu = menu;
            Summary = summary;
            Content = content;
            Navigation = navigation;
        }

        #endregion
    }
}
