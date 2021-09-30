using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Identity.Client;

namespace OneDriveTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var filePath = @"C:\dev\Test\OneDriveTest\info.txt";

            var tenantId = "*";
            var clientId = "";
            var clientSecret = "";
            var userId = "";

            var instance = $"https://login.microsoftonline.com/{tenantId}";
            
            Console.WriteLine("Start");

            try
            {
                var scopes = new[] { "https://graph.microsoft.com/.default" };
                var options = new TokenCredentialOptions
                {
                    AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
                };

                var clientSecretCredential = new ClientSecretCredential(
                    tenantId, clientId, clientSecret, options);

                var graphClient = new GraphServiceClient(clientSecretCredential, scopes);

                var user = await graphClient.Users[userId].Request().GetAsync();

                Console.WriteLine(user.DisplayName);

                var fileData = System.IO.File.ReadAllBytes(filePath);

                var stream = new System.IO.MemoryStream(fileData);

                await graphClient.Users[userId].Drive.Root
                .ItemWithPath("info.txt")
                .Content
                .Request()
                .PutAsync<DriveItem>(stream);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
