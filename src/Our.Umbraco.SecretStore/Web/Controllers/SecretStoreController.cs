using System.Web.Http;
using Our.Umbraco.SecretStore.Services;
using Umbraco.Web.Editors;
using Umbraco.Web.WebApi;
using Umbraco.Web.WebApi.Filters;
using Applications = Umbraco.Core.Constants.Applications;

namespace Our.Umbraco.SecretStore.Web.Controllers
{
    [UmbracoApplicationAuthorize(Applications.Settings)]
    [JsonCamelCaseFormatter]
    public class SecretStoreController : UmbracoAuthorizedJsonController
    {
        private readonly ISecretService _secretService;

        public SecretStoreController(ISecretService secretService)
        {
            _secretService = secretService;
        }

        [HttpGet]
        public IHttpActionResult GetSecrets()
        {
            var secrets = _secretService.GetAll();

            return Ok(secrets);
        }

        [HttpPost]
        public IHttpActionResult Save(string key, string value)
        {
            var secret = _secretService.Save(key, value);

            if (secret == null)
            {
                return BadRequest($"Failed to save secret '{key}'");
            }

            return Ok(secret);
        }

        [HttpDelete]
        public IHttpActionResult Delete(string key)
        {
            if (_secretService.Exists(key) == false)
            {
                return BadRequest($"Secret '{key}' does not exist");
            }

            if (_secretService.Delete(key) == false)
            {
                return BadRequest($"Failed to delete secret '{key}'");
            }

            return Ok();
        }
    }
}