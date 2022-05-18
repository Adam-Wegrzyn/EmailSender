using ElasticEmail.Api;
using ElasticEmail.Client;
using ElasticEmail.Model;
using EmailSender;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;


var host = CreateHostBuilder(args).Build();
var emailSender = host.Services.GetService<IEmailSenderService>();


string menu = "1 - Send email \n2 - Upload CSV \nE - Exit ";
bool exit = false;

Console.WriteLine(menu);

while (!exit)
{
    switch (Console.ReadLine())
    {
        case "1":
            var messageData = emailSender.EmailCreation();
            emailSender.SendEmail(messageData);
            Console.WriteLine(menu);
            break;
        case "2":
            var messageDataCsv = emailSender.MergeCsvEmailCreation();
            emailSender.SendEmailsFromCsv(messageDataCsv);
            Console.WriteLine(menu);
            break;
        case "E":
            Console.WriteLine("Goodbye");
            exit = true;
            break;
        default:
            Console.WriteLine("Incorrect input");
            break;
   }
}

static IHostBuilder CreateHostBuilder(string[] args)
{
    var hostBuilder = Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((context, builder) =>
        {
            builder.SetBasePath(Directory.GetCurrentDirectory());
        })
        .ConfigureServices((context, services) =>
        {
            services.AddTransient<IEmailSenderService, EmailSenderService>();
       
        });

    return hostBuilder;
}