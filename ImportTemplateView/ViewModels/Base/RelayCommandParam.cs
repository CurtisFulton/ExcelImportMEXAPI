using System;
using System.Windows.Input;

namespace ImportTemplateView
{
    class RelayCommandParam : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action<object> RelayAction { get; set; }
        public RelayCommandParam(Action<object> action)
        {
            RelayAction = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            RelayAction(parameter);
        }
    }
}
