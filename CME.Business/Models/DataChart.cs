using System;
using System.Collections.Generic;
using System.Text;

namespace CME.Business.Models
{
    public class DataChart
    {
        public string UCLAverage { get; set; }
        public string CTLAverage { get; set; }
        public string ZForUCl { get; set; }
        public string ZForCTL { get; set; }
        public string UCLAfterRemoval { get; set; }
        public string CTLAfterRemoval { get; set; }

    }
}
