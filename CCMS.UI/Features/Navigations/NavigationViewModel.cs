using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using CCMS.UI.Features.CreditCards;
using CCMS.UI.Infrastructure;

namespace CCMS.UI.Features.Navigations
{
    public class NavigationViewModel : ViewModelBase
    {
        private readonly NavigationController _controller;

        public virtual ObservableCollection<NavigationItemViewModel> Items { get; set; }

        public NavigationViewModel()
        {
            _controller = new NavigationController(this);
        }
    }
}
