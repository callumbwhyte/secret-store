using System.Collections.Generic;
using Our.Umbraco.SecretStore.Models;

namespace Our.Umbraco.SecretStore.Services
{
    public interface ISecretService
    {
        IEnumerable<ISecretValue> GetAll();

        string GetSecret(string key);

        bool Exists(string key);

        ISecretValue Save(string key, string value);

        bool Delete(string key);
    }
}