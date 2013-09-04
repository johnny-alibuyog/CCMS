using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.UI.Features.TransactionClassifications
{
    public class ListItemViewModel : ViewModelBase
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
    }
}
