using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.ViewModels
{
    public class MasterDetailsViewModel : BindableBase
    {
        public StatusViewModel Status { get; private set; }
        public FavoritesViewModel Favorites { get; private set; }
        public ReaderViewModel Reader { get; private set; }
        public PreviewViewModel Preview { get; private set; }
        public AuthorViewModel Author { get; private set; }
        public EditorViewModel Editor { get; private set; }
        public ModeratorViewModel Moderator { get; private set; }

        public MasterDetailsViewModel(
            IFavorites favorites, 
            IEnumerable<IReader> readers, 
            IEnumerable<IPreview> previews, 
            IEnumerable<IAuthor> authors, 
            IEnumerable<IEditor> editors,
            IEnumerable<IModerator> moderators)
        {
            Status = new StatusViewModel();
            Favorites = new FavoritesViewModel(favorites);
            Reader = new ReaderViewModel(readers);

            Favorites.PropertyChanged += (c, e) =>
            {
                var selectedItem = Favorites.SelectedItem;
                
                if (e.PropertyName == "SelectedItem" && selectedItem != null && !string.IsNullOrEmpty(selectedItem.Uri))
                {
                    var uri = selectedItem.Uri;
                    
                    Reader.Uri = uri;
                    Reader.LoadCommand.Execute(null);
                    Author = new AuthorViewModel(Status, uri, authors);
                    Preview = null;
                    Editor = null;
                    Moderator = null;

                    var saveCommand = Author.SaveCommand as StatusCommand;

                    if (saveCommand != null)
                    {
                        saveCommand.Success += (f, g) =>
                        {
                            Author.Item = null;
                            Reader.LoadCommand.Execute(null);
                        };
                    }
                }
            };

            Reader.PropertyChanged += (c, e) =>
            {
                var selectedItem = Reader.SelectedItem;

                if (e.PropertyName == "SelectedItem" && selectedItem != null)
                {
                    var uri = (string)((dynamic)selectedItem).Uri;

                    Preview = new PreviewViewModel(Status, uri, selectedItem, previews);
                    Editor = new EditorViewModel(Status, uri, editors);
                    Moderator = new ModeratorViewModel(Status, uri, moderators);

                    var saveCommand = Editor.SaveCommand as StatusCommand;

                    if (saveCommand != null)
                    {
                        saveCommand.Success += (f, g) =>
                        {
                            Editor.Item = null;
                            Reader.LoadCommand.Execute(null);
                        };
                    }
                }
                else
                {
                    Preview = null;
                    Author = null;
                    Editor = null;
                }
            };
        }
    }
}