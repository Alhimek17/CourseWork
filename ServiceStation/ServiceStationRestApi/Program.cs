using Microsoft.OpenApi.Models;
using ServiceStationBusinessLogic.BusinessLogics;
using ServiceStationBusinessLogic.MailWorker;
using ServiceStationBusinessLogic.OfficePackage;
using ServiceStationBusinessLogic.OfficePackage.Implements;
using ServiceStationContracts.BindingModels;
using ServiceStationContracts.BusinessLogicsContracts;
using ServiceStationContracts.StoragesContracts;
using ServiceStationDatabaseImplement.Implements;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Logging.AddLog4Net("log4net.config");

builder.Services.AddTransient<ICarStorage, CarStorage>();
builder.Services.AddTransient<IDefectStorage, DefectStorage>();
builder.Services.AddTransient<IExecutorStorage, ExecutorStorage>();
builder.Services.AddTransient<ITechnicalWorkStorage, TechnicalWorkStorage>();

builder.Services.AddTransient<IGuarantorStorage, GuarantorStorage>();
builder.Services.AddTransient<ISparePartStorage, SparePartStorage>();
builder.Services.AddTransient<IRepairStorage, RepairStorage>();
builder.Services.AddTransient<IWorkStorage, WorkStorage>();

builder.Services.AddTransient<ICarLogic, CarLogic>();
builder.Services.AddTransient<IDefectLogic, DefectLogic>();
builder.Services.AddTransient<IExecutorLogic, ExecutorLogic>();
builder.Services.AddTransient<ITechnicalWorkLogic, TechnicalWorkLogic>();
builder.Services.AddTransient<IExecutorReportLogic, ExecutorReportLogic>();
builder.Services.AddTransient<IGuarantorReportLogic, GuarantorReportLogic>();

builder.Services.AddTransient<ISparePartLogic, SparePartLogic>();
builder.Services.AddTransient<IGuarantorLogic, GuarantorLogic>();
builder.Services.AddTransient<IWorkLogic, WorkLogic>();
builder.Services.AddTransient<IRepairLogic, RepairLogic>();

builder.Services.AddTransient<AbstractSaveToExcelExecutor, SaveToExcelExecutor>();
builder.Services.AddTransient<AbstractSaveToWordExecutor, SaveToWordExecutor>();
builder.Services.AddTransient<AbstractSaveToPdfExecutor, SaveToPdfExecutor>();

builder.Services.AddTransient<AbstractSaveToExcelGuarantor, SaveToExcelGuarantor>();
builder.Services.AddTransient<AbstractSaveToWordGuarantor, SaveToWordGuarantor>();
builder.Services.AddTransient<AbstractSaveToPdfGuarantor, SaveToPdfGuarantor>();

builder.Services.AddSingleton<AbstractMailWorker, MailKitWorker>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ServiceStation",
        Version = "v1"
    });
});

var app = builder.Build();

var mailSender = app.Services.GetService<AbstractMailWorker>();

mailSender?.MailConfig(new MailConfigBindingModel
{
    MailLogin = builder.Configuration?.GetSection("MailLogin")?.Value?.ToString() ?? string.Empty,
    MailPassword = builder.Configuration?.GetSection("MailPassword")?.Value?.ToString() ?? string.Empty,
    SmtpClientHost = builder.Configuration?.GetSection("SmtpClientHost")?.Value?.ToString() ?? string.Empty,
    SmtpClientPort = Convert.ToInt32(builder.Configuration?.GetSection("SmtpClientPort")?.Value?.ToString()),
    PopHost = builder.Configuration?.GetSection("PopHost")?.Value?.ToString() ?? string.Empty,
    PopPort = Convert.ToInt32(builder.Configuration?.GetSection("PopPort")?.Value?.ToString())
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ServiceStation v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();