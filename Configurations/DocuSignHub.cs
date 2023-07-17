using AbsolCase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using MyCaseApi.Entities;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace AbsolCase.Configurations
{
    public class DocuSignHub
    {
        private static IHttpContextAccessor httpContextAccessor;
        private static IConfiguration config;
        public static void SetHttpContextAccessor(IHttpContextAccessor accessor, IConfiguration configuration)
        {
            httpContextAccessor = accessor;
            config = configuration;
        }


        public async Task<object> GetTokken(string code)
        {
            try
            {
                //string webBasePath = config.GetValue<string>("App:RemoteWebUrl");

                string clientId = "776af63d-ca72-46f3-b5fb-1515cca41d7b";
                string tokenEndpoint = "https://account-d.docusign.com/oauth/token";
                string clientSecret = "c0ffddd8-fa5b-4332-bd11-1575788790b9";
                string redirectUri = "http://localhost:25601/Attorney/Integrations/Token";
                //string redirectUri = webBasePath + "Attorney/Integrations/Token";
                HttpClient client = new HttpClient();

                var formContent = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("redirect_uri", redirectUri)
                });

                HttpResponseMessage response = await client.PostAsync(tokenEndpoint, formContent);

                string responseContent = await response.Content.ReadAsStringAsync();
                var serializedToken = responseContent;
                string serializedTokenStr = responseContent.ToString();
                //// Deserialize the JSON response
                //var responseObject = JsonConvert.DeserializeObject<dynamic>(responseContent);
                //// Retrieve the token from the response
                //string token = responseObject.access_token;
                //string refreshToken = responseObject.access_token;

                return serializedToken;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<object> SendEnvelope(DocumentSign sign)
        {
            try
            {
                string accountId = "aaac805e-f893-411d-bc84-777b1d8ce1ed";
                string accessToken = sign.AccessToken;
                string apiUrl = "https://demo.docusign.net/restapi/v2.1/accounts/";

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string requestUrl = $"{apiUrl}{accountId}/envelopes";

                var envelopeData = new
                {
                    documents = new[]
                    {
                        new
                        {
                            documentBase64 = sign.DocumentString,
                            documentId = sign.DocumentId,
                            fileExtension = "pdf",
                            name = sign.DocumentName
                        }
                    },
                    emailSubject = "Please read carefully and Sign the document",
                    //recipients = new
                    //{
                    //    signers = new[]
                    //                    {
                    //        new
                    //        {
                    //            email = "haseeb.shaukat@ab-sol.com",
                    //            name = sign.RecipientName,
                    //            recipientId = sign.RecipientId
                    //        }
                    //    }
                    //},
                    recipients = new
                    {
                        signers = sign.SignersArray // Use the signersArray here
                    },
                    status = "sent"
                };

                string jsonPayload = JsonConvert.SerializeObject(envelopeData);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(requestUrl, content);
                var respDataaaa = "";
                if (response.IsSuccessStatusCode)
                {
                    // Envelope creation successful
                    var responseContent = await response.Content.ReadAsStringAsync();
                    respDataaaa = responseContent;
                    // Process the response as needed
                }
                else
                {
                    // Envelope creation failed
                    var errorContent = await response.Content.ReadAsStringAsync();
                    respDataaaa = errorContent;
                    // Process the error message
                }
                return respDataaaa;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
