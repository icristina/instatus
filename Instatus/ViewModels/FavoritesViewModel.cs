using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Instatus.ViewModels
{
    public class FavoritesViewModel : BindableBase
    {
        private IFavorites favorites;

        private ObservableCollection<LinkViewModel> items = new ObservableCollection<LinkViewModel>();

        public ObservableCollection<LinkViewModel> Items
        {
            get
            {
                return items;
            }
        }

        private LinkViewModel selectedItem;

        public LinkViewModel SelectedItem
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
            if (favorites == null)
            {
                return;
            }

            var result = await favorites.GetFavoritesAsync();

            items.Clear();

            foreach (var item in result)
            {
                items.Add(new LinkViewModel()
                {
                    Uri = item.Key.ToString(),
                    Title = item.Value
                });
            }
        }

        public FavoritesViewModel(IFavorites favorites)
        {
            this.favorites = favorites;
        }
    }
}
