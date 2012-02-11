using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using Instatus.Web;

namespace Instatus
{
    public static class MailMessageExtensions
    {
        public static void Send(this MailMessage message)
        {
            var smtpClient = new SmtpClient();

            if (WebApp.IsDebugOrLocal)
            {
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                smtpClient.PickupDirectoryLocation = WebPath.Server("~/App_Data/");
            }

            smtpClient.Send(message);
        }
    }
}