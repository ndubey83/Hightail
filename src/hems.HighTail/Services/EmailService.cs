using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace hems.HighTail.Services {
    public class EmailService {

        public static void SendErrorMail(string errorMessage) {
            var addressTo = ConfigurationManager.AppSettings["NotificationEmail"].ToString().Split(',').ToList();
            SendMail(addressTo, "Data File job failure", string.Format("Data File job failed<br><b>Error:</b><br> {0}", errorMessage));
        }

        public static void SendSuccessMail() {
            var addressTo = ConfigurationManager.AppSettings["DevNotificationEmail"].ToString().Split(',').ToList();
            SendMail(addressTo, "Data File job success", string.Format("Data File job completed at {0}", DateTime.Now));
        }


        private static void SendMail(List<string> addressTo, string subject, string messageBody) {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("niteshdubey.jb@gmail.com", "Data File");
            foreach (string address in addressTo) {
                mail.To.Add(new MailAddress(address));
            }
            mail.Body = messageBody;
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            SmtpClient smtpClient = new SmtpClient(ConfigurationManager.AppSettings["SmtpServer"].ToString());
            smtpClient.Send(mail);
        }

    }
}
