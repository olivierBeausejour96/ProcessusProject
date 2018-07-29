using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VersionOfficielle
{
    class CAlert
    {
        private const string AMIGO_EMAIL = "amigoalertspoker@gmail.com";

        private List<string> FFLstEmailAddress;
        private List<CPhoneNumber> FFLstPhoneNumbers;
                
        public CAlert(List<string> _lstEmailAddress, List<CPhoneNumber> _lstPhoneNumbers)
        {
            FFLstEmailAddress = _lstEmailAddress;
            FFLstPhoneNumbers = _lstPhoneNumbers;
        }

        public bool sendAlertToEmails(string _alertTitle, string _alertMessage)
        {
            if (FFLstEmailAddress == null || FFLstEmailAddress.Count <= 0)
                return false;

            MailMessage mail = new MailMessage();
            SmtpClient client = new SmtpClient();

            try
            {
                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 300000; // 5 minutes (in case the internet crash)
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(AMIGO_EMAIL, "beausejourgrenier");

                mail.To.Add(string.Join(",", FFLstEmailAddress));
                mail.From = new MailAddress(AMIGO_EMAIL);
                mail.Subject = _alertTitle;
                mail.Body = _alertMessage;
                mail.Priority = MailPriority.High;
                client.Send(mail);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool sendAlertToEmails(string _alertTitle, string _alertMessage, List<string> _lstImagesPath)
        {
            if (FFLstEmailAddress == null || FFLstEmailAddress.Count <= 0)
                return false;

            MailMessage mail = new MailMessage();
            SmtpClient client = new SmtpClient();

            try
            {
                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 300000; // 5 minutes (in case the internet crash)
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(AMIGO_EMAIL, "beausejourgrenier");
                
                mail.To.Add(string.Join(",", FFLstEmailAddress));
                mail.From = new MailAddress(AMIGO_EMAIL);
                mail.Subject = _alertTitle;
                mail.Body = _alertMessage;
                mail.Priority = MailPriority.High;

                for (int currentImage = 0; currentImage < _lstImagesPath.Count; ++currentImage)                
                    mail.Attachments.Add(new Attachment(_lstImagesPath[currentImage]));
                            
                client.Send(mail);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool sendAlertToPhoneNumbers(string _alertTitle, string _alertMessage)
        {
            if (FFLstPhoneNumbers == null || FFLstPhoneNumbers.Count <= 0)
                return false;

            MailMessage mail = new MailMessage();
            SmtpClient client = new SmtpClient();

            try
            {
                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 300000; // 5 minutes (in case the internet crash)
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(AMIGO_EMAIL, "beausejourgrenier");

                string emailAddresses = "";
                for (int currentPhoneNumber = 0; currentPhoneNumber < FFLstPhoneNumbers.Count - 1; currentPhoneNumber++)                
                    emailAddresses = FFLstPhoneNumbers[currentPhoneNumber].getPhoneEmail() + ",";                
                emailAddresses = FFLstPhoneNumbers.Last().getPhoneEmail();

                mail.From = new MailAddress(AMIGO_EMAIL);
                mail.Subject = _alertTitle;
                mail.Body = _alertMessage;
                mail.To.Add(emailAddresses);
                client.Send(mail);           

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
