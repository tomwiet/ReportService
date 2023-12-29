﻿using Cipher.EncryptStringSample;
using EmailSender;
using ReportService.Core;
using ReportService.Core.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ReportService
{
    public partial class ReportService : ServiceBase
    {
        private static readonly NLog.Logger Logger 
            = NLog.LogManager.GetCurrentClassLogger();
        private int SendHour = 8;
        private const int IntervalInMinutes = 1;
        private Timer _timer = new Timer(IntervalInMinutes*60000);
        private ErrorRepository _errorRepository = new ErrorRepository();
        private ReportRepository _reportRepository = new ReportRepository();
        private Email _email;
        private GenerateHtmlEmail _htmlEmail = new GenerateHtmlEmail();
        private string _emailReceiver;
        private StringCipher _stringCypher = new StringCipher("002473B4-F135-40AF-B680-8BFC8F4C34B2");

        public ReportService()
        {
            InitializeComponent();
            try
            {
                _emailReceiver = ConfigurationManager.AppSettings["ReceiverEmail"];


                _email = new Email(new EmailParams
                {
                    HostSmtp = ConfigurationManager.AppSettings["HostSmtp"],
                    EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]),
                    Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
                    SenderName = ConfigurationManager.AppSettings["SenderName"],
                    SenderEmail = ConfigurationManager.AppSettings["SenderEmail"],
                    SenderEmailPassword = DecryptSenderEmailPassword()

                }); ;

            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                throw new Exception(ex.Message);
            }
            
        }

        private string DecryptSenderEmailPassword()
        {
            var encryptedPassword = ConfigurationManager.AppSettings["SenderEmailPassword"];

            if (encryptedPassword.StartsWith("encrypt:"))
            {
                encryptedPassword = _stringCypher.
                    Encrypt(encryptedPassword.Replace("encrypt:", ""));

                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                configFile.AppSettings.Settings["SenderEmailPassword"].Value = encryptedPassword;

                configFile.Save();
            }
            return _stringCypher.Decrypt(encryptedPassword);

        }

        protected override void OnStart(string[] args)
        {
            _timer.Elapsed += DoWork;
            _timer.Start();
            Logger.Info("Sevice sratrted....");
        }

        private async void DoWork(object sender, ElapsedEventArgs e)
        {
            try
            {
                await SendError();
                await SendReport();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                throw new Exception(ex.Message);
            }
            
        }

        private async Task SendError()
        {
            var errors = _errorRepository.GetLastErrors(IntervalInMinutes);

            if (errors == null || !errors.Any())
                return;
            await _email.Send("Błedy w aplikacji", _htmlEmail.GenerateErrors(errors, IntervalInMinutes), _emailReceiver);
            Logger.Info("Error sent.");
        }
        private async Task SendReport()
        {
            var actualHour = DateTime.Now.Hour;

            if (actualHour < SendHour)
                return;

            var report = _reportRepository.GetLastNotSendReport();
            
            if (report == null)
                return;
            
            await _email.Send("Raport dobowy", _htmlEmail.GenerateReports(report), _emailReceiver);

            _reportRepository.ReportSent(report);
            Logger.Info("Report sent.");
        }

        protected override void OnStop()
        {
            Logger.Info("Sevice stoped....");
        }
    }
}
