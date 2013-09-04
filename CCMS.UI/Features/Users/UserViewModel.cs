using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.UI.Features.Users
{
    public class UserViewModel : ViewModelBase
    {
        public virtual Guid Id { get; set; }

        public virtual string Username { get; set; }

        public virtual string Fullname { get; set; }
    }
}
