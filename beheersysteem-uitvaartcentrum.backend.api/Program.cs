using beheersysteem_uitvaartcentrum.backend.application.Interfaces.Repositories;
using beheersysteem_uitvaartcentrum.backend.application.Interfaces.Services;
using beheersysteem_uitvaartcentrum.backend.application.Services;
using beheersysteem_uitvaartcentrum.backend.infrastructure.Data;
using beheersysteem_uitvaartcentrum.backend.infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IDossierRepository, DossierRepository>();
builder.Services.AddScoped<IDossierService, DossierService>();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();