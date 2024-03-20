using Microsoft.EntityFrameworkCore;
using NLog;
using Repositories.EFCore;
using Services.Contracts;
using WebAPI.Extensions;


namespace WebAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

			builder.Services.AddControllers(config =>
			{
				config.RespectBrowserAcceptHeader = true; //i�erik pazarl��� konfig�rasyonu defaultu false pazarl��a a��ksak true ya �ekmemiz gerek
				config.ReturnHttpNotAcceptable = true; //Kabul etmedi�imiz formatta bir taleple kar��la�t���m�zda bunu istemciyle payla��yoruz (406 not acceptable olarak)
			}).AddCustomCsvFormatter() //ayarlar� utilities ve extensions da
			.AddXmlDataContractSerializerFormatters() //xml format�nda da ��kt� vermek i�in ekledik
			.AddApplicationPart(typeof(Presentation.AssemblyRefence).Assembly).AddNewtonsoftJson();

			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			//Extensions
			builder.Services.ConfigureSqlContext(builder.Configuration);
			builder.Services.ConfigureRepositoryManager();
			builder.Services.ConfigureServiceManager();
			builder.Services.ConfigureLoggerService();

			//Automapper

			builder.Services.AddAutoMapper(typeof(Program));

			var app = builder.Build();

			//Exception Config
			var logger=app.Services.GetRequiredService<ILoggerService>();
			app.ConfigureExceptionHandler(logger);

			if (app.Environment.IsProduction())
			{
				app.UseHsts();
			}



			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
