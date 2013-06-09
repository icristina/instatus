
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

        private async Task Approve()
        {
            var moderator = moderators.FirstOrDefault(m => m.CanModerate(uri));

            if (moderator == null)
            {
                return;
            }

            await moderator.MarkApprovedAsync(uri);
        }

        private async Task Archive()
        {
            var moderator = moderators.FirstOrDefault(m => m.CanModerate(uri));

            if (moderator == null)
            {
                return;
            }

            await moderator.MarkArchivedAsync(uri);
        }

        private async Task Spam()
        {
            var moderator = moderators.FirstOrDefault(m => m.CanModerate(uri));

            if (moderator == null)
            {
                return;
            }

            await moderator.MarkSpamAsync(uri);
        }

        public ModeratorViewModel(StatusViewModel status, string uri, IEnumerable<IModerator> moderators)
        {
            this.uri = uri;
            this.moderators = moderators;
            this.ApproveCommand = new StatusCommand(status, Approve, "Failed to approve");
            this.ArchiveCommand = new StatusCommand(status, Archive, "Failed to archive");
            this.SpamCommand = new StatusCommand(status, Spam, "Failed to mark as spam");
        }
    }
}