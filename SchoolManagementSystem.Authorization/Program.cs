using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SchoolManagementSystem.Authorization.Models;
using SchoolManagementSystem.Authorization.Services;
using System.Security.Claims;
using System.Text;

namespace SchoolManagementSystem.Authorization;

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

		services.AddCors(options =>
		{
			options.AddPolicy(name: "AllowedOrigins",
				builder =>
				{
					builder.WithOrigins(configuration.GetSection("AllowedOrigins").Get<string[]>()).AllowAnyHeader()
						.AllowAnyMethod();
				});
		});

		services.AddControllers()
			.AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
			})
			.ConfigureApiBehaviorOptions(options =>
			{
				options.SuppressModelStateInvalidFilter = true;
			});

		services.AddScoped<IUserStore<ApplicationUser>, CustomUserStore>();
		services.AddScoped<IAccountsService, AccountService>();

		services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
			{
				options.SignIn.RequireConfirmedAccount = false;

				options.Password.RequireDigit = true;
				options.Password.RequireLowercase = true;
				options.Password.RequireNonAlphanumeric = true;
				options.Password.RequireUppercase = true;
				options.Password.RequiredLength = 8;

				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
			})
			.AddRoles<ApplicationRole>()
			.AddEntityFrameworkStores<ApplicationDbContext>()
			.AddDefaultTokenProviders();

		services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
		var key = Encoding.ASCII.GetBytes(configuration["JwtSettings:Secret"]);
		var refreshTokenValidationParams = new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(key),
			ValidIssuer = configuration["JwtSettings:Issuer"],
			ValidateIssuer = true,
			ValidAudience = configuration["JwtSettings:Audience"],
			ValidateAudience = true,
			ValidateLifetime = false,
			RequireExpirationTime = true,
			ClockSkew = TimeSpan.Zero
		};
		services.AddSingleton(refreshTokenValidationParams);

		services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(jwt =>
			{
				jwt.SaveToken = true;
				jwt.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidIssuer = configuration["JwtSettings:Issuer"],
					ValidateIssuer = true,
					ValidAudience = configuration["JwtSettings:Audience"],
					ValidateAudience = true,
					ValidateLifetime = true,
					RequireExpirationTime = true,
					RoleClaimType = ClaimTypes.Role,
					ClockSkew = TimeSpan.Zero
				};
			});


		if (environment.IsDevelopment())
			services.AddDatabaseDeveloperPageExceptionFilter();
	}

	private static void ConfigureApplication(WebApplication application)
	{
		if (application.Environment.IsDevelopment())
			application.UseDeveloperExceptionPage();

		application.UseCors("AllowedOrigins");

		application.UseRouting();

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