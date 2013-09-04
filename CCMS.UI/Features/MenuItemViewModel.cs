using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace CCMS.UI.Features
{
    public class MenuItemViewModel : ViewModelBase
    {
        public virtual string Text { get; set; }

        public virtual ICommand Command { get; set; } 

        public MenuItemViewModel() { }

        public MenuItemViewModel(string text, ICommand command)
        {
            this.Text = text;
            this.Command = command;
        } 
    }
}
