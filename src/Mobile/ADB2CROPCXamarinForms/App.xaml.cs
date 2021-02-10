// <copyright file="App.xaml.cs" company="InternetWideWorld.com">
// Copyright (c) George Leithead, InternetWideWorld.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>

namespace ADB2CROPCXamarinForms
{
	using ADB2CROPCXamarinForms.Features.LogOn;
	using ADB2CROPCXamarinForms.Interfaces;
	using Xamarin.Forms;

	public partial class App : Application
	{
		public static object UIParent { get; set; } = null;

		public App()
		{
			InitializeComponent();
			DependencyService.Register<B2CAuthenticationService>();
			DependencyService.Resolve<IDialogService>();

			MainPage = new AppShell();
		}

		protected override void OnStart()
		{
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}
