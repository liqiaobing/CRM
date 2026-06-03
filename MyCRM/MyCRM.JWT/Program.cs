using IRepository;
using IService;
using Microsoft.EntityFrameworkCore;
using MyCRM.DataBase;
using MyCRM.Service.AutoMapper;
using Repository;
using Service;

namespace MyCRM.JWT
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

            #region 配置跨域服务
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("cors", p =>
                {
                    p.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });
            #endregion
            #region 数据库字符串连接
            builder.Services.AddDbContext<ApplicationDbcontext>(options =>
            {
                string connStr = builder.Configuration.GetSection("ConnStr").Value;
                options.UseMySql(connStr, ServerVersion.AutoDetect(connStr), b => b.MigrationsAssembly("MyCRM"));
                //options.UseSqlServer(builder.Configuration.GetSection("ConnStr").Value);
            });

            #endregion
            #region 自定义Ioc注入
            builder.Services.AddCustomIoc();
            #endregion
            #region AutoMapper
            builder.Services.AddAutoMapper(typeof(CustomAutoMapperProfile));
            #endregion
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            //启用跨域
            app.UseCors("cors");

            app.UseHttpsRedirection();
            //app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }

    public static class Expansion
    {
        public static IServiceCollection AddCustomIoc(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}