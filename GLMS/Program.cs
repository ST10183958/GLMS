
using GLMS.Web.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddHttpClient("GLMSApi", client =>
{
    client.BaseAddress =
        new Uri("http://localhost:5099/");
});



builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpClient<ICurrencyService, CurrencyService>();

builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IContractRulesService, ContractRulesService>();

builder.Services.AddScoped<
    IContractsApiService,
    ContractsApiService>();

builder.Services.AddScoped<
    IClientsApiService,
    ClientsApiService>();

builder.Services.AddScoped<
    IClientsApiService,
    ClientsApiService>();

builder.Services.AddScoped<
    IServiceRequestsApiService,
    ServiceRequestsApiService>();

builder.Services.AddScoped<
    IContractsApiService,
    ContractsApiService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();