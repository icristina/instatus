using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Instatus
{
    public class AsyncCommand : ICommand
    {
        private Func<Task> task;
        
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public async void Execute(object parameter)
        {
            try
            {
                await task();
            }
            catch
            {
                // do nothing
            }
        }

        public AsyncCommand(Func<Task> task)
        {
            this.task = task;
        }
    }
}
