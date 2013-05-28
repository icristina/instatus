using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Instatus.ViewModels
{
    public class PreviewViewModel : BindableBase
    {
        private string uri;
        private object model;
        private IEnumerable<IPreview> previews;

        public ICommand LoadCommand { get; set; }

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

        private async Task Load()
        {
            var preview = previews.FirstOrDefault(p => p.CanPreview(uri));

            Item = await preview.GetPreviewAsync(uri, model);
        }

        public PreviewViewModel(string uri, object model, IEnumerable<IPreview> previews)
        {
            this.uri = uri;
            this.model = model;
            this.previews = previews;
            this.LoadCommand = new AsyncCommand(Load);
        }
    }
}
