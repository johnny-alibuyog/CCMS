using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.CreditCardProviders
{
    public class ComputationSettingController : ControllerBase<ComputationSettingViewModel>
    {
        public ComputationSettingController(ComputationSettingViewModel viewModel) : base(viewModel)
        {
            this.ViewModel.Save = new ReactiveCommand(this.ViewModel
                .WhenAny(x => x.IsValid, x => x.Value == true));
            this.ViewModel.Save.Subscribe(x => Save());
        }

        private void Save()
        {
            var message = string.Format("Do you want to save Computation Settings?");
            var result = this.MessageBox.ShowQuestion(message);
            if (result == MessageBoxResult.OK)
                this.ViewModel.ActionResult = true;
        }
    }
}
