using Hangfire;
using Hangfire.MemoryStorage;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using teste.db;
using teste.service;

var builder = WebApplication.CreateBuilder(args);

// Adicione serviços ao contêiner.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<BookServicePostGre>();
builder.Services.AddDbContext<Context>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("HangfireConnection")));

builder.Services.AddHangfire(x => x.UseMemoryStorage());
builder.Services.AddHangfireServer();

builder.Services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UsePostgreSqlStorage(builder.Configuration.GetConnectionString("HangfireConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(acesso => { acesso.AllowAnyHeader(); acesso.AllowAnyMethod(); acesso.AllowAnyOrigin(); });
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseHangfireDashboard();

app.UseAuthorization();

app.MapControllers();

app.Run();


    
        
