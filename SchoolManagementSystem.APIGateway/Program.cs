using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

if (allowedOrigins != null && allowedOrigins.Any())
{
	builder.Services.AddCors(options =>
	{
		options.AddPolicy(name: "AllowedOrigins",
			config =>
			{
				config.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod();
			});
	});
}


var app = builder.Build();

if (allowedOrigins != null && allowedOrigins.Any())
	app.UseCors("AllowedOrigins");

await app.UseOcelot();

app.UseRouting();

app.Run();