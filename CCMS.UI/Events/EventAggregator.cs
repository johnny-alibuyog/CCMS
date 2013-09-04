using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Text;

namespace CCMS.UI.Events
{
    public class EventAggregator : IEventAggregator
    {
        private readonly ConcurrentDictionary<Type, object> _subjects;

        public EventAggregator()
        {
            _subjects = new ConcurrentDictionary<Type, object>();
        }

        public IObservable<T> GetEvent<T>()
        {
            var subject = (ISubject<T>)_subjects.GetOrAdd(
                typeof(T), x => new Subject<T>());
            return subject.AsObservable();
        }

        public void Publish<T>(T value)
        {
            var subjects = _subjects.Where(x => x.Key.IsAssignableFrom(typeof(T)));
            foreach (var subject in subjects)
            {
                var type = typeof(IObserver<>).MakeGenericType(subject.Key);
                var methodInfo = type.GetMethod("OnNext");
                methodInfo.Invoke(subject.Value, new object[] { value });
            }
        }
    }
}
