using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features.Navigations;

namespace CCMS.UI.Features.CreditCards
{
    public interface INavigationService
    {
        NavigationViewModel ViewModel { get; set; }
        void Populate(Guid userId);
    }
}
