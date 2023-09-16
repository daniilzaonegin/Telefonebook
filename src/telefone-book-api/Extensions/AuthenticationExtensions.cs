namespace telefone_book_api.Extensions;

public static class AuthenticationExtensions
{
    public static WebApplicationBuilder AddAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer(opt =>
        {
            opt.TokenValidationParameters.NameClaimType = "name";
            opt.TokenValidationParameters.RoleClaimType = "role";
        });
        //.AddGoogle(googleOptions =>
        //{
        //    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
        //    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
        //});

        return builder;
    }
}
