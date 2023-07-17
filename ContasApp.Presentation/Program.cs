using ContasApp.Presentation.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//configurando e ativando autentica��o por Cookies
builder.Services.Configure<CookiePolicyOptions>
    (options => { options.MinimumSameSitePolicy = SameSiteMode.None; });
builder.Services.AddAuthentication
    (CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseMiddleware<CacheControlFilter>();

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

//Configurando o padr�o de navega��o do projeto /Controllers/Views
//e definindo a p�gina inicial do projeto
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
