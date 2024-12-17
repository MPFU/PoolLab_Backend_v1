using PoolLab.Application.ServiceExtension;
using PoolLab.Infrastructure.Firebase;
using PoolLab.WebAPI.Hubs;
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

// Thêm route cho SignalR
app.MapHub<NotificationHub>("/notificationHub");

app.UseCors(opt =>
{
    opt.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:3000");
    opt.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:8081");
});


app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
