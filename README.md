# GithubOAuth Module

## Overview

The GithubOAuth module adds GitHub as an external authentication (SSO) provider for the VirtoCommerce platform. Once configured, platform users (e.g. Commerce Manager operators) can sign in using their GitHub account instead of a local username/password pair.

The module is built on top of [AspNet.Security.OAuth.GitHub](https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers) and the platform's `IExternalSignInProvider` extensibility point (`VirtoCommerce.Platform.Security.ExternalSignIn`), so it plugs into the existing external sign-in pipeline without any changes to core platform code.

Key capabilities:

- Adds a GitHub OAuth authentication scheme to the platform's authentication middleware.
- On successful GitHub login, resolves the platform username from the GitHub account's email claim.
- Can automatically provision a new platform user on first login (`AllowCreateNewUser`), assigning a configurable default user type and default roles.
- OAuth scopes, client credentials and redirect URL are fully configurable via `appsettings.json`.

This module does not replace or migrate any existing authentication mechanism — it is an additive external sign-in option that can be enabled alongside local accounts or other external providers (e.g. Google, Azure AD).

## Requirements

- VirtoCommerce Platform 3.318.0 or higher.
- A GitHub OAuth App (create one under GitHub → Settings → Developer settings → OAuth Apps) with the Authorization callback URL pointing to your platform instance, e.g. `https://<your-host>/signin-github`.

## Configuration

Configure the module in `appsettings.json` under the `GithubOAuth` section:

```json
{
  "GithubOAuth": {
    "Enabled": true,
    "ClientId": "<GitHub OAuth App Client ID>",
    "ClientSecret": "<GitHub OAuth App Client Secret>",
    "RedirectUrl": "https://<your-host>/signin-github",
    "AuthenticationType": "GitHub",
    "Scopes": [ "user:email" ],
    "DefaultUserType": "Manager",
    "DefaultUserRoles": [ "manager" ]
  }
}
```

| Setting | Description |
| --- | --- |
| `Enabled` | Turns the GitHub authentication scheme on or off. When `false` (or when the `GithubOAuth` section is absent), the module registers nothing. |
| `ClientId` / `ClientSecret` | Credentials issued by GitHub for your OAuth App. Required when `Enabled` is `true`. |
| `RedirectUrl` | Callback URL/path configured in the GitHub OAuth App (e.g. `https://<your-host>/signin-github` or just `/signin-github`). Applied to the handler's callback path. Defaults to `/signin-github` if omitted. |
| `AuthenticationType` | Name of the authentication scheme/external sign-in provider. Defaults to `GitHub`; only change this if you also change the GitHub OAuth App's callback accordingly, since it drives both the registered scheme and the platform's provider lookup. |
| `Scopes` | GitHub OAuth scopes requested during authentication. Defaults to `[ "user:email" ]`, which is required to resolve the user's email. |
| `DefaultUserType` | Platform user type assigned to auto-provisioned users. Defaults to `Manager`. |
| `DefaultUserRoles` | Platform roles assigned to auto-provisioned users. |

## How It Works

`Module.cs` reads the `GithubOAuth` configuration section and, when `Enabled` is `true`, registers the GitHub authentication handler (`AddGitHub`) under the configured `AuthenticationType` scheme, plus a `GithubOAuthExternalSignInProvider`. On sign-in, the provider:

1. Reads the user's email from the GitHub authentication claims to determine the platform username.
2. Reports the configured default user type and roles for new users.
3. Lets the platform's external sign-in pipeline create or sign in the corresponding platform account.

## Related Topics

- [AspNet.Security.OAuth.GitHub](https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers)
- [Creating an OAuth App on GitHub](https://docs.github.com/en/apps/oauth-apps/building-oauth-apps/creating-an-oauth-app)
- [VirtoCommerce Platform Security](https://docs.virtocommerce.org/platform/developer-guide/Security/)

## License

Copyright (c) Virto Solutions LTD.  All rights reserved.

Licensed under the Virto Commerce Open Software License (the "License"); you
may not use this file except in compliance with the License. You may
obtain a copy of the License at

<http://virtocommerce.com/opensourcelicense>

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
implied.