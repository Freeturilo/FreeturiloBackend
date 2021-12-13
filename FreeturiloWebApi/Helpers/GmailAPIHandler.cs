using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using FreeturiloWebApi.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace FreeturiloWebApi.Helpers
{
    static public class GmailAPIHandler
    {
        private static readonly string subject = "Freeturilo - zgłoszenie niesprawnej stacji";
        private static string CreateBody(Administrator admin, Station station)
        {
            string body = "Cześć " + admin.Name + " " + admin.Surname + "!" + "\n\n" +
                $"Stacja Veturilo {station.Name} (id: {station.Id}) została zgłoszona przez użytkowników jako niesprawna!\n\n" +
                "Wiadomość wygenerowana automatycznie\n" +
                "Serwer Freeturilo";

            return body;
        }
        public static void SendEmail(Administrator admin, Station station)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("freeturilo@gmail.com", "Freeturilo");
            var subject = GmailAPIHandler.subject;
            var to = new EmailAddress(admin.Email, $"{admin.Name} {admin.Surname}");
            var plainTextContent = CreateBody(admin, station);
            var htmlContent = "<p>" + CreateBody(admin, station) + "</p>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            client.SendEmailAsync(msg).Wait();
        }
    }
}
