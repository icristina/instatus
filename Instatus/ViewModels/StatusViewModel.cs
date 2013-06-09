using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.ViewModels
{
    public class StatusViewModel : BindableBase
    {
        private string message;

        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                SetProperty(ref message, value);
            }
        }

        private bool isBusy;

        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                SetProperty(ref isBusy, value);
            }
        }
    }
}
