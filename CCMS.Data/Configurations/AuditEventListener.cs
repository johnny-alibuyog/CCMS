using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Event;
using CCMS.Core.Entities;
using NHibernate.Persister.Entity;

namespace CCMS.Data.Configurations
{
    internal class AuditEventListener : IPreInsertEventListener, IPreUpdateEventListener, IPreDeleteEventListener
    {
        #region Routine Helpers

        private void Set(IEntityPersister persister, object[] state, string propertyName, object value)
        {
            var index = Array.IndexOf(persister.PropertyNames, propertyName);
            if (index == -1)
                return;

            state[index] = value;
        }

        private AuditResolver GetAuditProvider(AbstractPreDatabaseOperationEvent @event)
        {
            var isAuditable = @event.Entity
                .GetType()
                .GetProperties()
                .Any(x => x.PropertyType == typeof(Audit));

            if (!isAuditable)
                return null;

            var auditInfo = @event.Entity
                .GetType()
                .GetProperties()
                .Where(x => x.PropertyType == typeof(Audit))
                .Select(x => new
                {
                    PropertyInfo = x,
                    Value = x.GetValue(@event.Entity, null) as Audit
                })
                .FirstOrDefault();

            var auditProvider = SessionProvider.Instance.AuditResolver;
            if (auditProvider == null)
                auditProvider = new AuditResolver();

            auditProvider.PropertyInfo = auditInfo.PropertyInfo;
            auditProvider.CurrentAudit = auditInfo.Value;

            return auditProvider;
        }

        #endregion

        public bool OnPreInsert(PreInsertEvent @event)
        {
            var auditProvider = GetAuditProvider(@event);
            if (auditProvider == null)
                return false;

            var newAudit = auditProvider.CreateNew();

            Set(@event.Persister, @event.State, auditProvider.PropertyName, newAudit);

            auditProvider.PropertyInfo.SetValue(@event.Entity, newAudit, null);

            return false;
        }

        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            var auditProvider = GetAuditProvider(@event);
            if (auditProvider == null)
                return false;

            var updatedAudit = auditProvider.CreateUpdate();

            Set(@event.Persister, @event.State, auditProvider.PropertyName, updatedAudit);

            auditProvider.PropertyInfo.SetValue(@event.Entity, updatedAudit, null);

            return false;
        }

        public bool OnPreDelete(PreDeleteEvent eventObject)
        {
            throw new NotImplementedException();
        }
    }
}
