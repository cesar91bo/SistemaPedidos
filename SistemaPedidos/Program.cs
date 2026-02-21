using Microsoft.EntityFrameworkCore;
using SistemaPedidos.Application.Features.Categorias.Interfaces;
using SistemaPedidos.Application.Features.Pedidos.Interfaces;
using SistemaPedidos.Application.Features.Productos.Interfaces;
using SistemaPedidos.Components;
using SistemaPedidos.Infrastructure.Persistence;
using SistemaPedidos.Infrastructure.Services.Categorias;
using SistemaPedidos.Infrastructure.Services.Pedidos;
using SistemaPedidos.Infrastructure.Services.Productos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
