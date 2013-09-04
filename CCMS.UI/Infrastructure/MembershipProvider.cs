using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Domain.Models;

namespace CCMS.UI.Infrastructure
{
    public class MembershipProvider
    {
        public static User CurrentUser { get; set; }
    }
}
