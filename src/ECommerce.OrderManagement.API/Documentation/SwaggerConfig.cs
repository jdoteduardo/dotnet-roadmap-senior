using Microsoft.OpenApi.Models;
using TarefaAPI.Documentation;

namespace ECommerce.OrderManagement.API.Documentation
{
    public static class SwaggerConfig
    {
        public static void AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "JWT Authorization header using the Bearer scheme",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                };

                c.AddSecurityDefinition("Bearer", securityScheme);

                c.OperationFilter<AuthorizeCheckOperationFilter>();
            });
        }
    }
}
