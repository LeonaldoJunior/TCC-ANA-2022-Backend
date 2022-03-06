using Azure.Security.KeyVault.Secrets;
using System.Threading.Tasks;

namespace TCC_Ana.Services
{
    public interface IKeyvaultManagement
    {
        SecretClient CreateKVSecretClient();
        public string RetrieveKeyVaultSecret(string secretName);
        Task UpdateKeyVaultSecretAsync(string secretName, string newSecretValue);
    }
}