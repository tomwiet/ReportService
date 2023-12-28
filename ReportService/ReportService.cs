using ReportService.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private const int IntervalInMinutes = 60;
        private Timer _timer = new Timer(IntervalInMinutes*60000);
        private ErrorRepository _errorRepository = new ErrorRepository();
        private ReportRepository _reportRepository = new ReportRepository();
        public ReportService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _timer.Elapsed += DoWork;
            _timer.Start();
            Logger.Info("Sevice sratrted....");
        }

        private void DoWork(object sender, ElapsedEventArgs e)
        {
            try
            {
                SendError();
                SendReport();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                throw new Exception(ex.Message);
            }
            
        }

        private void SendError()
        {
            var errors = _errorRepository.GetLastErrors(IntervalInMinutes);

            if (errors != null || !errors.Any())
                return;
            //todo: sending mail
            Logger.Info("Error sent.");
        }
        private void SendReport()
        {
            var actualHour = DateTime.Now.Hour;

            if (actualHour < SendHour)
                return;

            var report = _reportRepository.GetLastNotSendReport();
            
            if (report == null)
                return;
            //todo: send mail

            _reportRepository.ReportSent(report);
            Logger.Info("Report sent.");
        }

        protected override void OnStop()
        {
            Logger.Info("Sevice stoped....");
        }
    }
}
