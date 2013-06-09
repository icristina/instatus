using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Instatus.ViewModels
{
    public class StatusCommand : ICommand
    {
        private StatusViewModel status;
        private Func<Task> task;
        private string failureMessage;

        public bool CanExecute(object parameter)
        {
            return !status.IsBusy;
        }

        public event EventHandler CanExecuteChanged;
        public event EventHandler Success;

        public async void Execute(object parameter)
        {
            status.Message = null;
            status.IsBusy = true;
            
            try
            {
                await task();
            }
            catch
            {
                status.Message = failureMessage;
            }
            finally
            {
                status.IsBusy = false;

                var eventHandler = this.Success;

                if (eventHandler != null)
                {
                    eventHandler(this, EventArgs.Empty);
                }
            }
        }

        public StatusCommand(StatusViewModel status, Func<Task> task, string failureMessage)
        {
            this.status = status;
            this.task = task;
            this.failureMessage = failureMessage;
            this.status.PropertyChanged += (c, e) =>
            {
                if (e.PropertyName == "IsBusy")
                {
                    var eventHandler = this.CanExecuteChanged;

                    if (eventHandler != null)
                    {
                        eventHandler(this, EventArgs.Empty);
                    }
                }
            };
        }
    }
}
