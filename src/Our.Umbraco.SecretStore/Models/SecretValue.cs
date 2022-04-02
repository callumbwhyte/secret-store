using System;

namespace Our.Umbraco.SecretStore.Models
{
    internal class SecretValue : ISecretValue
    {
        public string Name { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}