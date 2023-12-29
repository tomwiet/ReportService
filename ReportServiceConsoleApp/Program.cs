<<<<<<< HEAD
<<<<<<< HEAD
﻿using Cypher.EncryptStringSample;
=======
﻿using Cipher.EncryptStringSample;
>>>>>>> 3b7edae25e60fcc2582269aaab1ca6a448763e92
=======
﻿using Cipher.EncryptStringSample;
>>>>>>> 3b7edae25e60fcc2582269aaab1ca6a448763e92
using EmailSender;
using ReportService.Core;
using ReportService.Core.Domains;
using ReportService.Core.Models;
using ReportService.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportServiceConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
<<<<<<< HEAD
<<<<<<< HEAD
            var emailReceiver = ConfigurationManager.AppSettings["ReceiverEmail"];
=======
=======
>>>>>>> 3b7edae25e60fcc2582269aaab1ca6a448763e92
            var stringCipher = new StringCipher("1");
            var encryptedPassword = stringCipher.Encrypt("hasło");
            var decriptedPassword = stringCipher.Decrypt(encryptedPassword);

            Console.WriteLine(encryptedPassword); 
            Console.WriteLine(decriptedPassword);

            Console.ReadLine();
            
            return;
            var emailReceiver = "tete10@wp.pl";

>>>>>>> 3b7edae25e60fcc2582269aaab1ca6a448763e92
            var htmlEmail = new GenerateHtmlEmail();
            
            var email = new Email(new EmailParams
            {
                HostSmtp = ConfigurationManager.AppSettings["HostSmtp"],
                EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]),
                Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
                SenderName = ConfigurationManager.AppSettings["SenderName"],
                SenderEmail = ConfigurationManager.AppSettings["SenderEmail"],
                SenderEmailPassword = DecryptSenderEmailPassword()

            });

            

            var report = new Report
            {
                Id = 1,
                Title = "R/1/1/2023",
                Date = new DateTime(2023, 1, 1, 12, 0, 0),
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

            Console.WriteLine("Wysyłanie maila z błędami...");
            email.Send("Błedy w aplikacji", htmlEmail.GenerateErrors(error, 1), emailReceiver).Wait();
            Console.WriteLine("Mail z błędami wysłany");

            Console.WriteLine("Wysyłanie maila z raportem...");
            email.Send("Raport dobowy", htmlEmail.GenerateReports(report), emailReceiver).Wait();
            Console.WriteLine("Mail z raportem wysłany");

            Console.ReadLine();
        }

        public static string DecryptSenderEmailPassword()
        {
            var encryptedPassword = ConfigurationManager.AppSettings["SenderEmailPassword"];
            var stringCypher = new StringCypher("002473B4-F135-40AF-B680-8BFC8F4C34B2");
            if (encryptedPassword.StartsWith("encrypt:"))
            {
                encryptedPassword = stringCypher.
                    Encrypt(encryptedPassword.Replace("encrypt:", ""));

                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                configFile.AppSettings.Settings["SenderEmailPassword"].Value = encryptedPassword;

                configFile.Save();
            }
            return stringCypher.Decrypt(encryptedPassword);

        }
    }


}
