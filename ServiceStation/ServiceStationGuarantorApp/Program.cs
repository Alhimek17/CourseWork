using ServiceStationBusinessLogic.BusinessLogics;
using ServiceStationBusinessLogic.OfficePackage;
using ServiceStationBusinessLogic.OfficePackage.Implements;
using ServiceStationContracts.BusinessLogicsContracts;
using ServiceStationContracts.StoragesContracts;
using ServiceStationDatabaseImplement.Implements;
using ServiceStationGuarantorApp;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IGuarantorReportLogic, GuarantorReportLogic>();
builder.Services.AddTransient<IDefectStorage, DefectStorage>();
builder.Services.AddTransient<ITechnicalWorkStorage, TechnicalWorkStorage>();
builder.Services.AddTransient<IRepairStorage, RepairStorage>();
builder.Services.AddTransient<IWorkStorage, WorkStorage>();
builder.Services.AddTransient<ISparePartStorage, SparePartStorage>();
builder.Services.AddTransient<AbstractSaveToExcelGuarantor, SaveToExcelGuarantor>();
builder.Services.AddTransient<AbstractSaveToPdfGuarantor, SaveToPdfGuarantor>();
builder.Services.AddTransient<AbstractSaveToWordGuarantor, SaveToWordGuarantor>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();
APIGuarantor.Connect(builder.Configuration);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
