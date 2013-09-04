using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.Data.Commands
{
    public sealed class Invoker : SessionManager
    {
        public void Invoke(Command command)
        {
            command.Session = this.Session;
            command.Transaction = this.Transaction;
            command.Execute();
        }

        public T Invoke<T>(Command<T> command)
        {
            command.Session = this.Session;
            command.Transaction = this.Transaction;
            return command.Execute();
        }
    }
}
