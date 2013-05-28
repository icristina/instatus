using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Instatus.ViewModels
{
    public class AuthorViewModel : BindableBase
    {
        private string uri;
        private IEnumerable<IAuthor> authors;

        public ICommand CreateCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        private object item;

        public object Item
        {
            get
            {
                return item;
            }
            set
            {
                SetProperty(ref item, value);
            }
        }

        private void Create()
        {
            var author = authors.FirstOrDefault(a => a.CanCreate(uri));

            if (author == null)
            {
                return;
            }

            Item = author.CreateInstance();
        }

        private async Task Save()
        {
            var author = authors.FirstOrDefault(a => a.CanCreate(uri));

            if (author == null)
            {
                return;
            }

            await author.PostAsync(Item);
        }

        private void Cancel()
        {
            Item = null;
        }

        public AuthorViewModel(IEnumerable<IAuthor> authors)
        {
            this.authors = authors;
            this.CreateCommand = new RelayCommand(Create);
            this.SaveCommand = new AsyncCommand(Save);
            this.CancelCommand = new RelayCommand(Cancel);
        }
    }
}
