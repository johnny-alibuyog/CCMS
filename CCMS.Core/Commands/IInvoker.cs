using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.Core.Commands
{
    public interface IInvoker
    {
        //ICommand Command { get; set; }
        //void Invoke();
        void Invoke(ICommand command);
        T Invoke<T>(ICommand<T> command);
    }
}
