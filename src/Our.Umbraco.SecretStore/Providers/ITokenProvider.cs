using System;

namespace Our.Umbraco.SecretStore.Providers
{
    public interface ITokenProvider
    {
        string EncryptToken(string value);

        string DecryptToken(string value);
    }
}