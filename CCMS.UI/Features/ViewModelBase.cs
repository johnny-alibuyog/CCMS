using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CCMS.UI.Bootstrappers.IoC;
using NHibernate;
using NHibernate.Validator.Engine;
using ReactiveUI;

namespace CCMS.UI.Features
{
    public class ViewModelBase : ReactiveObject, IDataErrorInfo, UI.Infrastructure.IChangeTracking //INotifyPropertyChanging, INotifyPropertyChanged, IDataErrorInfo //ReactiveValidatedObject
    {
        private IMessageBus _messageBus;
        private ValidatorEngine _validator;
        private Nullable<bool> _actionResult;

        protected virtual IMessageBus MessageBus
        {
            get
            {
                if (_messageBus == null)
                    _messageBus = IoC.Container.Resolve<IMessageBus>();

                return _messageBus;
            }
        }

        protected virtual ValidatorEngine Validator
        {
            get
            {
                if (_validator == null)
                    _validator = IoC.Container.Resolve<ValidatorEngine>();

                return _validator;
            }
        }

        public virtual Nullable<bool> ActionResult
        {
            get { return _actionResult; }
            set { _actionResult = value; }
        }

        public virtual void Initialize() { }

        #region Close Dialog Overload

        public virtual void Close()
        {
            this.Close(true);
        }

        public virtual void Close(bool result)
        {
            this.ActionResult = result;
        }

        #endregion

        #region IDataErrorInfo Members

        public virtual bool IsValid { get; set; }

        public virtual string Error
        {
            get
            {
                var invalidValues = this.Validator.Validate(this);
                this.IsValid = (invalidValues.Count() == 0);
                return string.Join(Environment.NewLine, invalidValues.Select(x => x.Message));
            }
        }

        public virtual string this[string columnName]
        {
            get
            {
                if (columnName == string.Empty)
                    return null;

                var invalidValues = this.Validator.Validate(this);
                this.IsValid = (invalidValues.Count() == 0);

                var invalidValue = invalidValues.FirstOrDefault(x => x.PropertyName == columnName);
                if (invalidValue != null)
                    return invalidValue.Message;
                else
                    return null;

                //if (columnName == string.Empty)
                //    return null;

                //var invalidValue = this.Validator
                //    .ValidatePropertyValue(this, columnName)
                //    .FirstOrDefault();

                //if (invalidValue != null)
                //    return invalidValue.Message;
                //else
                //    return null;
            }
        }

        public virtual void Revalidate()
        {
            var invalidValues = this.Validator.Validate(this);
            this.IsValid = (invalidValues.Count() == 0);
        }

        #endregion

        #region IChangeTracking Members

        public virtual bool IsDirty { get; private set; }

        public bool HasAppliedChanges { get; private set; }

        public virtual void AcceptChanges()
        {
            this.IsDirty = false;
            this.HasAppliedChanges = true;
        }

        public void ActivateChangeTracking()
        {
            var excludedProperties = new[] { "IsDirty", "SelectedItem" };

            this.Changed.Subscribe(x =>
            {
                if (excludedProperties.Contains(x.PropertyName))
                    return;

                this.IsDirty = true;
            });
        }

        #endregion
    }
}
