using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using VirtoCommerce.GithubOAuth.Core.Models;
using VirtoCommerce.Platform.Security.ExternalSignIn;

namespace VirtoCommerce.GithubOAuth.Data.Services
{
    public class GithubOAuthExternalSignInProvider : IExternalSignInProvider
    {
        public int Priority => 200;

        public bool HasLoginForm => false;

        public bool AllowCreateNewUser => true;

        private readonly GithubOAuthOptions _options;

        public GithubOAuthExternalSignInProvider(IOptions<GithubOAuthOptions> options)
        {
            _options = options.Value;
        }

        public string GetUserName(ExternalLoginInfo externalLoginInfo)
        {
            return externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);
        }

        public string GetUserType()
        {
            return _options.DefaultUserType;
        }
    }
}
