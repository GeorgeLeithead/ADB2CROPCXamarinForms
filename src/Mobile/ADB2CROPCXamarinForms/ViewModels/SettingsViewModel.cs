// <copyright file="SettingsViewModel.cs" company="InternetWideWorld.com">
// Copyright (c) George Leithead, InternetWideWorld.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>

namespace ADB2CROPCXamarinForms.ViewModels
{
	using ADB2CROPCXamarinForms.Features.LogOn;
	using ADB2CROPCXamarinForms.Helpers;
	using ADB2CROPCXamarinForms.ViewModels.Base;
	using System.Threading.Tasks;
	using System.Windows.Input;
	using Xamarin.Forms;

	public class SettingsViewModel : ViewModelBase
	{
		public SettingsViewModel()
		{
			if (DesignMode.IsDesignModeEnabled)
			{
				return;
			}

			IsBusy = true;
			Title = "Settings";
			SignOutCommand = new Command(async () => await SignOut());
			IsBusy = false;
		}

		public ICommand SignOutCommand { get; set; }

		private async Task SignOut()
		{
			TokenCache.Clear(); // Clear any cached access cache token
			await B2CAuthenticationService.Instance.SignOutAsync();
			await Shell.Current.GoToAsync("///SignIn");
		}
	}
}