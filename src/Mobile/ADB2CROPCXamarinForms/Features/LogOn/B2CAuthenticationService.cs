// <copyright file="B2CAuthenticationService.cs" company="InternetWideWorld.com">
// Copyright (c) George Leithead, InternetWideWorld.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>

namespace ADB2CROPCXamarinForms.Features.LogOn
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Security;
	using System.Text;
	using System.Threading.Tasks;
	using ADB2CROPCXamarinForms.Helpers;
	using ADB2CROPCXamarinForms.Interfaces;
	using ADB2CROPCXamarinForms.Models;
	using Microsoft.Identity.Client;
	using Newtonsoft.Json.Linq;
	using Xamarin.Forms;

	public class B2CAuthenticationService
	{
		private static readonly Lazy<B2CAuthenticationService> lazy = new Lazy<B2CAuthenticationService>(() => new B2CAuthenticationService());
		private readonly IPublicClientApplication AuthenticationClient;
		private readonly IPublicClientApplication AuthenticationRopcClient;

		private readonly string AuthorityRopc;
		private readonly string AuthoritySignIn;
		private readonly string[] Scopes;

		private B2CAuthenticationService()
		{
			string AzureADB2CHostname = string.Format(AppSettingsManager.Settings["AzureADB2CHostname"], AppSettingsManager.Settings["TenantName"]);
			string AuthorityBase = string.Format(AppSettingsManager.Settings["AuthorityBase"], AzureADB2CHostname, AppSettingsManager.Settings["TenantId"]);
			AuthorityRopc = string.Format("{0}{1}", AuthorityBase, AppSettingsManager.Settings["PolicyRopc"]);
			AuthoritySignIn = string.Format("{0}{1}", AuthorityBase, AppSettingsManager.Settings["PolicySignIn"]);
			Scopes = new string[] { string.Format(AppSettingsManager.Settings["Scopes"], AppSettingsManager.Settings["TenantId"]) };

			// default redirectURI; each platform specific project will have to override it with its own
			PublicClientApplicationBuilder builder = PublicClientApplicationBuilder.Create(AppSettingsManager.Settings["ClientId"])
				.WithB2CAuthority(AuthoritySignIn)
				.WithIosKeychainSecurityGroup(AppSettingsManager.Settings["IosKeychainSecurityGroups"])
				.WithRedirectUri($"msal{AppSettingsManager.Settings["ClientId"]}://auth");

			// Android implementation is based on https://github.com/jamesmontemagno/CurrentActivityPlugin
			// iOS implementation would require to expose the current ViewControler - not currently implemented as it is not required
			// UWP does not require this
			IParentWindowLocatorService windowLocatorService = DependencyService.Get<IParentWindowLocatorService>();
			if (windowLocatorService != null)
			{
				builder = builder.WithParentActivityOrWindow(() => windowLocatorService?.GetCurrentParentWindow());
			}

			AuthenticationClient = builder.Build();

			builder = PublicClientApplicationBuilder.Create(AppSettingsManager.Settings["ClientId"])
				.WithB2CAuthority(AuthorityRopc)
				.WithIosKeychainSecurityGroup(AppSettingsManager.Settings["IosKeychainSecurityGroups"])
				.WithRedirectUri($"msal{AppSettingsManager.Settings["ClientId"]}://auth");
			AuthenticationRopcClient = builder.Build();
		}

		public static B2CAuthenticationService Instance { get { return lazy.Value; } }

		public async Task<UserContext> AcquireTokenSilent()
		{
			IEnumerable<IAccount> accounts = await AuthenticationClient.GetAccountsAsync();
			AuthenticationResult authResult = await AuthenticationClient
				.AcquireTokenSilent(Scopes, GetAccountByPolicy(accounts, AppSettingsManager.Settings["PolicySignIn"]))
				.WithB2CAuthority(AuthoritySignIn)
				.ExecuteAsync();
			UserContext newContext = UpdateUserInfo(authResult);
			return newContext;
		}

		public async Task<UserContext> GetTokenForWebApiUsingUsernamePasswordAsync(string username, SecureString password)
		{
			AuthenticationResult authResult = await AuthenticationRopcClient.AcquireTokenByUsernamePassword(Scopes, username, password).ExecuteAsync();
			UserContext newContext = UpdateUserInfo(authResult);
			return newContext;
		}

		public async Task<UserContext> SignInAsync()
		{
			UserContext newContext;
			try
			{
				// acquire token silent
				newContext = await AcquireTokenSilent();
			}
			catch (MsalUiRequiredException)
			{
				// acquire token interactive
				newContext = await SignInInteractively();
			}
			return newContext;
		}

		public async Task<UserContext> SignOutAsync()
		{
			IEnumerable<IAccount> accounts = await AuthenticationClient.GetAccountsAsync();
			while (accounts.Any())
			{
				await AuthenticationClient.RemoveAsync(accounts.FirstOrDefault());
				accounts = await AuthenticationClient.GetAccountsAsync();
			}

			UserContext signedOutContext = new UserContext
			{
				IsLoggedOn = false
			};

			return signedOutContext;
		}

		public UserContext UpdateUserInfo(AuthenticationResult ar)
		{
			UserContext newContext = new UserContext
			{
				IsLoggedOn = false
			};

			JObject user = ParseIdToken(ar.IdToken);

			// NOTE: if you return additional user properties you want, add them here and to your UserContext object.
			newContext.AccessToken = ar.AccessToken;
			newContext.Issuer = user["iss"]?.ToString();
			newContext.Subject = user["sub"]?.ToString();
			newContext.Audience = user["aud"]?.ToString();
			newContext.ExpiresOn = ar.ExpiresOn;
			newContext.IsLoggedOn = true;
			return newContext;
		}

		private string Base64UrlDecode(string s)
		{
			s = s.Replace('-', '+').Replace('_', '/');
			s = s.PadRight(s.Length + (4 - s.Length % 4) % 4, '=');
			byte[] byteArray = Convert.FromBase64String(s);
			string decoded = Encoding.UTF8.GetString(byteArray, 0, byteArray.Count());
			return decoded;
		}

		private IAccount GetAccountByPolicy(IEnumerable<IAccount> accounts, string policy)
		{
			foreach (IAccount account in accounts)
			{
				string userIdentifier = account.HomeAccountId.ObjectId.Split('.')[0];
				if (userIdentifier.EndsWith(policy.ToLower()))
				{
					return account;
				}
			}

			return null;
		}

		private JObject ParseIdToken(string idToken)
		{
			// Get the piece with actual user info
			idToken = idToken.Split('.')[1];
			idToken = Base64UrlDecode(idToken);
			return JObject.Parse(idToken);
		}

		private async Task<UserContext> SignInInteractively()
		{
			AuthenticationResult authResult = await AuthenticationClient
				.AcquireTokenInteractive(Scopes)
				.WithPrompt(Prompt.SelectAccount)
				.WithB2CAuthority(AuthoritySignIn)
				.WithParentActivityOrWindow(App.UIParent)
				.WithUseEmbeddedWebView(true)
				.ExecuteAsync();

			UserContext newContext = UpdateUserInfo(authResult);
			return newContext;
		}
	}
}