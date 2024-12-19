using ChristmasTree;
using ChristmasTree.Data;
using ChristmasTree.Services.Factory;
using ChristmasTree.Services.Services;
using ChristmasTree.Services.Token;
using ChristmasTree.Services.Verifier;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EntityContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<TokenAccessor>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<LightFactory>();
builder.Services.AddScoped<LightService>();
builder.Services.AddHttpClient<LightVerifier>(client =>
{
    client.BaseAddress = new Uri("https://polygon.gsk567.com");
});

builder.Services.AddControllersWithViews();
builder.Services.AddMvc();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "_dbztPolicy",
        policy =>
        {
            policy.WithOrigins("https://codingburgas.karagogov.com")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

await app.PrepareAsync();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseCors("_dbztPolicy");

app.UseAuthorization();

app.MapControllers();
/*app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");*/

app.Run();