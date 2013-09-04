using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CCMS.UI.Infrastructure;
using NHibernate.Validator.Constraints;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CCMS.UI.Features.CreditCardProviders
{
    public class ComputationSettingViewModel : ViewModelBase
    {
        //private readonly ComputationSettingController _controller;

        public virtual Guid Id { get; set; }

        public virtual string Title { get; set; }

        public virtual string Details { get; set; }

        [NotNull]
        public virtual KeyValuePair<string, string> MinimumAmountCurrency { get; set; }

        public virtual decimal MinimumAmountValue { get; set; }

        public virtual decimal Rate { get; set; }

        public virtual ObservableCollection<KeyValuePair<string, string>> Currencies { get; set; }

        public virtual IReactiveCommand Save { get; set; }

        public virtual void HydrateWith(ComputationSettingViewModel value)
        {
            this.Id = value.Id;
            this.MinimumAmountCurrency = value.MinimumAmountCurrency;
            this.MinimumAmountValue = value.MinimumAmountValue;
            this.Rate = value.Rate;
        } 

        //public ComputationSettingViewModel()
        //{
        //    _controller = new ComputationSettingController(this);
        //}

        public ComputationSettingViewModel() { }

        public ComputationSettingViewModel(string title, string details) //: this()
        {
            this.Title = title;
            this.Details = details;
        }
    }
}
