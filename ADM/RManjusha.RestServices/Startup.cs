using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using RManjusha.RestServices.Helpers;
using RManjusha.RestServices.Interceptors;
using RManjusha.RestServices.Models;
using RManjusha.RestServices.Models.AuthModels;
using RManjusha.RestServices.Securities;
using System;
using System.IO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Reflection;

namespace RManjusha.RestServices
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            Configuration = configuration;
        }
        private IWebHostEnvironment _hostingEnvironment;
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });
            services.AddControllers(
                options =>
                {
                    // options.AllowEmptyInputInBodyModelBinding = true;
                }).AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        // options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
    }
);
            JwtSettings settings = GetJwtSettings();
            services.AddScoped<RManjushaContext>();
            services.AddSingleton(settings);
            services.AddTransient(typeof(SecurityManager), typeof(SecurityManager));
            services.AddTransient(typeof(BlobStorageService), typeof(BlobStorageService));
            services.AddTransient(typeof(JWTHelper), typeof(JWTHelper));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            // Get JWT Token Settings from JSON file
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = settings.Issuer,
                    ValidAudience = settings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key))
                };

            });

            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });
            services.AddMvc(
                options =>
                {
                    options.SuppressAsyncSuffixInActionNames = false;
                }).AddControllersAsServices();
        }

        public JwtSettings GetJwtSettings()
        {
            JwtSettings settings = new JwtSettings();
            settings.Key =
              Configuration["JwtSettings:key"];
            settings.Audience =
              Configuration["JwtSettings:audience"];
            settings.Issuer =
              Configuration["JwtSettings:issuer"];
            settings.MinutesToExpiration =
              Convert.ToInt32(
                Configuration["JwtSettings:minutesToExpiration"]);
            return settings;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder
     .AllowAnyOrigin()
     .AllowAnyMethod()
     .AllowAnyHeader());
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseMiddleware<JwtMiddleware>();
            app.UseMiddleware<RequestResponseLoggingMiddleware>();

            // get the directory
            var assemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var assetDirectory = Path.Combine(_hostingEnvironment.ContentRootPath, "Upload");

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(assetDirectory),
                RequestPath = "/Assets"
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
