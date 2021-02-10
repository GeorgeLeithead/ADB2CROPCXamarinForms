// <copyright file="Startup.cs" company="InternetWideWorld.com">
// Copyright (c) George Leithead, InternetWideWorld.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>

namespace ADB2CROPCXamarinForms.HelloService
{
	using ADB2CROPCXamarinForms.HelloService.AuthorizationPolicies;
	using Microsoft.AspNetCore.Authentication.JwtBearer;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Identity.Web;
	using Microsoft.OpenApi.Models;

	/// <summary>Service startup where services required by the app are configured and the request handling pipeline is defined as a series of middleware components.</summary>
	public class Startup
	{
		/// <summary>Initialises a new instance of the <see cref="Startup" /> class.</summary>
		/// <param name="configuration">Service configuration.</param>
		public Startup(IConfiguration configuration)
		{
			this.Configuration = configuration;
		}

		/// <summary>Gets the service configuration.</summary>
		public IConfiguration Configuration { get; }

		/// <summary>This method gets called by the runtime. Use this method to configure the HTTP request pipeline.</summary>
		/// <param name="app">Application builder.</param>
		/// <param name="env">Web host environment.</param>
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ADB2CROPCXamarinForms v1"));
			}
			else
			{
				app.UseHsts();
			}

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}

		/// <summary>This method gets called by the runtime. Use this method to add services to the container.</summary>
		/// <param name="services">Service collection.</param>
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton(this.Configuration);

			// Adds Microsoft Identity platform (AAD v2.0) support to protect this Api
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
					.AddMicrosoftIdentityWebApi(
						options =>
							{
								this.Configuration.Bind("AzureAdB2C", options);
								options.TokenValidationParameters.NameClaimType = "name";
							},
						options => { this.Configuration.Bind("AzureAdB2C", options); });

			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "ADB2CROPCXamarinForms", Version = "v1" });
			});

			services.AddAuthorization(options =>
			{
				// Create policy to check for the scope 'Files.Read'
				options.AddPolicy("FilesReadScope", policy => policy.Requirements.Add(new ScopesRequirement("access_as_user")));
			});
		}
	}
}