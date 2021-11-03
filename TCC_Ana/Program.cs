using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Microsoft.Azure.KeyVault;

namespace TCC_Ana
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureAppConfiguration((context, config) =>
                {
                    var settings = config.Build();
                    var keyVaultEndpoint = settings["VaultUri"];

                    var keyVaultClient = new KeyVaultClient(async (authority, resource, scope) =>
                    {
                        var credentials = new DefaultAzureCredential(false);
                        var token = credentials.GetToken(
                            new TokenRequestContext(
                                new[] { "https://vault.azure.net/.default" }));

                        return token.Token;

                    });

                    config.AddAzureKeyVault(keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());


                }
        );
        
    }
}
    