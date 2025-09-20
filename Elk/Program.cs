
#region SeriLog
using Elk.Helper;
using Elk.Interface;
using Elk.Service;
using Hangfire;
using Hangfire.SqlServer;
using Serilog;
using Serilog.Events;

var logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File(
        path: "Logs/info-.log",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 5,
        rollOnFileSizeLimit: true,
        fileSizeLimitBytes: 500L * 1024 * 1024,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message}{NewLine}{Exception}",
        restrictedToMinimumLevel: LogEventLevel.Information
    )
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(le => le.Level == LogEventLevel.Error)
        .WriteTo.File(
            path: "Logs/error-.log",
            rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: 5,
            rollOnFileSizeLimit: true,
            fileSizeLimitBytes: 500L * 1024 * 1024,
            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message}{NewLine}{Exception}"
        )
    )
    .CreateLogger();

Log.Logger = logger;
#endregion
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<ISchaduleService, SchaduleService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHangfire(configur => configur
              .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
              .UseSimpleAssemblyNameTypeSerializer()
              .UseRecommendedSerializerSettings()
              .UseSqlServerStorage(builder.Configuration.GetConnectionString("Job"), new SqlServerStorageOptions
              {
                  CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                  SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                  QueuePollInterval = TimeSpan.Zero,
                  UseRecommendedIsolationLevel = true,
                  DisableGlobalLocks = true
              }));

builder.Services.AddHangfireServer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseHangfireDashboard("/ManageJob", new DashboardOptions
{
   Authorization = new[] { new HangfireAuthorizationFilter() }
});

#region Jobs
RecurringJob.AddOrUpdate<ISchaduleService>(
    "CreateError Every Hour",
    job => job.CreateError(),
    Cron.Hourly
);
#endregion
app.Run();
