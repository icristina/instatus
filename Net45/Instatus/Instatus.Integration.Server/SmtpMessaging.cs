﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using Instatus.Core;
using Instatus.Core.Models;

namespace Instatus.Integration.Server
{
    public class SmtpMessaging : IMessaging
    {
        public void Send(string from, string to, string subject, string body, Metadata metaData)
        {
            var mailMessage = new MailMessage(from, to, subject, body)
            {
                IsBodyHtml = true
            };

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.SendAsync(mailMessage, null);
            }
        }
    }
}
