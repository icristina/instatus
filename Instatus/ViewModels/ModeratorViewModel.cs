
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Instatus.ViewModels
{
    public class ModeratorViewModel
    {
        private string uri;
        private IEnumerable<IModerator> moderators;

        public ICommand ApproveCommand { get; set; }
        public ICommand ArchiveCommand { get; set; }
        public ICommand SpamCommand { get; set; }

        public async Task Approve()
        {
            var moderator = moderators.FirstOrDefault(m => m.CanModerate(uri));

            if (moderator == null)
            {
                return;
            }

            await moderator.MarkApprovedAsync(uri);
        }

        public async Task Archive()
        {
            var moderator = moderators.FirstOrDefault(m => m.CanModerate(uri));

            if (moderator == null)
            {
                return;
            }

            await moderator.MarkArchivedAsync(uri);
        }

        public async Task Spam()
        {
            var moderator = moderators.FirstOrDefault(m => m.CanModerate(uri));

            if (moderator == null)
            {
                return;
            }

            await moderator.MarkSpamAsync(uri);
        }

        public ModeratorViewModel(string uri, IEnumerable<IModerator> moderators)
        {
            this.uri = uri;
            this.moderators = moderators;
        }
    }
}