using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Event;
using CCMS.Data.Common.Exceptions;

namespace CCMS.Data.Configurations
{
    public class ValidationEventListener : IPreInsertEventListener, IPreUpdateEventListener
    {
        private void PerformValidation(object entity)
        {
            var validator = SessionProvider.Instance.ValidatorEngine;
            var invalidValues = validator.Validate(entity);
            if (invalidValues.Count() > 0)
                throw new BusinessExceptionBuilder().Build(invalidValues);
        }

        public bool OnPreInsert(PreInsertEvent @event)
        {
            PerformValidation(@event.Entity);
            return false;
        }

        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            PerformValidation(@event.Entity);
            return false;
        }
    }
}
