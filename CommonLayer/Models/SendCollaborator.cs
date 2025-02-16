using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace CommonLayer.Models
{
    public class SendCollaborator
    {
        public string SendMail(string ToEmail, int NotesId, string Collaborator)
        {
            string FromEmail = "poojadonawadi7@gmail.com";
            MailMessage message = new MailMessage(FromEmail, ToEmail);
            string MailBody = "Hii! " + ToEmail+" You have collaborated with notes:"+NotesId;
            message.Subject = "Collaborator by person"+ Collaborator;
            message.Body = MailBody.ToString();
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            NetworkCredential credential = new NetworkCredential("poojadonawadi7@gmail.com", "imsk hnuc gnhj qdru");

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = true;
            smtpClient.Credentials = credential;

            smtpClient.Send(message);
            return ToEmail;
        }
    }
}
