using System;

namespace Our.Umbraco.SecretStore.Models
{
    public interface ISecretValue
    {
        string Name { get; }

        DateTime? UpdateDate { get; }
    }
}