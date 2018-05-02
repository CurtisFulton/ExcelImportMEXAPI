using System;
using System.Windows.Input;

namespace ImportTemplateView
{
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action RelayAction { get; set; }

        public RelayCommand(Action action)
        {
            RelayAction = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            RelayAction();
        }
    }
}
