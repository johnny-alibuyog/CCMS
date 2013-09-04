using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.Staffs
{
    public class StaffController : ControllerBase<StaffViewModel>
    {
        public StaffController(StaffViewModel viewModel) : base(viewModel)
        {
            this.ViewModel.ObservableForProperty(x => x.Person.IsValid)
                .Subscribe(x => this.ViewModel.IsValid = x.Value);

            this.ViewModel.Save = new ReactiveCommand(this.ViewModel
                .WhenAny(x => x.IsValid, x => x.Value == true));
            this.ViewModel.Save.Subscribe(x => Save());
        }

        private void Save()
        {
            var message = string.Format("Do you want to save Staff?");
            var result = this.MessageBox.ShowQuestion(message);
            if (result == MessageBoxResult.OK)
                this.ViewModel.ActionResult = true;
        }
    }
}
