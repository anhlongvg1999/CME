using CME.Business.Interfaces;
using CME.Business.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LumenWorks.Framework.IO.Csv;
using SERP.Filenet.DB;
using Microsoft.AspNetCore.Hosting;
using CME.Entities;
using Microsoft.EntityFrameworkCore;

namespace CME.Business.Implementations
{
    class ReadCSVService : IReadCSVService
    {
        private readonly DataContext _dataContext;
        private readonly IHostingEnvironment _environment;
        public ReadCSVService(DataContext dataContext, IHostingEnvironment environment)
        {
            _dataContext = dataContext;
            _environment = environment;
        }
        async public Task<bool> GetDataCSV(List<DataCSV> dataCSV)
        {
            string strFilePath = @"C:\Users\84355\Desktop\CME\CME\Assets\Lob_" + dataCSV[0].Lot + "_Sub_" + dataCSV[0].Sub + ".csv";
            DateTime creation = File.GetCreationTime(strFilePath);
            TimeSpan time = DateTime.Now.Subtract(creation);
            if (!File.Exists(strFilePath) || time.Days >= 2)
            {
                await WriteNewCSV(strFilePath, dataCSV);
                FileCSV fileCSV = new FileCSV();
                fileCSV.Id = Guid.NewGuid();
                // TODO: USER_ID
                //model.CreatedByUserId = userId;
                fileCSV.CreatedOnDate = DateTime.Now;
                fileCSV.LastModifiedOnDate = DateTime.Now;
                fileCSV.Path = strFilePath;

                await _dataContext.FileCSVs.AddAsync(fileCSV);
                await _dataContext.SaveChangesAsync();
            }
            else
            {
                await AppendCSV(strFilePath, dataCSV);
            }

            return true;
        }
        async private Task AppendCSV(string strFilePath, List<DataCSV> dataCSV)
        {
            var csvTable = new DataTable();
            int index = 0;
            int indexpoint = 0;
            var UCL = dataCSV.Select(x => x.UC).ToArray();
            var CTL = dataCSV.Select(x => x.CT).ToArray();
            string[] dataUCL = await AVG(UCL);
            string[] dataCTL = await AVG(CTL);
            using (var csvReader = new CsvReader(new StreamReader(System.IO.File.OpenRead(strFilePath)), true))
            {
                csvTable.Load(csvReader);
                index = csvTable.Rows.Count;
            }
            foreach (var item in dataCSV)
            {
                string CTProcessing = "#N/A";
                string UCProcessing = "#N/A";

                if (item.CT != 0)
                {
                    CTProcessing = item.CT.ToString();
                }
                if (item.UC != 0)
                {
                    UCProcessing = item.UC.ToString();
                }
                string dataAppend = index.ToString() + "," + item.Hours + "," + item.Lot + "," + item.Sub + "," + item.CT.ToString()
                    + "," + CTProcessing + "," + item.UC.ToString() + "," + UCProcessing + "," + item.Z.ToString() + "," + item.Theta.ToString() + "," + dataUCL[indexpoint] + "," + dataCTL[indexpoint];
                File.AppendAllText(strFilePath, dataAppend + Environment.NewLine);
                index++;
                indexpoint++;
            }
        }
        async private Task WriteNewCSV(string strFilePath, List<DataCSV> dataCSV)
        {
            DataTable table = new DataTable();
            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("Hours", typeof(string));
            table.Columns.Add("Lot", typeof(string));
            table.Columns.Add("Sub", typeof(string));
            table.Columns.Add("CT", typeof(string));
            table.Columns.Add("CTProcessing", typeof(string));
            table.Columns.Add("UC", typeof(string));
            table.Columns.Add("UCProcessing", typeof(string));
            table.Columns.Add("Z", typeof(string));
            table.Columns.Add("Theta", typeof(string));
            table.Columns.Add("UCL Thickness Moving average", typeof(string));
            table.Columns.Add("CTL Thickness Moving average", typeof(string));
            var UCL = dataCSV.Select(x => x.UC).ToArray();
            var CTL = dataCSV.Select(x => x.CT).ToArray();
            string[] dataUCL = await AVG(UCL);
            string[] dataCTL = await AVG(CTL);
            var index = 0;
            foreach (var item in dataCSV)
            {
                string CTProcessing = "#N/A";
                string UCProcessing = "#N/A";
                if (item.CT != 0)
                {
                    CTProcessing = item.CT.ToString();
                }
                if (item.UC != 0)
                {
                    UCProcessing = item.UC.ToString();
                }
                table.Rows.Add(index, item.Hours, item.Lot, item.Sub, item.CT.ToString(), CTProcessing, item.UC.ToString(), UCProcessing, item.Z.ToString(), item.Theta.ToString(), dataUCL[index], dataCTL[index]);
                index++;
            }
            StreamWriter sw = new StreamWriter(strFilePath, false);
            //headers
            for (int i = 0; i < table.Columns.Count; i++)
            {
                sw.Write(table.Columns[i]);
                if (i < table.Columns.Count - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);
            foreach (DataRow dr in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(','))
                        {
                            value = String.Format("\"{0}\"", value);
                            sw.Write(value);
                        }
                        else
                        {
                            sw.Write(dr[i].ToString());
                        }
                    }
                    if (i < table.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
        }
        async private Task<string[]> AVG(double[] data)
        {
            string[] result = new string[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                double total = 0;
                double index = 0;
                if (i - 3 >= 0)
                {
                    if (i + 3 > data.Length - 1)
                    {
                        if (data[i] == 0)
                        {
                            for (int j = i - 3; j < data.Length; j++)
                            {
                                result[j] = "#N/A";

                            }
                            return result;
                        }
                        else
                        {
                            for (int j = i - 3; j < data.Length; j++)
                            {
                                total = total + data[j];
                                index++;
                            }
                        }

                    }
                    else
                    {
                        if (data[i] == 0)
                        {
                            for (int j = i - 3; j <= i + 3; j++)
                            {
                                result[j] = "#N/A";

                            }
                            if (i + 4 > data.Length - 1)
                            {
                                return result;
                            }
                            else
                            {
                                i = i + 3;
                                continue;
                            }
                        }
                        else
                        {
                            for (int j = i - 3; j <= i + 3; j++)
                            {
                                total = total + data[j];
                                index++;
                            }

                        }

                    }

                }
                else if (i - 3 < 0)
                {
                    if (data[i] == 0)
                    {
                        for (int j = 0; j <= i + 3; j++)
                        {
                            result[j] = "#N/A";

                        }
                        if (i + 4 > data.Length - 1)
                        {
                            return result;
                        }
                        else
                        {
                            i = i + 3;
                            continue;
                        }
                    }
                    else
                    {
                        for (int j = 0; j <= i + 3; j++)
                        {
                            total = total + data[j];
                            index++;
                        }
                    }

                }
                if (result[i] == "#N/A")
                {
                    i++;
                }
                else
                {
                    result[i] = Math.Round((total / index), 2).ToString();
                }

            }
            return result;
        }

        public async Task<FileCSV> GetById(Guid id)
        {
            var model = await _dataContext.FileCSVs.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return model;
        }

        public async Task<List<DataChart>> GetDataChart(DataChartQueryModel queryModel)
        {
            var dataPath = await GetById(queryModel.Id);
            if (dataPath == null)
            {
                throw new ArgumentException($"Không tìm thấy dữ liệu");
            }
            var csvTable = new DataTable();
            List<DataChart> result = new List<DataChart>();
            using (var csvReader = new CsvReader(new StreamReader(System.IO.File.OpenRead(dataPath.Path)), true))
            {
                csvTable.Load(csvReader);
                for (int i = 0; i < csvTable.Rows.Count; i++)
                {
                    DataChart dataChart = new DataChart();
                    dataChart.UCLAverage = csvTable.Rows[i][10].ToString();
                    dataChart.CTLAverage = csvTable.Rows[i][11].ToString();
                    if (csvTable.Rows[i][10].ToString() != "#N/A" && (Convert.ToDouble(csvTable.Rows[i][10].ToString()) * Convert.ToDouble(1 + queryModel.ThresholdForUCL) > Convert.ToDouble(csvTable.Rows[i][11]) && (Convert.ToDouble(csvTable.Rows[i][10].ToString()) * Convert.ToDouble(1 - queryModel.ThresholdForUCL) < Convert.ToDouble(csvTable.Rows[i][11]))))
                    {
                        dataChart.UCLAfterRemoval = csvTable.Rows[i][11].ToString();
                        dataChart.ZForUCl = csvTable.Rows[i][8].ToString();
                    }
                    else
                    {
                        dataChart.UCLAfterRemoval = "#N/A";
                        dataChart.ZForUCl = "#N/A";
                    }
                    if (csvTable.Rows[i][11].ToString() != "#N/A" && (Convert.ToDouble(csvTable.Rows[i][11].ToString()) * Convert.ToDouble(1 + queryModel.ThresholdForCTL) > Convert.ToDouble(csvTable.Rows[i][5]) && (Convert.ToDouble(csvTable.Rows[i][11].ToString()) * Convert.ToDouble(1 - queryModel.ThresholdForCTL) < Convert.ToDouble(csvTable.Rows[i][5]))))
                    {
                        dataChart.CTLAfterRemoval = csvTable.Rows[i][5].ToString();
                        dataChart.ZForCTL = csvTable.Rows[i][8].ToString();
                    }
                    else
                    {
                        dataChart.CTLAfterRemoval = "#N/A";
                        dataChart.ZForCTL = "#N/A";
                    }
                    result.Add(dataChart);
                }
            }
            return result;
        }
    }

}
