using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Modules;
using ReactiveUI;

namespace CCMS.UI.Bootstrappers.IoC.Modules
{
    public class MessageModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IMessageBus>()
                .To<MessageBus>()
                .InSingletonScope();
        }
    }
}
