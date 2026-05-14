using Microsoft.AspNetCore.Authentication.Cookies;
using ProjetoLogin.Repositorio;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => {
        options.LoginPath = "/Usuario/Logar"; // Onde o usuário cai se não estiver logado
        options.AccessDeniedPath = "/Usuario/AcessoNegado"; // Se não tiver permissão
    });

// Registrando a Injeção de Dependência
builder.Services.AddScoped<IProdutoRepositorio, ProdutoRepositorio>();
builder.Services.AddScoped<ILoginRepositorio, LoginRepositorio>();


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

//IMPORTANTE ADICIONAR
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
