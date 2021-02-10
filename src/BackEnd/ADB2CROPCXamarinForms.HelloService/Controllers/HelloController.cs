// <copyright file="HelloController.cs" company="InternetWideWorld.com">
// Copyright (c) George Leithead, InternetWideWorld.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>

namespace ADB2CROPCXamarinForms.HelloService.Controllers
{
	using System;
	using System.Net.Mime;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;

	/// <summary>Hello controller.</summary>
	[Route("api/[controller]")]
	[Produces("application/json")]
	public class HelloController : Controller
	{
		/// <summary>Get action.</summary>
		/// <returns>Hello response.</returns>
		[HttpGet]
		[Authorize(Policy = "FilesReadScope")]
		[Consumes(MediaTypeNames.Application.Json)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public IActionResult Get()
		{
			string jsonResponse = @"{'Hello': true, 'Authenticated': true}";
			return this.Ok(jsonResponse);
		}

		/// <summary>Alive action.</summary>
		/// <returns>Current UTC datetime.</returns>
		[HttpGet]
		[Route("Alive")]
		[Consumes(MediaTypeNames.Application.Json)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public IActionResult GetAlive()
		{
			return this.Ok(DateTime.UtcNow);
		}
	}
}