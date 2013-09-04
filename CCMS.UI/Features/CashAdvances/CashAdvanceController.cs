using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using CCMS.Core.Services;
using CCMS.UI.Bootstrappers.IoC;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.CashAdvances
{
    public class CashAdvanceController : ControllerBase<CashAdvanceViewModel>
    {
        private IComputationService _calculator;

        public CashAdvanceController(CashAdvanceViewModel viewModel) : base(viewModel)
        {
            this.ViewModel.Save = new ReactiveCommand();
            this.ViewModel.Save.Subscribe(x => Save());

            this.ViewModel.WhenAny(x => x.Amount, x => x.Value)
                .Subscribe(x => ComputeFee(x));
        }

        private void Save()
        {
            var message = string.Format("Do you want to save Cash Advance?");
            var result = this.MessageBox.ShowQuestion(message);
            if (result == MessageBoxResult.OK)
                this.ViewModel.ActionResult = true;
        }

        private void ComputeFee(decimal amount)
        {
            if (_calculator == null)
                _calculator = IoC.Container.Resolve<IComputationService>();

            var serviceFee = _calculator.Compute<ServiceFeeSetting>(amount);
            this.ViewModel.ServiceFee = serviceFee != null ? serviceFee.Value : 0M;
        }
    }
}
