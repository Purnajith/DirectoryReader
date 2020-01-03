using System.Text;
using DirectoryReader.Api.Infrastructure.Context;
using DirectoryReader.Api.Infrastructure.Model;
using DirectoryReader.Api.Repositories;
using DirectoryReader.Api.Repositories.Directory;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DirectoryReader.Api
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<AppSettings>(Configuration);
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			services.Configure<AppSettings>(Configuration);

			services.AddTransient<IContentContext, ContentContext>();


			services.AddTransient<IRepository<ContentModel>, ContentRepository>();


			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)  
			.AddJwtBearer(options =>  
			{  
				options.TokenValidationParameters = new TokenValidationParameters  
				{  
					ValidateIssuer = true,  
					ValidateAudience = true,  
					ValidateLifetime = true,  
					ValidateIssuerSigningKey = true,  
					ValidIssuer = Configuration["Jwt:Issuer"],  
					ValidAudience = Configuration["Jwt:Issuer"],  
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))  
				};  
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}

			app.UseAuthentication();
			app.UseMvc();
		}
	}
}
