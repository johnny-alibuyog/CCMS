using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Features.CashAdvances;

namespace CCMS.UI.Features.CashAdvances
{
    public interface ICashAdvanceListService
    {
        CashAdvanceListViewModel ViewModel { get; set; }
        void Insert(CashAdvanceViewModel item);
        void Delete(CashAdvanceViewModel item);
        void PopulateList();
    }
}
