using System;
using System.Net;
using System.Net.Mail;

namespace MyPoetryMobileService
{
    public static class EmailHandler
    {
        /// <summary>
        /// Sends a custom email from company account.
        /// </summary>
        /// <param name="toEmail">To</param>
        /// <param name="toDisplayName">Name of the recipient</param>
        /// <param name="subject">Subject</param>
        /// <param name="body">Message body</param>
        /// <param name="isBodyHtml">Body HTML content</param>
        public static void SendEmail(string toEmail, string toDisplayName, string subject, string body, bool isBodyHtml)
        {
            try
            {
                // Smtp client with authentication
                var gmailClient = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("mypoetry.app@gmail.com", ".UWPpoetry2017.")
                };

                // Adds from-to mailaddresses
                MailAddress from = new MailAddress("mypoetry.app@gmail.com", "MyPoetry");
                MailAddress to = new MailAddress(toEmail, toDisplayName);
                MailMessage myMail = new MailMessage(from, to);

                // Sets subject and encoding
                myMail.Subject = subject;
                myMail.SubjectEncoding = System.Text.Encoding.UTF8;

                // Sets body-message and encoding
                myMail.Body = body;
                myMail.BodyEncoding = System.Text.Encoding.UTF8;
                myMail.IsBodyHtml = isBodyHtml;

                // Sends email
                gmailClient.Send(myMail);
            }
            catch (Exception)
            {
            }
        }
    }
}