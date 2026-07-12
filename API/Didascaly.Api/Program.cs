using Didascaly.Api.Services;
using Didascaly.Api.Hubs;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

builder.Services.AddSingleton<Didascaly.Core.Interfaces.IPlayRepository, PlayRepository>();
builder.Services.AddSingleton<RoomManager>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMobileApp", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .SetIsOriginAllowed((string host) => true) 
              .AllowCredentials();
    });
});

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowMobileApp");

app.UseAuthorization();
app.MapControllers();

app.MapHub<TheaterHub>("/hubs/theater");

app.Run();