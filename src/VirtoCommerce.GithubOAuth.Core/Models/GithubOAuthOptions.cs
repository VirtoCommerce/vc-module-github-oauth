namespace VirtoCommerce.GithubOAuth.Core.Models
{
    public class GithubOAuthOptions
    {
        public bool Enabled { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUrl { get; set; }
        public string DefaultUserType { get; set; }
        public string[] DefaultUserRoles { get; set; } = new[] { "Manager" };
        public string[] Scopes { get; set; } = new[] { "user:email" };
        public string AuthenticationType { get; set; }
    }
}
