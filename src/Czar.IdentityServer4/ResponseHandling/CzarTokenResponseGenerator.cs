using Czar.IdentityServer4.Options;
using IdentityModel;
using IdentityServer4.ResponseHandling;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Czar.IdentityServer4.ResponseHandling
{
    public class CzarTokenResponseGenerator : TokenResponseGenerator
    {

        private readonly DapperStoreOptions _config;
        private readonly ICache<CzarToken> _cache;
        public CzarTokenResponseGenerator(ISystemClock clock, ITokenService tokenService, IRefreshTokenService refreshTokenService, IResourceStore resources, IClientStore clients, ILogger<TokenResponseGenerator> logger, DapperStoreOptions config, ICache<CzarToken> cache) : base(clock, tokenService, refreshTokenService, resources, clients, logger)
        {
            _config = config;
            _cache = cache;
        }

        /// <summary>
        /// Processes the response.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public override async Task<TokenResponse> ProcessAsync(TokenRequestValidationResult request)
        {
            var result = new TokenResponse();
            switch (request.ValidatedRequest.GrantType)
            {
                case OidcConstants.GrantTypes.ClientCredentials:
                    result = await ProcessClientCredentialsRequestAsync(request);
                    break;
                case OidcConstants.GrantTypes.Password:
                    result = await ProcessPasswordRequestAsync(request);
                    break;
                case OidcConstants.GrantTypes.AuthorizationCode:
                    result = await ProcessAuthorizationCodeRequestAsync(request);
                    break;
                case OidcConstants.GrantTypes.RefreshToken:
                    result = await ProcessRefreshTokenRequestAsync(request);
                    break;
                default:
                    result = await ProcessExtensionGrantRequestAsync(request);
                    break;
            }
            if (_config.EnableForceExpire)
            {//增加白名单
                var token = new CzarToken();
                string key = request.ValidatedRequest.Client.ClientId;
                var _claim = request.ValidatedRequest.Subject?.FindFirst(e => e.Type == "sub");
                if (_claim != null)
                {
                    //提取amr
                    var amrval = request.ValidatedRequest.Subject.FindFirst(p => p.Type == "amr");
                    if (amrval != null)
                    {
                        key += amrval.Value;
                    }
                    key += _claim.Value;
                }
                //加入缓存
                if (!String.IsNullOrEmpty(result.AccessToken))
                {
                    token.Token = result.AccessToken;
                    await _cache.SetAsync(key, token, TimeSpan.FromSeconds(result.AccessTokenLifetime));
                }
            }
            return result;
        }
    }
}
