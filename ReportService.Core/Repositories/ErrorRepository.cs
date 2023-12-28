using ReportService.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Core.Repositories
{
    public class ErrorRepository
    {
        public List<Error> GetLastErrors(int intervalInMinutes) 
        {
            //todo: pobieranie z bazy danych

            return new List<Error>
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
        }
    }
}
