using AutoMapper;
using IRepository;
using IService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyCRM.Common;
using MyCRM.DataBase;
using MyCRM.Models;
using MyCRM.Service.AutoMapper;
using MyCRM.Token;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Repository;
using Service;
using System.Text;

namespace MyCRM
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
            builder.Services.AddSwaggerGen(s =>
            {
                #region Swagger 使用鉴权组件
                s.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Description = "直接在下面输入框中输入Bearer {token} (注意两者直接是空格)",
                    Name = "Authorization",
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                s.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme{
                            Reference = new OpenApiReference {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"}
                        },new string[] { }
                    }
                });
                #endregion
            });

            #region 加载配置文件到配置类 
            // 绑定配置到类
            builder.Services.Configure<SystemItem>(builder.Configuration.GetSection("SystemItem"));
            #endregion

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


            #region 添加过滤器
            builder.Services.Configure<MvcOptions>(opt =>
            {
                opt.Filters.Add<JWTValidationFilter>();
            });
            #endregion
            #region 数据库字符串连接
            builder.Services.AddDbContext<ApplicationDbcontext>(options =>
            {
                string connStr = builder.Configuration.GetSection("ConnStr").Value;
                options.UseMySql(connStr, ServerVersion.AutoDetect(connStr), b => b.MigrationsAssembly("MyCRM"));
                //options.UseSqlServer(builder.Configuration.GetSection("ConnStr").Value, m => m.MigrationsAssembly("MyCRM"));
            });
            #endregion
            #region 自定义Ioc注册
            builder.Services.AddCustomIoc();
            #endregion
            #region JWT鉴权
            builder.Services.AddCustomJWT();
            #endregion
            #region AutoMapper
            builder.Services.AddAutoMapper(typeof(CustomAutoMapperProfile));
            #endregion
            #region json日期格式化
            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                //首字母大写
                //options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                //时区
                //options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                //统一设置日期格式
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });
            #endregion

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            #region 异常拦截
            app.UseExceptionHandler(configure =>
            {
                configure.Run(async context =>
                {
                    var exHeader = context.Features.Get<IExceptionHandlerPathFeature>();
                    var exception = context.Features.Get<IExceptionHandlerFeature>();
                    var ex = exHeader.Error;
                    /*if (exception.Error.InnerException.Message.Contains("Cannot delete or update a parent row: a foreign key constraint fails"))
                    {
                        await context.Response.WriteAsJsonAsync(new { code = 500, errPath = exHeader.Path, msg = "删除数据失败，请检查是否有被依赖数据" });
                    }
                    else */if (ex != default)
                    {
                        await context.Response.WriteAsJsonAsync(new { code = 500, errPath = exHeader.Path, msg = "服务器内部错误" });
                    }
                });
            });
            #endregion

            //启用跨域
            app.UseCors("cors");
            //app.UseCors("AllowAnyOrigin");

            app.UseHttpsRedirection();

            //先鉴权
            app.UseAuthentication();
            //再授权
            app.UseAuthorization();
            app.MapControllers();

            //开启静态文件
            app.UseStaticFiles();

            app.Run();
        }
    }

    public static class Expansion
    {

        /// <summary>
        /// Ioc注册
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomIoc(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IActivityRepository, ActivityRepository>();
            services.AddScoped<IActivityService, ActivityService>();
            services.AddScoped<IRemarkRepository, RemarkRepository>();
            services.AddScoped<IRemarkService, RemarkService>();
            services.AddScoped<IClueRepository, ClueRepository>();
            services.AddScoped<IClueService, ClueService>();
            services.AddScoped<IDicValueRepository, DicValueRepository>();
            services.AddScoped<IDicValueService, DicValueService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IPermissionService, PermissionService>();
            return services;
        }
        /// <summary>
        /// JWT 鉴权 Token
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomJWT(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,//是否验证签名,不验证的画可以篡改数据，不安全
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SDMC-CJAS1-SAD-DFSFA-ASDHJVF-VFFFFF")),//解密的密钥
                    ValidateIssuer = true,  //是否验证发行人，就是验证载荷中的Iss是否对应ValidIssuer参数  
                                            //issuer代表颁发token的web应用程序
                    ValidIssuer = "https://localhost:7257;http://localhost:5004",   //发行人
                                                                                    //ValidIssuer = "https://localhost:7257",
                    ValidateAudience = true,    //是否验证订阅人，就是验证载荷中的Aud是否对应ValidAudience参数
                                                //audience是token的受理者
                    ValidAudience = "https://localhost:7075;http://localhost:5092", //订阅人
                                                                                    //ValidAudience = "https://localhost:7075",
                    ValidateLifetime = true,    //是否验证过期时间，过期了就拒绝访问
                    ClockSkew = TimeSpan.FromMinutes(60),   // 这个是缓冲过期时间，也就是说，即使我们配置了过期时间，这里也要考虑进去，过期时间 + 缓冲，默认好像是7分钟，你可以直接设置为
                };
            });
            return services;
        }
    }
}

