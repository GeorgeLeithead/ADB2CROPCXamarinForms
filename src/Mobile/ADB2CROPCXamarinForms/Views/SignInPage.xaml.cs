// <copyright file="SignInPage.xaml.cs" company="InternetWideWorld.com">
// Copyright (c) George Leithead, InternetWideWorld.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>

namespace ADB2CROPCXamarinForms.Views
{
	using ADB2CROPCXamarinForms.Helpers;
	using Xamarin.Forms;
	using Xamarin.Forms.Xaml;

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SignInPage : ContentPage
	{
		public SignInPage()
		{
			InitializeComponent();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			CheckAuth();
		}

		private async void CheckAuth()
		{
			if (!TokenCache.IsExpired)
			{
				await Shell.Current.GoToAsync("///main");
			}

			base.OnAppearing();
		}

		protected override bool OnBackButtonPressed() => false;
	}
}