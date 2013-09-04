using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features.SplashScreens;

namespace CCMS.UI.Features.SplashScreens
{
    public class SplashScreenViewModel : ViewModelBase
    {
        #region Properties

        public virtual string Licensee { get; set; }

        public virtual IEnumerable<string> Plugins { get; set; }

        public virtual string Message { get; set; } 

        #endregion

        #region Constructors

        public SplashScreenViewModel()
        {
            this.MessageBus.Listen<SplashScreenMessage>()
                .Subscribe(param => this.Message = param.Message);
        } 

        #endregion
    }
}