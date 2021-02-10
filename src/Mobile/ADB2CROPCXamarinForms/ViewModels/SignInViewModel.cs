namespace ADB2CROPCXamarinForms.ViewModels
{
	using ADB2CROPCXamarinForms.Features.LogOn;
	using ADB2CROPCXamarinForms.Helpers;
	using ADB2CROPCXamarinForms.Models;
	using ADB2CROPCXamarinForms.ViewModels.Base;
	using Microsoft.Identity.Client;
	using System;
	using System.Net;
	using System.Security;
	using System.Threading.Tasks;
	using System.Windows.Input;
	using Xamarin.Forms;
	using TokenCache = Helpers.TokenCache;

	public class SignInViewModel : ViewModelBase
	{
		private string password;
		private string userName;

		public SignInViewModel()
		{
			if (DesignMode.IsDesignModeEnabled)
			{
				return;
			}

			IsBusy = false;
			Title = "Sign In";
			SignInCommand = new Command(async () => await ExecuteSignIn());
			OnEnterPressedCommand = new Command(async () => await ExecuteSignIn());
			if (SecureSettingsManager.RememberMe)
			{
				UserName = SecureSettingsManager.RememberUserName;
			}
		}

		public ICommand OnEnterPressedCommand { get; set; }

		public string Password
		{
			get => password;
			set
			{
				if (password != value)
				{
					password = value;
					NotifyPropertyChanged(() => Password);
				}
			}
		}

		public bool Remeber
		{
			get => SecureSettingsManager.RememberMe;
			set
			{
				if (value != SecureSettingsManager.RememberMe)
				{
					SecureSettingsManager.RememberMe = value;
				}
			}
		}

		public ICommand SignInCommand { get; set; }

		public string UserName
		{
			get => userName;
			set
			{
				if (userName != value)
				{
					userName = value;
					NotifyPropertyChanged(() => UserName);
				}
			}
		}

		private async Task ExecuteSignIn()
		{
			IsBusy = true;
			SecureString securePassword = new NetworkCredential("", password).SecurePassword;
			try
			{
				UserContext userContext = await B2CAuthenticationService.Instance.GetTokenForWebApiUsingUsernamePasswordAsync(UserName, securePassword);
				if (userContext.AccessToken != null)
				{
					TokenCache.Set(userContext.AccessToken, userContext.ExpiresOn); // Store the Access Token
					UserSettingsManager.DisplayName = UserName; // Store the users sign in Display Name
					Password = string.Empty; // Clear down the passsword field.
					if (!Remeber)
					{
						UserName = string.Empty;
					}

					SecureSettingsManager.RememberUserName = UserName; // Remember the users user name
					await Shell.Current.GoToAsync("///main");
				}
			}
			catch (MsalClientException ex) when (ex.ErrorCode == "unknown_user_type")
			{
				// Message = "Unsupported User Type 'Unknown'. Please see https://aka.ms/msal-net-up"
				// The user is not recognized as a managed user, or a federated user. Azure AD was not
				// able to identify the IdP that needs to process the user
				await Shell.Current.DisplayAlert("SignIn Error", "User not recognised", "Dismiss");
			}
			catch (MsalClientException ex) when (ex.ErrorCode == "user_realm_discovery_failed")
			{
				// The user is not recognized as a managed user, or a federated user. Azure AD was not
				// able to identify the IdP that needs to process the user. That's for instance the case
				// if you use a phone number
				await Shell.Current.DisplayAlert("SignIn Error", "Wrong username", "Dismiss");
				//throw new ArgumentException("U/P: Wrong username", ex);
			}
			catch (MsalClientException ex) when (ex.ErrorCode == "unknown_user")
			{
				// the username was probably empty
				// ex.Message = "Could not identify the user logged into the OS. See http://aka.ms/msal-net-iwa for details."
				await Shell.Current.DisplayAlert("SignIn Error", "user not identified", "Dismiss");
				//throw new ArgumentException("U/P: Wrong username", ex);
			}
			catch (MsalUiRequiredException)
			{
				await Shell.Current.DisplayAlert("Expired", "Session has expired, please sign out and back in", "Dismiss");
			}
			catch (MsalClientException ex) when (ex.ErrorCode == "unknown_user")
			{
				await Shell.Current.DisplayAlert("Unknown user", "Unknown user", "Dismiss");
			}
			catch (MsalServiceException ex) when (ex.ErrorCode == "access_denied")
			{
				await Shell.Current.DisplayAlert("Access denied", "Incorrect username or password", "Dismiss");
			}
			catch (MsalServiceException ex) when (ex.ErrorCode == "temporarily_unavailable")
			{
				await Shell.Current.DisplayAlert("Temporarily unavailable", "Service is temporarily unavailable.  Try again later.", "Dismiss");
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Exception", ex.Message, "Dismiss");
			}
			finally
			{
				IsBusy = false;
			}
		}
	}
}