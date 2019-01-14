using Czar.IdentityServer4.Options;
using Czar.IdentityServer4.ResponseHandling;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Czar.IdentityServer4.Validation
{
    /// <summary>
    /// 金焰的世界
    /// 2019-01-14
    /// Token请求校验增加白名单校验
    /// </summary>
    public class CzarIntrospectionRequestValidator : IIntrospectionRequestValidator
    {
        private readonly ILogger _logger;
        private readonly ITokenValidator _tokenValidator;
        private readonly DapperStoreOptions _config;
        private readonly ICache<CzarToken> _cache;
        public CzarIntrospectionRequestValidator(ITokenValidator tokenValidator, DapperStoreOptions config, ICache<CzarToken> cache, ILogger<CzarIntrospectionRequestValidator> logger)
        {
            _tokenValidator = tokenValidator;
            _config = config;
            _cache = cache;
            _logger = logger;
        }

        public async Task<IntrospectionRequestValidationResult> ValidateAsync(NameValueCollection parameters, ApiResource api)
        {
            _logger.LogDebug("Introspection request validation started.");

            // retrieve required token
            var token = parameters.Get("token");
            if (token == null)
            {
                _logger.LogError("Token is missing");

                return new IntrospectionRequestValidationResult
                {
                    IsError = true,
                    Api = api,
                    Error = "missing_token",
                    Parameters = parameters
                };
            }

            // validate token
            var tokenValidationResult = await _tokenValidator.ValidateAccessTokenAsync(token);

            // invalid or unknown token
            if (tokenValidationResult.IsError)
            {
                _logger.LogDebug("Token is invalid.");

                return new IntrospectionRequestValidationResult
                {
                    IsActive = false,
                    IsError = false,
                    Token = token,
                    Api = api,
                    Parameters = parameters
                };
            }

            _logger.LogDebug("Introspection request validation successful.");

            if (_config.EnableForceExpire)
            {//增加白名单校验判断
                var _key = tokenValidationResult.Claims.FirstOrDefault(t => t.Type == "client_id").Value;
                var _amr = tokenValidationResult.Claims.FirstOrDefault(t => t.Type == "amr");
                if (_amr != null)
                {
                    _key += _amr.Value;
                }
                var _sub = tokenValidationResult.Claims.FirstOrDefault(t => t.Type == "sub");
                if (_sub != null)
                {
                    _key += _sub.Value;
                }
                var _token = await _cache.GetAsync(_key);
                if (_token == null || _token.Token != token)
                {//已加入黑名单
                    _logger.LogDebug("Token已经强制失效");
                    return new IntrospectionRequestValidationResult
                    {
                        IsActive = false,
                        IsError = false,
                        Token = token,
                        Api = api,
                        Parameters = parameters
                    };
                }
            }
            // valid token
            return new IntrospectionRequestValidationResult
            {
                IsActive = true,
                IsError = false,
                Token = token,
                Claims = tokenValidationResult.Claims,
                Api = api,
                Parameters = parameters
            };
        }
    }
}
