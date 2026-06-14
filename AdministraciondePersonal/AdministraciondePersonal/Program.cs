using AdministraciondePersonal.Repository;
using AdministraciondePersonal.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddSingleton<DbConnectionFactory>();

builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped(provider =>
    UsuarioService.GetInstance(
        provider.GetRequiredService<UsuarioRepository>(),
        provider.GetRequiredService<BitacoraService>()
    ));

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

builder.Services.AddScoped<BitacoraRepository>();
builder.Services.AddScoped(provider =>
    BitacoraService.GetInstance(
        provider.GetRequiredService<BitacoraRepository>()
    ));

builder.Services.AddScoped<InstitucionEducativaRepository>();
builder.Services.AddScoped<InstitucionEducativaService>();

builder.Services.AddScoped<RolService>();
builder.Services.AddScoped<RolRepository>();

builder.Services.AddScoped<ParametroRepository>();
builder.Services.AddScoped<ParametroService>();

builder.Services.AddScoped<PantallaRepository>();
builder.Services.AddScoped<PantallaService>();

builder.Services.AddScoped<CompaniaRepository>();
builder.Services.AddScoped<CompaniaService>();

builder.Services.AddDistributedMemoryCache();

// TODO:
// El tiempo de sesión deberá obtenerse posteriormente desde la tabla PARAMETROS
// utilizando un parámetro como TIEMPO_EXPIRACION_SESION.
const int TiempoSesionMinutos = 5;

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(TiempoSesionMinutos);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.MaxAge = TimeSpan.FromMinutes(TiempoSesionMinutos);
});

builder.Services.AddHttpContextAccessor();

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