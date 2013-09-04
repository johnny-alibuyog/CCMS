using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using CCMS.UI.Bootstrappers.IoC;
using CCMS.UI.Features.CreditCards;
using CCMS.UI.Features.Users;
using NHibernate;
using NHibernate.Transform;

namespace CCMS.UI.Features
{
    public class AppViewModel : ViewModelBase
    {
        private readonly AppController _controller;

        private UserViewModel _currentUser;
        private CreditCardInfoViewModel _selectedCreditcard;

        public virtual UserViewModel CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; }
        }

        public virtual CreditCardInfoViewModel SelectedCreditCard
        {
            get { return _selectedCreditcard; }
            set
            {
                if (_selectedCreditcard != value)
                {
                    _selectedCreditcard = value;
                    this.MessageBus.SendMessage(new CreditCardSelectedMessage(_selectedCreditcard));
                }
            }
        }

        public AppViewModel()
        {
            _controller = new AppController(this);
        }
    }
}
