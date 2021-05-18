using CME.Business.Models;
using CME.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CME.Business.Interfaces
{
    public interface IReadCSVService
    {
        Task<bool> GetDataCSV(List<DataCSV> dataCSV);
        Task<FileCSV> GetById(Guid id);
        Task<List<DataChart>> GetDataChart(DataChartQueryModel queryModel);
    }
}
