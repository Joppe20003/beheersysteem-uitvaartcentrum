using beheersysteem_uitvaartcentrum.backend.api.Filters;
using beheersysteem_uitvaartcentrum.backend.api.Middleware;
using beheersysteem_uitvaartcentrum.backend.application.Interfaces.Repositories;
using beheersysteem_uitvaartcentrum.backend.application.Interfaces.Services;
using beheersysteem_uitvaartcentrum.backend.application.Security;
using beheersysteem_uitvaartcentrum.backend.application.Services;
using beheersysteem_uitvaartcentrum.backend.infrastructure.Data;
using beheersysteem_uitvaartcentrum.backend.infrastructure.Repositories;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});
builder.Services.Configure<AntiforgeryOptions>(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 1024L * 1024L * 512L; // 500 MB
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 1024L * 1024L * 512L; // 500 MB
});

// Threat #25/#32 - XSS mitigatie: globale sanitatie filter voor alle inkomende requests
builder.Services.AddScoped<SanitizeInputFilter>();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<SanitizeInputFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("AppConnection"))
);
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("AuthConnection"))
);

builder.Services.AddScoped<IDossierRepository, DossierRepository>();
builder.Services.AddScoped<IDossierService, DossierService>();
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
builder.Services.AddScoped<IFileStorageProvider, FileStorageProvider>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 16;

    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ";
})
.AddEntityFrameworkStores<AuthDbContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["X-Access-Token"];
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddSingleton<IAuthorizationHandler, DossierAccessAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, DossierCreateAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, DossierInviteAuthorizationHandler>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("DossierAccess", policy =>
        policy.Requirements.Add(new DossierAccessRequirement()));
    options.AddPolicy("DossierCreate", policy =>
        policy.Requirements.Add(new DossierCreateRequirement()));
    options.AddPolicy("DossierInvite", policy =>
        policy.Requirements.Add(new DossierInviteRequirement()));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins(builder.Configuration["Cors:AllowedOrigins"]!)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var appContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var authContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();


    appContext.Database.Migrate();
    authContext.Database.Migrate();

    DbInitializer.Seed(authContext);

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        DbInitializer.Fixture(userManager, appContext);
    }
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("Frontend");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();