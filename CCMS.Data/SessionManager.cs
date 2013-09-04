using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Common.Exceptions;
using CCMS.Data.Common.Exceptions;
using NHibernate;

namespace CCMS.Data
{
    public class SessionManager
    {
        #region Fields

        private ISessionFactory _sessionFactory;
        private ISession _session;
        private ITransaction _transaction;
        private IExceptionBuilder<BusinessException> _exceptionBuilder;
        private bool _isUnitOfWorkOwner;

        #endregion

        #region Routine Helpers

        private void InitializeSession()
        {
            _session = _sessionFactory.OpenSession();
            _transaction = _session.BeginTransaction();
            _isUnitOfWorkOwner = true;
        }

        protected virtual IExceptionBuilder<BusinessException> ExceptionBuilder
        {
            get { return _exceptionBuilder; }
        }

        protected virtual bool IsUnitOfWorkOwner
        {
            get
            {
                return this._isUnitOfWorkOwner &&
                    this._session != null &&
                    this._transaction != null &&
                    this._transaction.IsActive;
            }
        }

        #endregion

        #region Field Accessors

        public virtual ISessionFactory SessionFactory
        {
            get { return _sessionFactory; }
        }

        public virtual ISession Session
        {
            get
            {
                if (_session == null)
                    this.InitializeSession();

                return _session;
            }
            set
            {
                if (_session == null)
                {
                    _session = value;
                    _isUnitOfWorkOwner = false;
                }
            }
        }

        public virtual ITransaction Transaction
        {
            get
            {
                if (_transaction == null)
                    this.InitializeSession();

                return _transaction;
            }
            set
            {
                if (_transaction == null)
                {
                    _transaction = value;
                    _isUnitOfWorkOwner = false;
                }
            }
        }

        public virtual void Commit()
        {
            if (this.IsUnitOfWorkOwner)
                _transaction.Commit();
        }

        public virtual void Rollback()
        {
            if (this.IsUnitOfWorkOwner)
                _transaction.Rollback();
        }

        #endregion

        #region Constructors

        public SessionManager()
        {
            _sessionFactory = SessionProvider.SessionFactory;
            _exceptionBuilder = new BusinessExceptionBuilder();
        }

        #endregion

        #region IDisposable Members

        private bool _isDisposed;

        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // check to see if Dispose has already been called
            if (!this._isDisposed)
            {
                if (disposing)
                {
                    if (this.IsUnitOfWorkOwner)
                    {
                        try
                        {
                            this._transaction.Commit();
                        }
                        catch
                        {
                            this._transaction.Rollback();
                            throw;
                        }
                        finally
                        {
                            this._transaction.Dispose();
                            this._transaction = null;

                            this._session.Dispose();
                            this._session = null;
                        }
                    }
                }

                // dispose unmanaged resources here

                // note disposing has been done
                this._isDisposed = true;
            }
        }

        ~SessionManager()
        {
            // do not re-create Dispose clean-up code here.
            // calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            this.Dispose(disposing: false);
        }

        #endregion
    }
}
