using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.Core.Commands
{
    public interface ICommand
    {
        void Execute();
        void Commit();
        void Rollback();
    }

    public interface ICommand<T>
    {
        T Execute();
        void Commit();
        void Rollback();
    }
}
