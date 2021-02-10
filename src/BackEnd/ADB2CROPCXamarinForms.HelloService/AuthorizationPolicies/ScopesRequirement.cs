// <copyright file="ScopesRequirement.cs" company="InternetWideWorld.com">
// Copyright (c) George Leithead, InternetWideWorld.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>

namespace ADB2CROPCXamarinForms.HelloService.AuthorizationPolicies
{
	using System.Linq;
	using System.Security.Claims;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.Identity.Web;

	/// <summary>Requirement used in authorization policies, to check if the scope claim has at least one of the requirement values. Since the class also extends AuthorizationHandler, its dependency injection is done out of the box.</summary>
	public class ScopesRequirement : AuthorizationHandler<ScopesRequirement>, IAuthorizationRequirement
	{
		private readonly string[] acceptedScopes;

		/// <summary>Initialises a new instance of the <see cref="ScopesRequirement"/> class.</summary>
		/// <param name="acceptedScopesRequirement">Accepted scopes requirement.</param>
		public ScopesRequirement(params string[] acceptedScopesRequirement)
		{
			this.acceptedScopes = acceptedScopesRequirement;
		}

		/// <summary>AuthorizationHandler that will check if the scope claim has at least one of the requirement values.</summary>
		/// <param name="context">Authorisation handler context.</param>
		/// <param name="requirement">Scopes requirement.</param>
		/// <returns>Task completed.</returns>
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ScopesRequirement requirement)
		{
			// If there are no scopes, do not process
			if (!context.User.Claims.Any(x => x.Type == ClaimConstants.Scope) && !context.User.Claims.Any(y => y.Type == ClaimConstants.Scp))
			{
				return Task.CompletedTask;
			}

			Claim scopeClaim = context?.User?.FindFirst(ClaimConstants.Scp);

			if (scopeClaim == null)
			{
				scopeClaim = context?.User?.FindFirst(ClaimConstants.Scope);
			}

			if (scopeClaim != null && scopeClaim.Value.Split(' ').Intersect(requirement.acceptedScopes).Any())
			{
				context.Succeed(requirement);
			}

			return Task.CompletedTask;
		}
	}
}