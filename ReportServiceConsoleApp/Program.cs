using EmailSender;
using ReportService.Core;
using ReportService.Core.Domains;
using ReportService.Core.Models;
using ReportService.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportServiceConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var emailReceiver = "tete10@wp.pl";

            var htmlEmail = new GenerateHtmlEmail();
            
            var email = new Email(new EmailParams
            {
                HostSmtp = "smtp.gmail.com",
                EnableSsl = true,
                Port = 587,
                SenderName = "TomWiet",
                SenderEmail = "rsmailservicesystem@gmail.com",
                SenderEmailPassword = "cmsbsetejtjdbbvw",

            });
            var report = new Report
            {
                Id = 1,
                Title = "R/1/1/2023",
                Date = new DateTime(2023, 10, 23, 14, 23, 41),
                Positions = new List<ReportPosition>
                {
                    new ReportPosition
                    {
                        Id=1,
                        RaportId = 1,
                        Title="Position 1",
                        Description ="Position 1",
                        Value = 123.4m
                    },
                    new ReportPosition
                    {
                        Id=2,
                        RaportId = 1,
                        Title="Position 2",
                        Description ="Position 2",
                        Value = 13.23m
                    },
                    new ReportPosition
                    {
                        Id=3,
                        RaportId = 1,
                        Title="Position 3",
                        Description ="Position 3",
                        Value = 3.10m
                    }

                }
            };
            var error =  new List<Error>
            {
                new Error
                {
                    Id = 1,
                    Message ="Błąd testowy 1",
                    Date = DateTime.Now,
                },
                new Error
                {
                    Id = 2,
                    Message ="Błąd testowy 2",
                    Date = DateTime.Now,
                }
            };
            Console.WriteLine("Wysyłanie email (Raport dobowy....");
            email.Send("Błedy w aplikacji", htmlEmail.GenerateErrors(error, 10), emailReceiver).Wait();
            Console.WriteLine("Wysyłano email (Raport dobowy....");

            Console.WriteLine("Wysyłanie email (Błedy w aplikacji....");
            email.Send("Raport dobowy", htmlEmail.GenerateReports(report), emailReceiver).Wait();
            Console.WriteLine("Wysyłano email (Błedy w aplikacji....");

            Console.ReadLine();
        }
    }
}
