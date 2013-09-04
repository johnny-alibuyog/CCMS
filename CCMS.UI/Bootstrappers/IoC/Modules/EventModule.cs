using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Modules;
using CCMS.UI.Events;

namespace CCMS.UI.Bootstrappers.IoC.Modules
{
    public class EventModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IEventAggregator>()
                .To<EventAggregator>()
                .InSingletonScope();
        }
    }
}
