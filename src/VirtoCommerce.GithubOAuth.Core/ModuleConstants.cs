using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.GithubOAuth.Core
{
    public static class ModuleConstants
    {
        public static class Security
        {
            public static class Permissions
            {
                public const string Access = "GithubOAuth:access";
                public const string Create = "GithubOAuth:create";
                public const string Read = "GithubOAuth:read";
                public const string Update = "GithubOAuth:update";
                public const string Delete = "GithubOAuth:delete";

                public static string[] AllPermissions { get; } =
                {
                Access,
                Create,
                Read,
                Update,
                Delete,
            };
            }
        }

        public static class Settings
        {
            public static class General
            {
                public static SettingDescriptor GithubOAuthEnabled { get; } = new SettingDescriptor
                {
                    Name = "GithubOAuth.GithubOAuthEnabled",
                    GroupName = "GithubOAuth|General",
                    ValueType = SettingValueType.Boolean,
                    DefaultValue = false,
                };

                public static SettingDescriptor GithubOAuthPassword { get; } = new SettingDescriptor
                {
                    Name = "GithubOAuth.GithubOAuthPassword",
                    GroupName = "GithubOAuth|Advanced",
                    ValueType = SettingValueType.SecureString,
                    DefaultValue = "qwerty",
                };

                public static IEnumerable<SettingDescriptor> AllGeneralSettings
                {
                    get
                    {
                        yield return GithubOAuthEnabled;
                        yield return GithubOAuthPassword;
                    }
                }
            }

            public static IEnumerable<SettingDescriptor> AllSettings
            {
                get
                {
                    return General.AllGeneralSettings;
                }
            }
        }
    }
}
