using AdministraciondePersonal.Repository;
using AdministraciondePersonal.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddScoped<DbConnectionFactory>();

builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<UsuarioService>();

builder.Services.AddScoped<OferenteRepository>();
builder.Services.AddScoped<OferenteService>();

builder.Services.AddScoped<ConcursoRepository>();
builder.Services.AddScoped<ConcursoService>();

builder.Services.AddScoped<PreparacionAcademicaRepository>();
builder.Services.AddScoped<PreparacionAcademicaService>();

builder.Services.AddScoped<ExperienciaLaboralRepository>();
builder.Services.AddScoped<ExperienciaLaboralService>();

builder.Services.AddScoped<EntrevistaRepository>();
builder.Services.AddScoped<EntrevistaService>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapRazorPages();

app.MapGet("/", context =>
{
    context.Response.Redirect("/Login");
    return Task.CompletedTask;
});

app.Run();