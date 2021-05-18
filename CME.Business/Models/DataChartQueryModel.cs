using System;
using System.Collections.Generic;
using System.Text;

namespace CME.Business.Models
{
    public class DataChartQueryModel
    {
        public Guid Id { get; set; }
        public double ThresholdForUCL { get; set; }
        public double ThresholdForCTL { get; set; }
    }
}
