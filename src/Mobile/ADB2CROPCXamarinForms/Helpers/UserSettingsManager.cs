// <copyright file="UserSettingsManager.cs" company="InternetWideWorld.com">
// Copyright (c) George Leithead, InternetWideWorld.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>

namespace ADB2CROPCXamarinForms.Helpers
{
	using Xamarin.Essentials;

	/// <summary>User settings manager.</summary>
	public static class UserSettingsManager
	{
		/// <summary>Gets or sets the users log in display name.</summary>
		public static string DisplayName
		{
			get => Preferences.Get(nameof(DisplayName), string.Empty);
			set => Preferences.Set(nameof(DisplayName), value);
		}
	}
}