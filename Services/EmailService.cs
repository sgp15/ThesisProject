using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;

namespace ThesisProject.Services
{
    public class EmailService
    {
     private GmailService gmailService;

        /// <summary>
        /// 
        /// </summary>
        public EmailService()
        {
            string[] Scopes = { GmailService.Scope.GmailReadonly, GmailService.Scope.GmailLabels };
            string ApplicationName = "Gmail API Quickstart";

            UserCredential credential;

            using (var stream =
                new FileStream(@"C:\Users\swapn\Desktop\Thesis\ThesisProject\ThesisProject\App_Data\client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Gmail API service.
            this.gmailService = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<Label> GetLabels()
        {
        
            // Define parameters of request.
            UsersResource.LabelsResource.ListRequest request = gmailService.Users.Labels.List("me");

            // List labels.
            IList<Label> labels = request.Execute().Labels;
       
            return labels;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<Message> GetMessages(string query)
        {

            // Define parameters of request.
            UsersResource.MessagesResource.ListRequest request = gmailService.Users.Messages.List("me");
            request.Q = query;

            // List labels.
            IList<Message> messages = request.Execute().Messages;

            return messages;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public Message GetMessageDetail(String messageId)
        {
            try
            {
                Google.Apis.Gmail.v1.UsersResource.MessagesResource.GetRequest getRequest = this.gmailService.Users.Messages.Get("me", messageId);
                getRequest.Format = Google.Apis.Gmail.v1.UsersResource.MessagesResource.GetRequest.FormatEnum.Full;

                return getRequest.Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
                throw e;
            }

        }

        /// <summary>
        /// Base64s the decode.
        /// </summary>
        /// <returns>The decode.</returns>
        /// <param name="base64EncodedData">Base64 encoded data.</param>
        public string Base64Decode(string base64EncodedData)
        {

            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}