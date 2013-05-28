using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Instatus.ViewModels
{
    public class ReaderViewModel : BindableBase
    {
        private IEnumerable<IReader> readers;
        
        private string uri;

        public string Uri
        {
            get
            {
                return uri;
            }
            set
            {
                SetProperty(ref uri, value);
            }
        }

        private ObservableCollection<object> items = new ObservableCollection<object>();

        public ObservableCollection<object> Items
        {
            get
            {
                return items;
            }
        }

        private object selectedItem;

        public object SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                SetProperty(ref selectedItem, value);
            }
        }

        public ICommand LoadCommand { get; set; }

        private async Task Load()
        {
            if (string.IsNullOrEmpty(Uri))
            {
                return;
            }

            var uri = Uri;
            var reader = readers.FirstOrDefault(r => r.CanRead(uri));

            if (reader == null)
            {
                return;
            }

            var result = await reader.GetListAsync(uri);

            items.Clear();

            foreach (var item in result)
            {
                items.Add(item);
            }
        }

        private IReader GetFirstReader()
        {
            return readers.Where(r => r.CanRead(Uri)).FirstOrDefault();
        }

        public ReaderViewModel(IEnumerable<IReader> readers)
        {
            this.readers = readers;
            this.LoadCommand = new AsyncCommand(Load);
        }
    }
}