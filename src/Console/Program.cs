using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
//using Microsoft.Build.Evaluation;

var builder = Host.CreateApplicationBuilder(args);

//builder.Services.AddHostedService<AzureDeployService>();

var app = builder.Build();

var stoppingTokenSource = new CancellationTokenSource();

var task = app.RunAsync(stoppingTokenSource.Token);

// Run the View
while(!task.IsCompleted || !task.IsCanceled)
{
  new View().Display();
}
