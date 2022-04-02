using System;
using System.Collections.Generic;
using System.Linq;
using Our.Umbraco.SecretStore.Models;
using Our.Umbraco.SecretStore.Persistence.Dtos;
using Our.Umbraco.SecretStore.Providers;
using Umbraco.Core;
using Umbraco.Core.Persistence;
using Umbraco.Core.Scoping;

namespace Our.Umbraco.SecretStore.Services.Implement
{
    internal class SecretService : ISecretService
    {
        private readonly IScopeProvider _scopeProvider;
        private readonly ITokenProvider _tokenProvider;

        public SecretService(IScopeProvider scopeProvider, ITokenProvider tokenProvider)
        {
            _scopeProvider = scopeProvider;
            _tokenProvider = tokenProvider;
        }

        public IEnumerable<ISecretValue> GetAll()
        {
            using (var scope = _scopeProvider.CreateScope(autoComplete: true))
            {
                var sql = scope.Database.SqlContext.Sql()
                    .Select<SecretDto>()
                    .From<SecretDto>()
                    .Where("[key] LIKE @0", $"{Constants.SecretPrefix}%")
                    .OrderBy<SecretDto>(x => x.Key);

                var results = scope.Database.Fetch<SecretDto>(sql);

                return results.Select(CreateValue);
            }
        }

        public ISecretValue GetByKey(string key)
        {
            key = key.EnsureStartsWith(Constants.SecretPrefix);

            using (var scope = _scopeProvider.CreateScope(autoComplete: true))
            {
                var sql = scope.Database.SqlContext.Sql()
                    .Select<SecretDto>()
                    .From<SecretDto>()
                    .Where<SecretDto>(x => x.Key == key);

                var result = scope.Database.FirstOrDefault<SecretDto>(sql);

                if (result == null)
                {
                    return null;
                }

                return CreateValue(result);
            }
        }

        public string GetSecret(string key)
        {
            key = key.EnsureStartsWith(Constants.SecretPrefix);

            using (var scope = _scopeProvider.CreateScope(autoComplete: true))
            {
                var sql = scope.Database.SqlContext.Sql()
                    .Select<SecretDto>()
                    .From<SecretDto>()
                    .Where<SecretDto>(x => x.Key == key);

                var result = scope.Database.FirstOrDefault<SecretDto>(sql);

                if (result == null)
                {
                    return null;
                }

                return _tokenProvider.DecryptToken(result.Secret);
            }
        }

        public bool Exists(string key)
        {
            return GetSecret(key) != null;
        }

        public ISecretValue Save(string key, string value)
        {
            key = key.EnsureStartsWith(Constants.SecretPrefix);

            using (var scope = _scopeProvider.CreateScope(autoComplete: true))
            {
                var secret = _tokenProvider.EncryptToken(value);

                var entity = new SecretDto
                {
                    Key = key,
                    Secret = secret,
                    UpdateDate = DateTime.UtcNow
                };

                scope.Database.Save(entity);

                return GetByKey(key);
            }
        }

        public bool Delete(string key)
        {
            key = key.EnsureStartsWith(Constants.SecretPrefix);

            using (var scope = _scopeProvider.CreateScope(autoComplete: true))
            {
                return scope.Database.Delete<SecretDto>("WHERE [key] = @0", key) > 0;
            }
        }

        private ISecretValue CreateValue(SecretDto secret)
        {
            return new SecretValue
            {
                Name = secret.Key.Replace(Constants.SecretPrefix, null),
                UpdateDate = secret.UpdateDate
            };
        }
    }
}