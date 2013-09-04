using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features.Users;

namespace CCMS.UI.Features.Users
{
    public interface ILoginService
    {
        LoginViewModel ViewModel { get; set; }

        Nullable<bool> Login();
    }
}
