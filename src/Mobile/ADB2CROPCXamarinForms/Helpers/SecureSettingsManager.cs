// <copyright file="SecureSettingsManager.cs" company="InternetWideWorld.com">
// Copyright (c) George Leithead, InternetWideWorld.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>

namespace ADB2CROPCXamarinForms.Helpers
{
	using System;
	using Xamarin.Essentials;

	public static class SecureSettingsManager
	{
		private readonly static string RememberMeKey = "rememberme";
		private readonly static string UserNameKey = "username";

		public static string RememberUserName
		{
			get => SecureStorage.GetAsync(UserNameKey).Result;
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					SecureStorage.Remove(UserNameKey);
				}
				else
				{
					SecureStorage.SetAsync(UserNameKey, value); // Remember the users user name
				}
			}
		}

		public static bool RememberMe
		{
			get => Convert.ToBoolean(SecureStorage.GetAsync(RememberMeKey).Result);
			set
			{
				SecureStorage.SetAsync(RememberMeKey, value.ToString());
			}
		}
	}
}
