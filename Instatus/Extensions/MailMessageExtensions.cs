using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;

namespace Instatus
{
    public static class MailMessageExtensions
    {
        public static void Send(this MailMessage message)
        {
            var smtpClient = new SmtpClient();

            if (HttpContext.Current.ApplicationInstance.IsDebug())
            {
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                smtpClient.PickupDirectoryLocation = VirtualPathUtility.ToAbsolute("~/App_Data/");
            }

            smtpClient.Send(message);
        }
    }
}