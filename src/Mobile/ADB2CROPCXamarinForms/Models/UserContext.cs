// <copyright file="UserContext.cs" company="InternetWideWorld.com">
// Copyright (c) George Leithead, InternetWideWorld.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>

namespace ADB2CROPCXamarinForms.Models
{
	using System;

	/// <summary>User context.</summary>
	/// <remarks>Add additional user properties here.</remarks>
	public class UserContext
	{
		public string AccessToken { get; internal set; }
		public string Audience { get; internal set; }
		public DateTimeOffset ExpiresOn { get; internal set; }
		public bool IsLoggedOn { get; internal set; }
		public string Issuer { get; internal set; }
		public string Subject { get; internal set; }
	}
}