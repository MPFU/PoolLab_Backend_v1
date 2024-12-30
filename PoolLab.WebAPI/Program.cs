using PoolLab.Application.ServiceExtension;
using PoolLab.Infrastructure.Firebase;
using PoolLab.Infrastructure.Hubs;
using PoolLab.WebAPI.WebAPIExtension;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDIServices(builder.Configuration);
builder.Services.AddDIWebAPI(builder.Configuration);
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.AppendTrailingSlash = false;
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Cấu hình dịch vụ SignalR
builder.Services.AddSignalR();

// Khởi tạo firebase
FirebaseConfiguration.InitializeFirebase();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "tutor_server.API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseCors(opt =>
{
    opt.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:3000");
    opt.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:8081");
    opt.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:5002");
});



app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// Thêm route cho SignalR
app.MapHub<NotificationHub>("notificationHub");
Console.WriteLine("SignalR hub is mapped to /notificationHub");

app.Run();
