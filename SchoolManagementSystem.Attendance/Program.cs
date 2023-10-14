using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Attendance.Models;
using SchoolManagementSystem.Shared.Auth;
using SchoolManagementSystem.Shared.Exceptions;
using SchoolManagementSystem.Shared.Helpers;
using System.Text.Json;

namespace SchoolManagementSystem.Attendance;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		ConfigureServices(builder.Services, builder.Configuration, builder.Environment);

		var app = builder.Build();
		ConfigureApplication(app);

		app.Run();
	}

	private static void ConfigureServices(
		IServiceCollection services,
		IConfiguration configuration,
		IHostEnvironment environment)
	{
		services.AddDbContext<ApplicationDbContext>(options =>
			options.UseSqlServer(configuration.GetConnectionString("Default")));

		services.AddSingleton(configuration);
		services.AddHttpClient();
		services.AddHttpContextAccessor();
		services.AddScoped<ISchoolHttpClient, SchoolHttpClient>();

		services.AddCors(options =>
		{
			options.AddPolicy(name: "AllowedOrigins",
				builder =>
				{
					builder.WithOrigins(configuration.GetSection("AllowedOrigins").Get<string[]>()).AllowAnyHeader().AllowAnyMethod();
				});
		});

		services.AddControllers()
			.AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
				options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
				options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
			})
			.ConfigureApiBehaviorOptions(options =>
			{
				options.SuppressModelStateInvalidFilter = true;
			});

		services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		}).AddJwtBearer();
		services.AddAuthorization();

		if (environment.IsDevelopment())
			services.AddDatabaseDeveloperPageExceptionFilter();
	}

	private static void ConfigureApplication(WebApplication application)
	{
		if (application.Environment.IsDevelopment())
			application.UseDeveloperExceptionPage();

		if (!application.Environment.IsDevelopment())
			application.UseExceptionHandling();

		application.UseCors("AllowedOrigins");

		application.UseRouting();

		application.UseSchoolAuthentication(application.Configuration["AuthorizationServiceUrl"]);
		application.UseAuthentication();
		application.UseAuthorization();

		application.UseEndpoints(endpoints =>
		{
			endpoints.MapControllerRoute(
				name: "default",
				pattern: "{controller}/{action}/{id?}");
		});
	}
}