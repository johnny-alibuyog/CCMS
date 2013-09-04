using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.UI.Features.SplashScreens
{
    public class SplashScreenMessage
    {
        public virtual string Message { get; private set; }

        public SplashScreenMessage(string message)
        {
            this.Message = message;
        }
    }
}
