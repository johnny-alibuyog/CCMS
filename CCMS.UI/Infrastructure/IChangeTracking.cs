using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.UI.Infrastructure
{
    public interface IChangeTracking
    {
        bool IsDirty { get; }
        bool HasAppliedChanges { get; }
        void AcceptChanges();
    }
}
