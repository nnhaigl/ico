using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace ICOCore.Utils.Mail
{
    public class MailUtils
    {
        private static string HOST;
        private static int PORT;
        private static string USERNAME;
        private static string PASSWORD;
        private static string FROM;
        private static bool ENABLE_SSL;
        private static string TO;
        private static string CC;

        static MailUtils()
        {
            try
            {
                HOST = ConfigurationManager.AppSettings["mail_host"].ToString();
                PORT = int.Parse(ConfigurationManager.AppSettings["mail_port"].ToString());
                USERNAME = ConfigurationManager.AppSettings["mail_username"].ToString();
                PASSWORD = ConfigurationManager.AppSettings["mail_password"].ToString();
                FROM = ConfigurationManager.AppSettings["mail_from"].ToString();
                ENABLE_SSL = bool.Parse(ConfigurationManager.AppSettings["mail_enableSSL"].ToString());
                TO = ConfigurationManager.AppSettings["mail_to"].ToString();
                CC = ConfigurationManager.AppSettings["mail_cc"].ToString();
            }
            catch (Exception ex)
            {
            }
        }

        public static Mailer GetDefaultMailer()
        {
            Mailer mailer = new Mailer();

            mailer.Server = HOST;
            mailer.Port = PORT;
            mailer.Account = USERNAME;
            mailer.Password = PASSWORD;
            mailer.Name = FROM;
            mailer.EnableSSL = ENABLE_SSL;
            mailer.To = TO;
            mailer.CC = CC;

            return mailer;
        }

        public static bool Send(Mailer mailer)
        {
            int intResult = 0;
            string MailResult = string.Empty;
            Send(mailer, ref intResult, ref MailResult);
            if (intResult == MailConstants.SUCCESS)
                return true;
            return false;
        }

        public static void Send(Mailer mailer, ref int intResult, ref string MailResult)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;

                SmtpClient mailClient = new SmtpClient();
                NetworkCredential basicAuthenticationInfo = new NetworkCredential();

                basicAuthenticationInfo.UserName = mailer.Account;
                basicAuthenticationInfo.Password = mailer.Password;
                mailClient.UseDefaultCredentials = false;
                mailClient.Credentials = basicAuthenticationInfo;
                mailClient.Host = mailer.Server;
                mailClient.Port = mailer.Port;
                mailClient.EnableSsl = mailer.EnableSSL;
                System.Net.Mail.MailMessage mailmessage = new System.Net.Mail.MailMessage(mailer.Account, mailer.To, mailer.Subject, mailer.Body);
                if (mailer.CC != null && mailer.CC.Trim() != "")
                {
                    List<string> ccs = mailer.CC.Split(';').ToList();
                    foreach (string s in ccs)
                    {
                        mailmessage.CC.Add(s);
                    }
                }
                //System.Net.Mail.Attachment Attach = null;
                //if (Attach != null)
                //    mailmessage.Attachments.Add(Attach);
                mailmessage.IsBodyHtml = true;

                // ---------- CAUSE THE PROBLEM ------------- //
                //mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                mailmessage.SubjectEncoding = System.Text.Encoding.UTF8;
                mailmessage.BodyEncoding = System.Text.Encoding.UTF8;

                mailClient.Send(mailmessage);
                MailResult = "Mail(s) sent successfully.";
                intResult = MailConstants.SUCCESS;

                //_logger.Info(string.Format(" ------------------------ GỬI MAIL THÀNH CÔNG TỚI ĐỊA CHỈ {0}-----------------", mailer.To));
                //_logger.Info(" ----------------------------- NỘI DUNG MAIL ĐÃ GỬI ------------------------");
                //_logger.Info(mailer.Body);
            }
            catch (Exception ex)
            {

                MailResult = "Mail Unsuccessful : " + ex;
                intResult = MailConstants.FAILED;
            }
        }


        private static void SendMailTest()
        {
            try
            {
                string fromaddr = "qfdsfsdf.1@gmail.com";
                string toaddr = "qfdsfsdf.1@gmail.com";
                string password = "131414ff1";

                MailMessage msg = new MailMessage();
                msg.Subject = "--- Test ---";
                msg.From = new MailAddress(fromaddr);
                msg.Body = "Message BODY";
                msg.To.Add(new MailAddress(toaddr));

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                NetworkCredential nc = new NetworkCredential(fromaddr, password);
                smtp.Credentials = nc;
                smtp.Send(msg);
            }
            catch (Exception ex)
            {

            }
        }

        public static string TestRenderHtmlTemplate()
        {
            StringWriter writer = new StringWriter();
            HtmlTextWriter html = new HtmlTextWriter(writer);

            html.RenderBeginTag(HtmlTextWriterTag.H1);
            html.WriteEncodedText("Heading Here");
            html.RenderEndTag();

            string test = string.Empty;
            html.WriteEncodedText(String.Format("Dear {0}", test));
            html.WriteBreak();

            html.RenderBeginTag(HtmlTextWriterTag.A);
            html.WriteEncodedText("");
            html.RenderEndTag();

            html.RenderBeginTag(HtmlTextWriterTag.P);
            html.WriteEncodedText("2016 @ Bizz Sipping !");
            html.RenderEndTag();

            html.Flush();

            string htmlString = writer.ToString();
            return htmlString;
        }

        /// <summary>
        /// https://razorengine.codeplex.com/
        /// </summary>
        public static string HTML_TEMPLATE =
         @"<html>
              <head>
                <title></title>
              </head>
              <body>
                {#body}
              </body>
            </html>";

    }
}
