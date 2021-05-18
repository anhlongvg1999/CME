using System;
using System.Collections.Generic;
using System.Text;

namespace CME.Business.Models
{
    public class PathCSVViewModel
    {
        public Guid Id { get; set; }

        public string Path { get; set; }
        public DateTime? Time { get; set; }
    }
}
