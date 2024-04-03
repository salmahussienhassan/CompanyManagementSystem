using Company.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace Company.PL.Helper
{
    public static class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            var Client=new SmtpClient("smtp.gmail.com",587);
            Client.EnableSsl = true;
            Client.Credentials = new NetworkCredential("salmahussien417@gmail.com", "mrefeawtkcefcprj");
            Client.Send("salmahussien417@gmail.com",email.To,email.Subject,email.Body);

        }
    }
}
