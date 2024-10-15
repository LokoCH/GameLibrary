
using GamesLibrary.Persistence.Extentions;
using GamesLibrary.Application.Extentions;
using GamesLibrary.Api.Middlewares.Exceptions;
using System.Text.Json;

namespace GamesLibrary.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers().
                AddJsonOptions(jsonOptions =>
                {
                    jsonOptions.JsonSerializerOptions.WriteIndented = true;
                    jsonOptions.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services
                .AddApplicationLayer()
                .AddPersistenceLayer(builder.Configuration);

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();

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
