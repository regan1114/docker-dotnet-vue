using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VueCliMiddleware;
using VueNet.Authorization;
using VueNet.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddCors();

builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.IgnoreNullValues = true);

var configuration = builder.Configuration;
// configure strongly typed settings object
builder.Services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

// configure DI for application services
builder.Services.AddScoped<IJwtUtils, JwtUtils>();

builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "ClientApp/dist";
});

//對Services資料夾內的檔案做DI
var types = new List<Type>();
var assemblys = Assembly.GetExecutingAssembly().DefinedTypes;
foreach (var assembly in assemblys)
{
    if (!string.IsNullOrEmpty(assembly.Namespace) && assembly.Namespace.EndsWith("Services", StringComparison.Ordinal))
    {
        types.Add(assembly.AsType());
    }
}

var implementTypes = types.Where(x => x.IsClass).ToList();
foreach (var implementType in implementTypes)
{
    var interfaceType = implementType.GetInterface("I" + implementType.Name);

    if (interfaceType != null)
    {
        builder.Services.Add(new ServiceDescriptor(interfaceType, implementType,
            ServiceLifetime.Scoped));
    }
}

var app = builder.Build();

_ = CommandLine.Arguments.TryGetOptions(Environment.GetCommandLineArgs(), false, out string mode, out ushort port, out bool https);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseSpaStaticFiles();
}

if (https) app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// global cors policy
app.UseCors(x => x
    .SetIsOriginAllowed(origin => true)
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());

// global error handler
app.UseMiddleware<ErrorHandlerMiddleware>();

// custom jwt auth middleware
app.UseMiddleware<JwtMiddleware>();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller}/{action=Index}/{id?}");

    // Add MapRazorPages if the app uses Razor Pages. Since Endpoint Routing includes support for many frameworks, adding Razor Pages is now opt -in.
    // endpoints.MapRazorPages();
});

app.UseSpa(spa =>
{
    // To learn more about options for serving an Angular SPA from ASP.NET Core,
    // see https://go.microsoft.com/fwlink/?linkid=864501
    spa.Options.SourcePath = "ClientApp";

    if (app.Environment.IsDevelopment())
    {
        // run npm process with client app
        if (mode == "start")
        {
            spa.UseVueCli(npmScript: "serve", port: port, forceKill: true, https: https);
        }

        // if you just prefer to proxy requests from client app, use proxy to SPA dev server instead,
        // app should be already running before starting a .NET client:
        // run npm process with client app
        if (mode == "attach")
        {
            spa.UseProxyToSpaDevelopmentServer($"{(https ? "https" : "http")}://localhost:{port}"); // your Vue app port
        }
    }
});

app.Run();

