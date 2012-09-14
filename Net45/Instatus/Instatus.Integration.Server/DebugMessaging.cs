using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.Hosting;
using Instatus.Core;

namespace Instatus.Integration.Server
{
    public class DebugMessaging : IMessaging
    {
        public void Send(string from, string to, string subject, string body, IMetadata metaData)
        {
            var mailMessage = new MailMessage(from, to, subject, body)
            {
                IsBodyHtml = true
            };

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                smtpClient.PickupDirectoryLocation = HostingEnvironment.MapPath(WellKnown.VirtualPath.AppData);                
                
                smtpClient.SendAsync(mailMessage, null);
            }
        }
    }
}
