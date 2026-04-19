using beheersysteem_uitvaartcentrum.backend.api.Filters;
using beheersysteem_uitvaartcentrum.backend.api.Middleware;
using beheersysteem_uitvaartcentrum.backend.application.Interfaces.Repositories;
using beheersysteem_uitvaartcentrum.backend.application.Interfaces.Services;
using beheersysteem_uitvaartcentrum.backend.application.Services;
using beheersysteem_uitvaartcentrum.backend.infrastructure.Data;
using beheersysteem_uitvaartcentrum.backend.infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Threat #25/#32 - XSS mitigatie: globale sanitatie filter voor alle inkomende requests
builder.Services.AddScoped<SanitizeInputFilter>();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<SanitizeInputFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IDossierRepository, DossierRepository>();
builder.Services.AddScoped<IDossierService, DossierService>();
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
builder.Services.AddScoped<IFileStorageProvider, FileStorageProvider>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins(builder.Configuration["Cors:AllowedOrigins"]!)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        context.Database.Migrate();

        DbInitializer.Fixture(context);
    }
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("Frontend");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
