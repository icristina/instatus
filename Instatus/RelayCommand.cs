using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Instatus
{
    public class RelayCommand : ICommand
    {
        private readonly Action action;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            action();
        }

        public RelayCommand(Action action)
        {
            this.action = action;
        }
    }
}
