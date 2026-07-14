using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.GithubOAuth.Core.Models;
using VirtoCommerce.GithubOAuth.Data.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Security.ExternalSignIn;

namespace VirtoCommerce.GithubOAuth.Web
{
    public class Module : IModule, IHasConfiguration
    {
        public ManifestModuleInfo ModuleInfo { get; set; }
        public IConfiguration Configuration { get; set; }

        public void Initialize(IServiceCollection serviceCollection)
        {
            var optionsSection = Configuration.GetSection(GithubOAuthOptions.SectionName);
            serviceCollection.AddOptions<GithubOAuthOptions>().Bind(optionsSection).ValidateDataAnnotations();

            var options = optionsSection.Get<GithubOAuthOptions>() ?? new GithubOAuthOptions();

            if (!options.Enabled)
            {
                return;
            }

            var authBuilder = new AuthenticationBuilder(serviceCollection);

            authBuilder.AddGitHub(options.AuthenticationType, githubOptions =>
            {
                githubOptions.ClientId = options.ClientId;
                githubOptions.ClientSecret = options.ClientSecret;

                if (!string.IsNullOrEmpty(options.RedirectUrl))
                {
                    githubOptions.CallbackPath = Uri.TryCreate(options.RedirectUrl, UriKind.Absolute, out var redirectUri)
                        ? redirectUri.AbsolutePath
                        : options.RedirectUrl;
                }

                if (options.Scopes != null)
                {
                    githubOptions.Scope.AddRange(options.Scopes);
                }
            });

            serviceCollection.AddSingleton<GithubOAuthExternalSignInProvider>();
            serviceCollection.AddSingleton(provider => new ExternalSignInProviderConfiguration
            {
                AuthenticationType = options.AuthenticationType,
                Provider = provider.GetRequiredService<GithubOAuthExternalSignInProvider>(),
            });
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
