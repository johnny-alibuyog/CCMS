using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features.Users;

namespace CCMS.UI.Features.Users
{
    public class AuthenticationDialogService : DialogService<AuthenticationView, AuthenticationViewModel>
    {
        public AuthenticationDialogService(AuthenticationView view, AuthenticationViewModel viewModel)
            : base(view, viewModel) { }
    }
}
