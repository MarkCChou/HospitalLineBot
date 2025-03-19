
using HospitalLineBot.Data;
using HospitalLineBot.Mappings;
using HospitalLineBot.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace HospitalLineBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<HospitalLineBotDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("HospitalLineBotConnectionString")));


            builder.Services.AddScoped<IAppointmentRepository, SQLAppointmentRepository>();

            builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            // Using Files
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider =
                    new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "UploadFiles")),
                RequestPath = "/UploadFiles",

            });
            //





            app.MapControllers();

            app.Run();
        }
    }
}
