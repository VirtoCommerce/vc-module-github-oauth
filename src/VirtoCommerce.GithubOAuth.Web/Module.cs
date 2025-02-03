using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.GithubOAuth.Core.Models;
using VirtoCommerce.GithubOAuth.Data.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Security.ExternalSignIn;

namespace VirtoCommerce.GithubOAuth.Web
{
    public class Module : IModule, IHasConfiguration
    {
        public ManifestModuleInfo ModuleInfo { get; set; }
        public IConfiguration Configuration { get; set; }

        public void Initialize(IServiceCollection serviceCollection)
        {
            // add options
            var optionsSection = Configuration.GetSection("GithubOAuth");
            var options = optionsSection.Get<GithubOAuthOptions>();
            optionsSection.Bind(options);
            serviceCollection.AddOptions<GithubOAuthOptions>().Bind(optionsSection).ValidateDataAnnotations();

            if (options.Enabled)
            {

                // add app builder google sso
                var authBuilder = new AuthenticationBuilder(serviceCollection);

                authBuilder.AddGitHub(githubOptions =>
                {
                    githubOptions.ClientId = options.ClientId;
                    githubOptions.ClientSecret = options.ClientSecret;
                    githubOptions.Scope.AddRange(options.Scopes);
                });

                // register Google external provider implementation
                serviceCollection.AddSingleton<GithubOAuthExternalSignInProvider>();
                serviceCollection.AddSingleton(provider => new ExternalSignInProviderConfiguration
                {

                    AuthenticationType = options.AuthenticationType,
                    Provider = provider.GetService<GithubOAuthExternalSignInProvider>(),
                });
            }
        }

        public void PostInitialize(IApplicationBuilder appBuilder)
        {
            var serviceProvider = appBuilder.ApplicationServices;

            // Apply migrations
            using var serviceScope = serviceProvider.CreateScope();
        }

        public void Uninstall()
        {
            // Nothing to do here
        }
    }
}
