using Our.Umbraco.SecretStore.Providers;
using Our.Umbraco.SecretStore.Services;
using Our.Umbraco.SecretStore.Services.Implement;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace Our.Umbraco.SecretStore.Startup
{
    public class SecretStoreComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.RegisterUnique<ISecretService, SecretService>();

            composition.RegisterUnique<ITokenProvider, MachineKeyTokenProvider>();
        }
    }
}