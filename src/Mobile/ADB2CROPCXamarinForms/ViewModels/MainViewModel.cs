// <copyright file="MainViewModel.cs" company="InternetWideWorld.com">
// Copyright (c) George Leithead, InternetWideWorld.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>

namespace ADB2CROPCXamarinForms.ViewModels
{
	using System;
	using System.Net.Http;
	using System.Threading.Tasks;
	using System.Windows.Input;
	using ADB2CROPCXamarinForms.Helpers;
	using ADB2CROPCXamarinForms.ViewModels.Base;
	using Microsoft.Identity.Client;
	using Xamarin.Forms;
	using TokenCache = Helpers.TokenCache;

	public class MainViewModel : ViewModelBase
	{
		private readonly string serviceUrl;

		private string apiResult;

		public MainViewModel()
		{
			if (DesignMode.IsDesignModeEnabled)
			{
				return;
			}

			IsBusy = true;
			Title = "Main";

			string port = (AppSettingsManager.HelloServiceIP == "localhost" || AppSettingsManager.HelloServiceIP == "10.0.2.2") ? ":5001" : string.Empty;
			serviceUrl = $"https://{AppSettingsManager.HelloServiceIP}{port}/{AppSettingsManager.Settings["HelloServicePath"]}";
			OnCallApiCommand = new Command(async () => await OnCallApi());
			IsBusy = false;
		}

		public string ApiResult
		{
			get => apiResult;
			set
			{
				apiResult = value;
				NotifyPropertyChanged(() => ApiResult);
			}
		}

		public string AccessToken => TokenCache.Get();

		public string ExpiresOn => TokenCache.ExpiresOn.ToString();

		public ICommand OnCallApiCommand { get; set; }

		public async Task OnCallApi()
		{
			CheckAuth();

			try
			{
				this.ApiResult = $"Calling API {this.serviceUrl}";
				string token = TokenCache.Get();

#if DEBUG
				HttpClientHandler insecureHandler = this.GetInsecureHandler();
				HttpClient client = new HttpClient(insecureHandler);
#else
				HttpClient client = new HttpClient();
#endif

				// Get data from API
				client.Timeout = TimeSpan.FromSeconds(10);
				HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, this.serviceUrl);
				message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
				HttpResponseMessage response = await client.SendAsync(message);
				string responseString = await response.Content.ReadAsStringAsync();
				if (response.IsSuccessStatusCode)
				{
					this.ApiResult = $"Response from API {this.serviceUrl} | {responseString}";
				}
				else
				{
					this.ApiResult = $"Error calling API {this.serviceUrl} | {responseString}";
				}
			}
			catch (MsalUiRequiredException ex)
			{
				await this.DialogService.DisplayAlert($"Session has expired, please sign out and back in.", ex.ToString(), "Dismiss");
			}
			catch (Exception ex)
			{
				await this.DialogService.DisplayAlert($"Exception:", ex.ToString(), "Dismiss");
			}
		}

		/// <summary>Ignore local SSL errors.</summary>
		/// <remarks>Attempting to invoke a local secure web service from an application running in the iOS simulator or Android emulator will result in a HttpRequestException being thrown, even when using the managed network stack on each platform. This is because the local HTTPS development certificate is self-signed, and self-signed certificates aren't trusted by iOS or Android. Therefore, it's necessary to ignore SSL errors when an application consumes a local secure web service.</remarks>
		/// <returns>HTTP Client handler.</returns>
		public HttpClientHandler GetInsecureHandler()
		{
			HttpClientHandler handler = new HttpClientHandler
			{
				ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
				{
					if (cert.Issuer.Equals("CN=localhost"))
					{
						return true;
					}

					return errors == System.Net.Security.SslPolicyErrors.None;
				},
			};
			return handler;
		}

		private async void CheckAuth()
		{
			if (TokenCache.IsExpired)
			{
				await Shell.Current.GoToAsync("///SignIn");
				IsBusy = false;
			}
		}
	}
}