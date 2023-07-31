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
            // Initialize database
            //var connectionString = Configuration.GetConnectionString(ModuleInfo.Id) ??
            //                       Configuration.GetConnectionString("VirtoCommerce");

            //serviceCollection.AddDbContext<GithubOAuth>(options => options.UseSqlServer(connectionString));

            // Override models
            //AbstractTypeFactory<OriginalModel>.OverrideType<OriginalModel, ExtendedModel>().MapToType<ExtendedEntity>();
            //AbstractTypeFactory<OriginalEntity>.OverrideType<OriginalEntity, ExtendedEntity>();

            // Register services
            //serviceCollection.AddTransient<IMyService, MyService>();

            var GithubOAuthEnabled = Configuration.GetValue<bool>("GithubOAuth:Enabled");
            if (GithubOAuthEnabled)
            {
                // add options
                var optionsSection = Configuration.GetSection("GithubOAuth");
                var options = new GithubOAuthOptions();
                optionsSection.Bind(options);
                serviceCollection.Configure<GithubOAuthOptions>(optionsSection);

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

            // Register settings
            //var settingsRegistrar = serviceProvider.GetRequiredService<ISettingsRegistrar>();
            //settingsRegistrar.RegisterSettings(ModuleConstants.Settings.AllSettings, ModuleInfo.Id);

            // Register permissions
            //var permissionsRegistrar = serviceProvider.GetRequiredService<IPermissionsRegistrar>();
            //permissionsRegistrar.RegisterPermissions(ModuleConstants.Security.Permissions.AllPermissions
            //    .Select(x => new Permission { ModuleId = ModuleInfo.Id, GroupName = "GithubOAuth", Name = x })
            //    .ToArray());

            // Apply migrations
            using var serviceScope = serviceProvider.CreateScope();
            //using var dbContext = serviceScope.ServiceProvider.GetRequiredService<GithubOAuth>();
            //dbContext.Database.Migrate();
        }

        public void Uninstall()
        {
            // Nothing to do here
        }
    }
}
