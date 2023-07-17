using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCaseApi.Entities;
using MyCaseApi.Repositories;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using MyCaseApi.Interfaces;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using AbsolCase.Configurations;
using AutoMapper;
using MyCaseApi.Dtos;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Hosting;

namespace MyCaseApi.Controllers
{
    //[Authorize(Roles = "Attorney,Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class IntegrationController : CommonController
    {
        private readonly IAttorneyAdmin attorneyAdmin;
        private readonly ApiDbContext dbContext;
        private readonly UserManager<User> userManager;
        private readonly DocuSignHub _docuSignHub;
        private readonly IWebHostEnvironment env;
        public IntegrationController(IAttorneyAdmin attorneyAdmin, UserManager<User> userManager, ApiDbContext dbContext, DocuSignHub docuSignHub, IWebHostEnvironment env) : base(env)
        {
            this.attorneyAdmin = attorneyAdmin;
            this.userManager = userManager;
            this.dbContext = dbContext;
            _docuSignHub = docuSignHub;
            this.env = env;
        }

        #region DocuSign

        [HttpPost]
        [Route("SendEnvelope")]
        public async Task<ActionResult<object>> SendEnvelope(DocumentSign model)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var loggedInUser = await userManager.FindByEmailAsync(email);
                var getCurrentParent = dbContext.User.ToList().Where(x => x.Id == loggedInUser.ParentId).FirstOrDefault();
                if (getCurrentParent != null)
                {
                    var GetFirm = dbContext.Firm.ToList().Where(x => x.UserId == getCurrentParent.Id).FirstOrDefault();
                    if (GetFirm != null)
                    {

                        string[] users = model.UsersList.Split(',');
                        List<object> signers = new List<object>();

                        foreach (string userId in users)
                        {
                            if (!string.IsNullOrEmpty(userId))
                            {
                                var getUser = dbContext.User.FirstOrDefault(x => x.Id == userId);
                                if (getUser != null)
                                {
                                    model.RecipientId = getUser.Id;
                                    model.RecipientName = getUser.FirstName + getUser.LastName;
                                    model.RecipientEmail = getUser.Email;
                                    signers.Add(new
                                    {
                                        email = model.RecipientEmail,
                                        name = model.RecipientName,
                                        recipientId = model.RecipientId
                                    });
                                }
                            }
                        }

                        model.SignersArray = signers.ToArray();
                        if (model.SignersArray != null)
                        {
                            model.DocumentId = "1";
                            var adddata = await _docuSignHub.SendEnvelope(model);
                            if (adddata != null)
                            {
                                object responseData = JsonConvert.DeserializeObject<object>((string)adddata);
                                dynamic obj = responseData;
                                string envelopeId = obj.envelopeId;
                                string uri = obj.uri;
                                string statusDateTime = obj.statusDateTime;
                                string status = obj.status;


                                for (int i = 0; i < users.Count(); i++)
                                {
                                    if (users[i] != "")
                                    {
                                        string id = (users[i]);

                                        var getUser = dbContext.User.Where(x => x.Id == id).FirstOrDefault();

                                        DocumentSign data = new DocumentSign();
                                        data.RecipientId = getUser.Id;
                                        data.RecipientName = getUser.FirstName + getUser.LastName;
                                        data.DocumentStatus = status;
                                        data.DocumentString = uri;
                                        data.SignRequestedDate = DateTime.Now;
                                        data.SignCompletedDate = DateTime.Now;
                                        data.DocumentName = model.DocumentName;
                                        data.DocumentId = envelopeId;
                                        data.RecipientEmail = getUser.Email;
                                        await dbContext.DocumentSign.AddAsync(data);
                                        await dbContext.SaveChangesAsync();
                                    }
                                }


                            }
                            return Ok(new
                            {
                                IsSuccess = true,
                                StatusCode = StatusCodes.Status200OK,
                                Message = "Document Sent",
                            });
                        }
                        else
                        {

                        }
                        //}

                    }
                    return Ok(new
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status200OK,
                        Message = "Please Add Firm Details First",
                    });
                }
                else
                {
                    return BadRequest("Please Add Firm Details First");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new CaseApiResult<string>
                {
                    Data = string.Empty,
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message,
                    Exception = null,
                });
            }
        }

        [HttpGet]
        [Route("GetAuthTokken")]
        public async Task<IActionResult> GetAuthTokken(string code)
        {
            try
            {
                var gottoken = await _docuSignHub.GetTokken(code);
                if (gottoken != null)
                {
                    return Ok(gottoken);
                }
                else
                {
                    return Ok("false");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("GetAllDocumentSign")]
        public async Task<IActionResult> GetAllDocumentSign()
        {
            try
            {
                var doc = await dbContext.DocumentSign.ToListAsync();
                return Ok(doc);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetEnvelopeDocs")]
        public async Task<IActionResult> GetEnvelopeDocs(string envelopeId, string accessToken)
        {
            string accountId = "aaac805e-f893-411d-bc84-777b1d8ce1ed";

            // Construct the API endpoint URL
            string apiUrl = $"https://demo.docusign.net/restapi/v2.1/accounts/{accountId}/envelopes/{envelopeId}/documents";

            // Create an instance of HttpClient
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    // Set the authorization header with the access token
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    // Send the GET request
                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content as string
                        string responseBody = await response.Content.ReadAsStringAsync();
                        JObject responseJson = JObject.Parse(responseBody);
                        string documentId = responseJson["envelopeDocuments"][0]["documentId"].ToString();
                        string documentName = responseJson["envelopeDocuments"][0]["name"].ToString();
                        if (documentId != "")
                        {
                            var docId = documentId;
                            var docName = documentName;
                            var fileName = docName;
                            var getdocument = await GetDocumentFromDocuSignApi(envelopeId, docId, accessToken, fileName);
                            var getdocumentstatus = await GetRecipientsFromDocuSignApi(envelopeId, accessToken);
                            //string filePath = Path.Combine("D:/CASE/GitApi/CMS-WebServices/MyCaseApi/Images/Doc/", fileName);
                            return Ok(envelopeId);
                        }
                        // TODO: Process the response data as needed
                        Console.WriteLine(responseBody);
                    }
                    else
                    {
                        // Handle the unsuccessful response
                        Console.WriteLine($"Request failed with status code {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exceptions
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
            return Ok();
        }

        [HttpGet]
        [Route("GetDocumentFromDocuSignApi")]
        public async Task<IActionResult> GetDocumentFromDocuSignApi(string envelopeId, string docId, string accessToken, string fileName)
        {
            try
            {
                //envelopeId = "3a9b88a0-61f9-45bc-aa6a-5646d3e15555";
                //envelopeId = "f5e22e9a-b249-4b36-ba4e-b8422a57c501";
                //docId = "1";
                string apiUrl = $"https://demo.docusign.net/restapi/v2.1/accounts/aaac805e-f893-411d-bc84-777b1d8ce1ed/envelopes/{envelopeId}/documents/{docId}";

                using (HttpClient httpClient = new HttpClient())
                {
                    try
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                        HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                        if (response.IsSuccessStatusCode)
                        {
                            //var fileName = "sad";
                            byte[] documentBytes = await response.Content.ReadAsByteArrayAsync();
                            //string filePath = Path.Combine("D:/CASE/GitApi/CMS-WebServices/MyCaseApi/Images/Doc/", fileName);

                            //await System.IO.File.WriteAllBytesAsync(filePath, documentBytes);

                            var filesavedpath = "";
                            if (documentBytes != null && documentBytes.Length > 0)
                            {

                                filesavedpath = await SaveFileAsyncIntegration(documentBytes, ".pdf", fileName);
                            }
                            var data = dbContext.DocumentSign.Where(x => x.DocumentId == envelopeId).ToList();
                            for (int i = 0; i < data.Count(); i++)
                            {
                                data[i].DocumentSavedPath = filesavedpath;
                                dbContext.Update(data[i]);
                                dbContext.SaveChanges();
                            }
                            // Return the file as a response
                            //return PhysicalFile(filePath, "application/pdf", fileName);
                            return Ok();
                        }
                        else
                        {
                            Console.WriteLine($"Request failed with status code {response.StatusCode}");
                            return NotFound(); // Or any other appropriate response
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpGet]
        [Route("GetRecipientsFromDocuSignApi")]
        public async Task<IActionResult> GetRecipientsFromDocuSignApi(string envelopeId, string accessToken)
        {
            try
            {
                string apiUrl = $"https://demo.docusign.net/restapi/v2.1/accounts/aaac805e-f893-411d-bc84-777b1d8ce1ed/envelopes/{envelopeId}/recipients";

                using (HttpClient httpClient = new HttpClient())
                {
                    try
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                        HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                        if (response.IsSuccessStatusCode)
                        {
                            string responseBody = await response.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<dynamic>(responseBody);

                            foreach (var signer in result.signers)
                            {
                                string email = signer.email;
                                string status = signer.status;
                                string recipientId = signer.recipientId;
                                DateTime signeddate = DateTime.Now;
                                if (status == "completed")
                                {
                                    signeddate = signer.signedDateTime;
                                }

                                var getdatauser = dbContext.DocumentSign.Where(x => x.DocumentId == envelopeId && x.RecipientId == recipientId).FirstOrDefault();
                                if (getdatauser != null)
                                {
                                    getdatauser.DocumentStatus = status;
                                    if (status == "completed")
                                    {
                                        getdatauser.SignCompletedDate = signeddate;
                                    }
                                    dbContext.Update(getdatauser);
                                }
                                dbContext.SaveChanges();
                                //string recipientInfo = $"Email: {email}, Status: {status}";
                                //recipients.Add(recipientInfo);
                            }

                            // Use the 'recipients' list as needed

                            return Ok();
                        }
                        else
                        {
                            Console.WriteLine($"Request failed with status code {response.StatusCode}");
                            return NotFound(); // Or any other appropriate response
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");
                        return null;
                    }
                }



            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [HttpGet]
        [Route("GetDocumentsByEnvelopeId")]
        public async Task<IActionResult> GetDocumentsByEnvelopeId(string Id)
        {
            try
            {
                //int _Id = Convert.ToInt32(Id);
                DocumentSign model = await dbContext.DocumentSign.Where(x => x.DocumentId == Id).FirstOrDefaultAsync();
                return Ok(model);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }





        //public async Task<string> GetDocumentFromDocuSignApi(string envelopeId, string docId, string accessToken)
        //{
        //    try
        //    {

        //        envelopeId = "3a9b88a0-61f9-45bc-aa6a-5646d3e15555";
        //        docId = "1";
        //        string apiUrl = $"https://demo.docusign.net/restapi/v2.1/accounts/aaac805e-f893-411d-bc84-777b1d8ce1ed/envelopes/{envelopeId}/documents/{docId}";

        //        // Create an instance of HttpClient
        //        using (HttpClient httpClient = new HttpClient())
        //        {
        //            try
        //            {
        //                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        //                // Send the GET request
        //                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

        //                // Check if the request was successful
        //                if (response.IsSuccessStatusCode)
        //                {
        //                    // Read the response content as string
        //                    string responseBody = await response.Content.ReadAsStringAsync();
        //                    byte[] documentBytes = await response.Content.ReadAsByteArrayAsync();
        //                    string filePath = "path/to/file.ext"; // Specify the file path and extension

        //                    File.WriteAllBytes(filePath, documentBytes);
        //                    // Return the document data
        //                    return responseBody;
        //                }
        //                else
        //                {
        //                    // Handle the unsuccessful response
        //                    Console.WriteLine($"Request failed with status code {response.StatusCode}");
        //                    return null;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                // Handle any exceptions
        //                Console.WriteLine($"An error occurred: {ex.Message}");
        //                return null;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}





        #endregion
    }
}
