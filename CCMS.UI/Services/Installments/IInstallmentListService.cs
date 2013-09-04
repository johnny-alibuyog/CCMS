using System;
using CCMS.UI.Features.Installments;

namespace CCMS.UI.Features.Installments
{
    public interface IInstallmentListService
    {
        InstallmentListViewModel ViewModel { get; set; }
        void Delete(InstallmentViewModel item);
        void Insert(InstallmentViewModel item);
        void PopulateList();
    }
}
