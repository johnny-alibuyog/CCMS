using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.UI.Events
{
    public interface IEventAggregator
    {
        void Publish<TEvent>(TEvent value);
        IObservable<TEvent> GetEvent<TEvent>();
    }
}
