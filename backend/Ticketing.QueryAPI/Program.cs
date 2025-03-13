using Ticketing.QueryAPI.Data;
using Ticketing.QueryAPI.KafkaConsumers;
using Ticketing.QueryAPI.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5001); // 讓容器內部監聽 80 埠
});
// 設定 CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton<TicketService>();
builder.Services.AddSingleton<OrderService>();
builder.Services.AddHostedService<TicketConsumer>();
builder.Services.AddHostedService<OrderConsumer>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));
try
{
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    //app.UseAuthorization();
    app.UseCors("AllowAll");
    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"Application failed to start: {ex.Message}");
    throw; // 讓應用程式崩潰並顯示錯誤
}

