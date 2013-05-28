using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Instatus.ViewModels
{
    public class EditorViewModel : BindableBase
    {
        private string uri;     
        private IEnumerable<IEditor> editors;

        public ICommand EditCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

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

        public async Task Edit()
        {
            var editor = editors.FirstOrDefault(e => e.CanEdit(uri));

            if (editor == null)
            {
                return;
            }

            Item = await editor.GetEditorAsync(uri);
        }

        public async Task Save()
        {
            var editor = editors.FirstOrDefault(e => e.CanEdit(uri));

            if (editor == null)
            {
                return;
            }

            await editor.PutAsync(uri, Item);
        }

        public void Cancel()
        {
            Item = null;
        }

        public async Task Delete()
        {
            var editor = editors.FirstOrDefault(e => e.CanDelete(uri));

            if (editor == null)
            {
                return;
            }

            await editor.DeleteAsync(uri);
        }

        public EditorViewModel(string uri, IEnumerable<IEditor> editors)
        {
            this.uri = uri;
            this.editors = editors;
            this.EditCommand = new AsyncCommand(Edit);
            this.SaveCommand = new AsyncCommand(Save);
            this.CancelCommand = new RelayCommand(Cancel);
            this.DeleteCommand = new AsyncCommand(Delete);
        }
    }
}