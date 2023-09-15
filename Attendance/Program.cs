namespace Attendance;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		ConfigureServices(builder.Services, builder.Configuration);

		var app = builder.Build();
		ConfigureApplication(app);

		app.Run();
	}

	private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
	{
		services.AddCors(options =>
		{
			options.AddPolicy(name: "AllowedOrigins",
				builder =>
				{
					builder.WithOrigins(configuration.GetSection("AllowedOrigins").Get<string[]>()).AllowAnyHeader().AllowAnyMethod();
				});
		});

		services.AddControllers();
	}

	private static void ConfigureApplication(WebApplication application)
	{
		if (application.Environment.IsDevelopment())
			application.UseDeveloperExceptionPage();

		application.UseCors("AllowedOrigins");

		application.UseRouting();
		application.UseEndpoints(endpoints =>
		{
			endpoints.MapControllerRoute(
				name: "default",
				pattern: "{controller}/{action}/{id?}");
		});
	}
}