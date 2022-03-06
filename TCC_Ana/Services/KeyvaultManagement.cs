using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TCC_Ana.Services
{
    public class KeyvaultManagement : IKeyvaultManagement
    {
        readonly IConfiguration _config;

        public KeyvaultManagement(IConfiguration config)
        {
            _config = config;
        }

        public SecretClient CreateKVSecretClient()
        {
            string keyVaultName = _config.GetValue<string>("keyVaultName");

            SecretClientOptions options = new SecretClientOptions()
            {
                Retry =
                    {
                        Delay = TimeSpan.FromSeconds(2),
                        MaxDelay = TimeSpan.FromSeconds(16),
                        MaxRetries = 5,
                        Mode = Azure.Core.RetryMode.Exponential
                    }
            };

            var kVUri = "https://" + keyVaultName + ".vault.azure.net/";

            return new SecretClient(new Uri(kVUri), new DefaultAzureCredential(), options);
        }

        public string RetrieveKeyVaultSecret(string secretName)
        {

            //const string secretName = "mySecret";
            //var keyVaultName = Environment.GetEnvironmentVariable("KEY_VAULT_NAME");
            //var kvUri = $"https://{keyVaultName}.vault.azure.net";

            //var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());


            //Console.WriteLine($"Retrieving your secret from {keyVaultName}.");
            //var secret = await client.GetSecretAsync(secretName);
            //Console.WriteLine($"Your secret is '{secret.Value.Value}'.");


            try
            {
            var secret = CreateKVSecretClient().GetSecret(secretName);
            Console.WriteLine($"Your secret is '{secret.Value.Value}'.");

                //return Ok(VaultClient.GetSecret("LeonaldoTest").Value.Value);

            }
            catch (Exception e)
            {

                throw;
            }


            return "";
            //return secret.Value.Value.ToString();
        }

        public async Task UpdateKeyVaultSecretAsync(string secretName, string newSecretValue)
        {
            await CreateKVSecretClient().SetSecretAsync(secretName, newSecretValue);
        }
    }
}
