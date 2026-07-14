using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.GithubOAuth.Core.Models
{
    public class GithubOAuthOptions
    {
        public const string SectionName = "GithubOAuth";
        public bool Enabled { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string ClientId { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string ClientSecret { get; set; }

        public string RedirectUrl { get; set; }
        public string DefaultUserType { get; set; } = "Manager";
        public string[] DefaultUserRoles { get; set; }
        public string[] Scopes { get; set; } = { "user:email" };
        public string AuthenticationType { get; set; } = "GitHub";
    }
}
