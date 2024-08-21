using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

//namespace Pillr.Server;

WebApplicationBuilder builder = WebApplication.CreateSlimBuilder(new WebApplicationOptions
{
  ApplicationName = "Pillr.Server",
  Args = args,
  WebRootPath = null,
});

builder.Configuration
  .AddJsonFile("appsettings.json", true)
  .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName.ToLower()}.json", true);

builder.Logging.AddEventLog(settings =>
{
  settings.LogName = "Application";
  settings.SourceName = "Pillr";
});

//builder.Services.AddWindowsService(); // Run this app as a Windows service

builder.Services.AddHostedService<Pillr.Server.WindowsService>();
builder.Services.AddHostedService<Pillr.Server.PipeListener>();

/******************************************************************************
 *  All code above this point is for app configuration
 * ***************************************************************************/

WebApplication app = builder.Build();

/******************************************************************************
 *  All code below this point is for pipeline configuration
 * ***************************************************************************/

//app.UseRouting();

app.Run();

Console.WriteLine("Exited app...");
