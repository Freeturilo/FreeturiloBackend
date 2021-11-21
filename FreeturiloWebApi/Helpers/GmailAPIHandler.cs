using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using FreeturiloWebApi.Models;

namespace FreeturiloWebApi.Helpers
{
    static public class GmailAPIHandler
    {
        private static MailAddress fromAddress = new("freeturilo@gmail.com", "Freeturilo");
        private static string fromPassword = "Freeturilo123PW!!";
        private static string subject = "Freeturilo - zgłoszenie niesprawnej stacji";
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
            var toAddress = new MailAddress(admin.Email, admin.Name + " " + admin.Surname);
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = CreateBody(admin, station)
            })
            {
                smtp.Send(message);
            }
        }
    }
}
