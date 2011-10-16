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
            new SmtpClient().Send(message);
        }
    }
}