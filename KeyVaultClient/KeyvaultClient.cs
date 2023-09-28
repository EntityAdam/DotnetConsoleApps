using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Common
{
    public class KeyvaultClient : IKeyvaultClient
    {
        private readonly IConfiguration configuration;

        public KeyvaultClient(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task FetchConnectionStringsFromKeyvault()
        {
            //need to add Microsoft.Extensions.Configuration.Binder here. Not all that discoverable!
            var keyvault = configuration.GetValue<string>("AZURE_KEYVAULT_URI");
            Console.WriteLine($"Fetching connection string from {keyvault}");

            //For DefaultAzureCredential
            //Signing in as developer: make sure Azure/Azure Gov is configured, make sure you are signed in in Visual Studio, make sure you have the Account Selection corret in options
            var client = new SecretClient(new Uri(keyvault), new DefaultAzureCredential());

            var response = await client.GetSecretAsync("myservice-eventhubs-connectionstring");

            Console.WriteLine($"Event Hubs Connection String: '{response.Value.Value}'");
        }
    }
}