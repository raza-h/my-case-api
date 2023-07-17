using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyCaseApi.Entities;
using MyCaseApi.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyCaseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private IWebHostEnvironment env;
        public CommonController (IWebHostEnvironment env)
        {
            this.env = env;
        }
        protected async Task<string> SaveFileAsync(byte[] file,string extention)
        {
            try
            {
                string hostRootPath = env.WebRootPath;
                string webRootPath = env.ContentRootPath;
                string _decumentPath = string.Empty;
                string _decuments = Guid.NewGuid().ToString();
                if (!string.IsNullOrEmpty(webRootPath))
                {
               
                    string path = webRootPath + "\\UploadDecuments\\";
                    string _decumentName = _decuments+"."+ extention;
                    _decumentPath = Path.Combine(path, _decumentName);
                    byte[] bytes = file;
                    System.IO.File.WriteAllBytes(_decumentPath, bytes);
                    return $"UploadDecuments/{_decumentName}";
                }
                else if (!string.IsNullOrEmpty(hostRootPath))
                {
                    string path = hostRootPath + "\\UploadDecuments\\";
                    string _decumentsName = _decuments +"."+extention;
                    _decumentPath = Path.Combine(path, _decumentsName);
                    byte[] bytes = file;
                    System.IO.File.WriteAllBytes(_decumentPath, bytes);
                    return _decumentPath;
                }
                _decumentPath = _decumentPath.Replace(" ", "");
                _decumentPath = _decumentPath.Substring(1);
                return _decumentPath;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected async Task<string> SaveFileAsyncIntegration(byte[] file, string extention,string fileName)
        {
            try
            {
                string hostRootPath = env.WebRootPath;
                string webRootPath = env.ContentRootPath;
                string _decumentPath = string.Empty;
                //string _decuments = Guid.NewGuid().ToString();
                string _decuments = fileName;
                if (!string.IsNullOrEmpty(webRootPath))
                {

                    string path = webRootPath + "\\Images\\Doc\\";
                    string _decumentName = _decuments;
                    _decumentPath = Path.Combine(path, _decumentName);
                    byte[] bytes = file;
                    System.IO.File.WriteAllBytes(_decumentPath, bytes);
                    return $"Images/Doc/{_decumentName}";
                }
                else if (!string.IsNullOrEmpty(hostRootPath))
                {
                    string path = hostRootPath + "\\Images\\Doc\\";
                    string _decumentsName = _decuments;
                    _decumentPath = Path.Combine(path, _decumentsName);
                    byte[] bytes = file;
                    System.IO.File.WriteAllBytes(_decumentPath, bytes);
                    return _decumentPath;
                }
                _decumentPath = _decumentPath.Replace(" ", "");
                _decumentPath = _decumentPath.Substring(1);
                return _decumentPath;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        protected async Task<string> SaveFileAsync(IFormFile file)
        {
            try
            {
                string imageName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(file.FileName)}";
                string webRootPath = env.ContentRootPath;
                string filePath = string.Empty;
                filePath = webRootPath + "\\Images\\" + imageName;
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return $"Images/{imageName}";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected string SaveImage(byte[] str, string ImgName)
        {
            string hostRootPath = env.WebRootPath;
            string webRootPath = env.ContentRootPath;
            string imgPath = string.Empty;

            if (!string.IsNullOrEmpty(webRootPath))
            {
                string path = webRootPath + "\\Images\\";
                string imageName = ImgName + ".jpg";
                imgPath = Path.Combine(path, imageName);
                byte[] bytes = str;
                System.IO.File.WriteAllBytes(imgPath, bytes);
                imgPath = $"Images/{imageName}";
            }
            else if (!string.IsNullOrEmpty(hostRootPath))
            {
                string path = hostRootPath + "\\Images\\";
                string imageName = ImgName + ".jpg";
                imgPath = Path.Combine(path, imageName);
                byte[] bytes = str;
                System.IO.File.WriteAllBytes(imgPath, bytes);
                imgPath = $"Images/{imageName}"; ;
            }
            return imgPath;
        }
        protected bool SendEmail(string toEmail, string emailBody = "", string subject = "")
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("solutionabsolute28@gmail.com");
                mail.To.Add(toEmail);
                mail.Subject = "Invitation to sign up on CMS";
                mail.Body = /*ConvertHtmlToString("Usman", "usman1@abs.com")*/ "Usman has invited you to sign up on CMS click <a href='#'>Complete Registration</a> to complete sign up";
                mail.IsBodyHtml = true;

                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("solutionabsolute28@gmail.com", "absolcase123");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
