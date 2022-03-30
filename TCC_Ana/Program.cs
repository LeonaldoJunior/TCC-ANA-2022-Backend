using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using TCC_Ana.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Azure.KeyVault;
using Azure.Identity;
using Azure.Core;

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
                //.ConfigureAppConfiguration((context, config) =>
                //{
                //    var settings = config.Build();
                //    //var keyVaultEndpoint = settings["VaultUri"];
                //    var keyVaultEndpoint = "https://kv-tcc-ana.vault.azure.net/";
                

                //    var keyVaultClient = new KeyVaultClient(async (authority, resource, scope) =>
                //    {
                //        var credentials = new DefaultAzureCredential(false);
                //        var token = credentials.GetToken(
                //            new TokenRequestContext(
                //                new[] { "https://vault.azure.net/.default" }));

                //        return token.Token;

                //    });

                //    config.AddAzureKeyVault(keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());


                //}
                //)
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<IKeyvaultManagement, KeyvaultManagement>();
                });
        
        
    }
}
    