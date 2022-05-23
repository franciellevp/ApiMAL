using ApiMAL.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text;

namespace ApiMAL
{
    public class Startup
    {
        public Startup (IConfiguration configuration) {
            Configuration = (IConfigurationRoot)configuration;
        }
        public IConfigurationRoot Configuration { get; }
        public void ConfigureServices (IServiceCollection services) {

            // Configure JSON options.
            services.Configure<JsonOptions>(options => {
                options.SerializerOptions.IncludeFields = true;
            });

            // Add services to the container.
            services.AddControllers();
            services.AddDbContext<AnimeListContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options => {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme {
                    Description = "Authorize to use methods with Bearer Scheme. (Type: \"bearer {token}\")",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                options.OperationFilter<SecurityRequirementsOperationFilter>();
                options.SwaggerDoc("v1", new OpenApiInfo {
                    Title = "Simple MAL API",
                    Version = "v1",
                    Description = "Simple ASP.NET Core Web API to make requests in Users Anime List based on data of `myanimelist.net`"
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                        .GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }
        public void Configure (IApplicationBuilder app) {
            app.UseSwagger();
            app.UseSwaggerUI(x => {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "MAL API V1");

            });

            app.UseRouting();

            app.UseCors();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();
            app.UseEndpoints(x => x.MapControllers());
        }
    }
}