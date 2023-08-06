using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MyCaseApi.ViewModels
{
    public class EmailService
    {
        public bool SendEmail(string toEmail)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("lydiacms00@gmail.com");
                mail.To.Add(toEmail);
                mail.Subject = "Invitation to sign up on CMS";
                mail.Body = ConvertHtml("Attorney");
                mail.IsBodyHtml = true;

                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("lydiacms00@gmail.com", "Cms@firm00");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool SendSignEmail(string toEmail)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("razah12145@gmail.com");
                mail.To.Add(toEmail);
                mail.Subject = "Solicited Sign on Document";
                mail.Body = ConvertSignHtml("Attorney");
                mail.IsBodyHtml = true;

                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("razah12145gmail.com", "pzvyhkqbvjvsoyyz");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool SendEmail(string toEmail, string Attorney, string url = "")
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("lydiacms00@gmail.com");
                mail.To.Add(toEmail);
                if (string.IsNullOrEmpty(Attorney))
                {
                    mail.Subject = "Reset password on CMS";
                    mail.Body = ConvertHtmlToStringForResetPassword(url);
                }
                else
                {
                    mail.Subject = "Invitation to sign up on CMS";
                    mail.Body = ConvertHtmlToString(Attorney, url);
                }
                mail.IsBodyHtml = true;

                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("lydiacms00@gmail.com", "Cms@firm00");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SendWelcomeEmail(string toEmail,string name, string Attorney, string url = "")
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("lydiacms00@gmail.com");
                mail.To.Add(toEmail);
                    mail.Subject = "Welcome to CMS";
                    mail.Body = ConvertHtmlToStringWelcome(name,Attorney, url);
                mail.IsBodyHtml = true;
                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("lydiacms00@gmail.com", "Cms@firm00");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool SendReminderEmail(string toEmail, string name, string Attorney, string url = "")
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("lydiacms00@gmail.com");
                mail.To.Add(toEmail);
                mail.Subject = "Meeting Reminder";
                mail.Body = ConvertHtmlToStringWelcome(name, Attorney, url);
                mail.IsBodyHtml = true;
                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("lydiacms00@gmail.com", "Cms@firm00");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private string ConvertHtml(string Attorney)
        {
            string html = string.Empty;
            html += "<style>body { margin-top: 20px;} </style>";
            html += "<table class='body-wrap' style='width:100%;'><tbody><tr><td></td><td>";
            html += "<div style='box-sizing:border-box;max-width:600px;margin:0 auto;padding:20px;'><table><tbody><tr>";
            html += "<td style='padding:30px;border:3px solid #67a8e4;border-radius:7px;'><meta><table><tbody><tr>";
            html += "<td style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += $"{Attorney} has invited you to sign up on CMS</td></tr><tr>";
            html += "<td style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += "If you want Sign in please click the button below to complete your registration process.";
            html += "</td></tr><tr><td style ='padding: 0 0 20px;'>";
            html += "<a href='#' class='btn-primary' itemprop='url' style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size: 14px; color: #FFF; text-decoration: none; line-height: 2em; font-weight: bold;cursor: pointer; display: inline-block; border-radius: 5px;background-color: #f06292;border-color: #f06292; border-style: solid; border-width: 8px 16px;'>";
            html += "Complete Registration</a></td></tr><tr>";
            html += "<td class='content-block' style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += "<b>Muhammad Usman</b><p>Support Team</p></td></tr><tr>";
            html += "<td style='text-align:center;font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;'>&copy; Absol Case";
            html += "</td></tr></tbody></table></td></tr></tbody></table></div></td></tr></tbody></table>";
            return html;
        }

        private string ConvertSignHtml(string Attorney)
        {
            string html = string.Empty;
            html += "<style>body { margin-top: 20px;} </style>";
            html += "<table class='body-wrap' style='width:100%;'><tbody><tr><td></td><td>";
            html += "<div style='box-sizing:border-box;max-width:600px;margin:0 auto;padding:20px;'><table><tbody><tr>";
            html += "<td style='padding:30px;border:3px solid #67a8e4;border-radius:7px;'><meta><table><tbody><tr>";
            html += "<td style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += $"{Attorney} has solicited your signature on a document.</td></tr><tr>";
            html += "<td style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += "If you want to sign, please click the button below.";
            html += "</td></tr><tr><td style ='padding: 0 0 20px;'>";
            html += "<a href='#' class='btn-primary' itemprop='url' style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size: 14px; color: #FFF; text-decoration: none; line-height: 2em; font-weight: bold;cursor: pointer; display: inline-block; border-radius: 5px;background-color: #f06292;border-color: #f06292; border-style: solid; border-width: 8px 16px;'>";
            html += "Sign</a></td></tr><tr>";
            html += "<td class='content-block' style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += "<b>Muhammad Usman</b><p>Support Team</p></td></tr><tr>";
            html += "<td style='text-align:center;font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;'>&copy; Absol Case";
            html += "</td></tr></tbody></table></td></tr></tbody></table></div></td></tr></tbody></table>";
            return html;
        }

        private string ConvertHtmlToString(string Attorney, string url)
        {
            string html = string.Empty;
            html += "<style>body { margin-top: 20px;} </style>";
            html += "<table class='body-wrap' style='width:100%;'><tbody><tr><td></td><td>";
            html += "<div style='box-sizing:border-box;max-width:600px;margin:0 auto;padding:20px;'><table><tbody><tr>";
            html += "<td style='padding:30px;border:3px solid #67a8e4;border-radius:7px;'><meta><table><tbody><tr>";
            html += "<td style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += $"{Attorney} has invited you to sign up on CMS</td></tr><tr>";
            html += "<td style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += "If you want Sign in please click the button below to complete your registration process.";
            html += "</td></tr><tr><td style ='padding: 0 0 20px;'><a href=";
            html += $"{url} class='btn-primary' itemprop='url' style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size: 14px; color: #FFF; text-decoration: none; line-height: 2em; font-weight: bold;cursor: pointer; display: inline-block; border-radius: 5px;background-color: #f06292;border-color: #f06292; border-style: solid; border-width: 8px 16px;'>";
            html += "Complete Registration</a></td></tr><tr>";
            html += "<td class='content-block' style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += "<b>Muhammad Usman</b><p>Support Team</p></td></tr><tr>";
            html += "<td style='text-align:center;font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;'>&copy; Absol Case";
            html += "</td></tr></tbody></table></td></tr></tbody></table></div></td></tr></tbody></table>";
            return html;
        }

        private string ConvertHtmlToStringWelcome(string name, string Attorney, string url)
        {
            string html = string.Empty;
            html += "<style>body { margin-top: 20px;} </style>";
            html += "<table class='body-wrap' style='width:100%;'><tbody><tr><td></td><td>";
            html += "<div style='box-sizing:border-box;max-width:600px;margin:0 auto;padding:20px;'><table><tbody><tr>";
            html += "<td style='padding:30px;border:3px solid #67a8e4;border-radius:7px;'><meta><table><tbody><tr>";
            html += "<td style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += $"Hi {name}. Get Started with streamlined case and practice management today!</td></tr><tr>";
            html += "<td style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += "If you want Sign in please click the button below to complete your registration process.";
            html += "</td></tr><tr><td style ='padding: 0 0 20px;'><a href=";
            html += $"{url} class='btn-primary' itemprop='url' style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size: 14px; color: #FFF; text-decoration: none; line-height: 2em; font-weight: bold;cursor: pointer; display: inline-block; border-radius: 5px;background-color: #f06292;border-color: #f06292; border-style: solid; border-width: 8px 16px;'>";
            html += "GO TO YOUR FIRM</a></td></tr><tr>";
            html += "<td class='content-block' style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += "<p>CMS is all-in-one legal practice management software for case and matter management, billing, and client communication</p></td></tr><tr>";
            html += "<td style='text-align:center;font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;'>&copy; Absol Case";
            html += "</td></tr></tbody></table></td></tr></tbody></table></div></td></tr></tbody></table>";
            return html;
        }
        private string ConvertHtmlToStringForResetPassword(string url)
        {
            string html = string.Empty;
            html += "<style>body { margin-top: 20px;} </style>";
            html += "<table class='body-wrap' style='width:100%;'><tbody><tr><td></td><td>";
            html += "<div style='box-sizing:border-box;max-width:600px;margin:0 auto;padding:20px;'><table><tbody><tr>";
            html += "<td style='padding:30px;border:3px solid #67a8e4;border-radius:7px;'><meta><table><tbody><tr>";
            html += "<td style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += $"If you want to reset your password please click the button below to set up a new password for your account. If you did not require it, simply ignore this email. This is confidential email, Please don't share it with anyone.</td></tr><tr>";
            html += "<td style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += "Please reset your password by clicking Reset Password.";
            html += "</td></tr><tr><td style ='padding: 0 0 20px;'><a href=";
            html += $"{url}' class='btn-primary' itemprop='url' style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size: 14px; color: #FFF; text-decoration: none; line-height: 2em; font-weight: bold;cursor: pointer; display: inline-block; border-radius: 5px;background-color: #f06292;border-color: #f06292; border-style: solid; border-width: 8px 16px;'>";
            html += "Reset Password</a></td></tr><tr>";
            html += "<td class='content-block' style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += "<b>Muhammad Usman</b><p>Support Team</p></td></tr><tr>";
            html += "<td style='text-align:center;font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;'>&copy; Absol Case";
            html += "</td></tr></tbody></table></td></tr></tbody></table></div></td></tr></tbody></table>";
            return html;
        }
    }
}
