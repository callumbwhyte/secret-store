using System.Text;
using System.Web;
using System.Web.Security;

namespace Our.Umbraco.SecretStore.Providers
{
    public class MachineKeyTokenProvider : ITokenProvider
    {
        public string EncryptToken(string value)
        {
            if (string.IsNullOrWhiteSpace(value) == true)
            {
                return null;
            }

            var valueBytes = Encoding.UTF8.GetBytes(value);

            var token = MachineKey.Protect(valueBytes, "secret");
            
            return HttpServerUtility.UrlTokenEncode(token);
        }

        public string DecryptToken(string value)
        {
            if (string.IsNullOrWhiteSpace(value) == true)
            {
                return null;
            }

            var valueStream = HttpServerUtility.UrlTokenDecode(value);

            var secret = MachineKey.Unprotect(valueStream, "secret");

            return Encoding.UTF8.GetString(secret);
        }
    }
}