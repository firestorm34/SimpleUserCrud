using SimpleUserCrud.Core.Interfaces;
using SimpleUserCrud.Core.Services;
using SimpleUserCrud.Data.Repositories;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

string providerType = builder.Configuration["DataProvider:Type"];
string connectionString = builder.Configuration[$"DataProvider:ConnectionStrings:{providerType}"];

if (providerType == "JsonFile")
{
	builder.Services.AddScoped<IUserRepository>(provider => new UserJsonFileRepository(connectionString));
}
else if (providerType == "MsSql")
{
	builder.Services.AddScoped<IUserRepository>(provider => new UserMsSqlRepository(connectionString));
}

builder.Services.AddScoped<UserService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else 
{
    app.UseExceptionHandler("/Home/Error");
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Users}/{action=Index}/{id?}");

app.Run();
