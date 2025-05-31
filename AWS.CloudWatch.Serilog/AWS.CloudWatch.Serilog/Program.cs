using Amazon.CloudWatchLogs;
using Serilog;
using Serilog.Sinks.AwsCloudWatch;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var options = new CloudWatchSinkOptions
{
    LogGroupName = "/log/demo",
    TextFormatter = new Serilog.Formatting.Json.JsonFormatter(),
    MinimumLogEventLevel = Serilog.Events.LogEventLevel.Debug,
    CreateLogGroup = true,
    LogStreamNameProvider = new DefaultLogStreamProvider()
};

var client = new AmazonCloudWatchLogsClient(Amazon.RegionEndpoint.USEast1);

builder.Host.UseSerilog((ctx, cfg) =>
{
    cfg
        .WriteTo.Console()
        .WriteTo.AmazonCloudWatch(options, client);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
