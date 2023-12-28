using ReportService.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Core.Repositories
{
    public class ReportRepository
    {
        public Report GetLastNotSendReport() 
        {
            //todo: pobieranie ostatniego raportu z bazy

            return new Report
            {
                Id = 1,
                Title = "R/1/1/2023",
                Date = new DateTime(2023,10,23,14,23,41),
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
        }

        public void ReportSent(Report report)
        {
            report.IsSend=true;
            //todo: zapis do bazy
        }
    }
}
