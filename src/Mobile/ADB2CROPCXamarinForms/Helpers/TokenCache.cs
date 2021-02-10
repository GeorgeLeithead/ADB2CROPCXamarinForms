// <copyright file="TokenCache.cs" company="InternetWideWorld.com">
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

	/// <summary>Provides simple secure storage for users access token.</summary>
	public static class TokenCache
	{
		private const string CacheExpiryName = "AccessTokenExpiry";
		private const string CacheName = "AccessToken";

		/// <summary>Indicates whether the users access token has expired.</summary>
		public static bool IsExpired
		{
			get
			{
				string cacheExpiry = SecureStorage.GetAsync(CacheExpiryName).Result;
				if (string.IsNullOrEmpty(cacheExpiry))
				{
					return true;
				}

				if (Convert.ToDateTime(cacheExpiry) < DateTimeOffset.UtcNow)
				{
					return true;
				}

				return false;
			}
		}

		/// <summary>Clear all tokens.</summary>
		public static void Clear()
		{
			SecureStorage.Remove(CacheExpiryName);
			SecureStorage.Remove(CacheName);
		}

		/// <summary>Gets the users access token.</summary>
		/// <returns>Users access token or empty string if expired.</returns>
		public static string Get()
		{
			if (IsExpired)
			{
				return string.Empty;
			}

			return SecureStorage.GetAsync(CacheName).Result;
		}

		/// <summary>Stores the users access token and sets the expiry.</summary>
		/// <param name="value">The Access token to be stored.</param>
		/// <param name="expireIn">The access token expiry offset.</param>
		public static void Set(string value, DateTimeOffset expireIn)
		{
			if (string.IsNullOrEmpty(value))
			{
				throw new ArgumentNullException(nameof(value), "Invalid token.");
			}

			if (expireIn == null)
			{
				throw new ArgumentNullException(nameof(expireIn), "Invalid expiry.");
			}

			SecureStorage.SetAsync(CacheExpiryName, expireIn.ToString());
			SecureStorage.SetAsync(CacheName, value);
		}

		/// <summary>Gets the expires on date time for the access token.</summary>
		public static DateTime ExpiresOn
		{
			get
			{
				string cacheExpiry = SecureStorage.GetAsync(CacheExpiryName).Result;
				if (string.IsNullOrEmpty(cacheExpiry))
				{
					return DateTime.Now;
				}

				return Convert.ToDateTime(cacheExpiry);
			}
		}
	}
}