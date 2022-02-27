using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace WorkerMail
{
    public class LogHelper
    {
        public void LogToFile(string message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\Logs\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/','_') + ".txt";
            if (!File.Exists(filePath))
            {
                using (StreamWriter streamWriter = File.CreateText(filePath))
                {
                    streamWriter.WriteLine($"{message} {DateTime.Now}");
                }
            }
            else
            {
                using (StreamWriter streamWriter = File.AppendText(filePath))
                {
                    streamWriter.WriteLine($"{message} {DateTime.Now}");
                }
            }
        }

        public void SendMail()
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential(userName:"senderMailAdress@gmail.com", password:"senderMailPassword");
            smtp.EnableSsl = true;

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(address: "senderMailAdress@gmail.com");
            mail.To.Add(addresses: "receiverMailAdress@windowslive.com");
            mail.Subject = "Worker Service";
            mail.Body = "This message was sent by worker service.";

            try
            {
                smtp.Send(mail);
                LogToFile("Mail sent successfully ");
            }
            catch (Exception ex)
            {
                LogToFile("ERROR: " + ex.Message);
            }
        }
    }
}
