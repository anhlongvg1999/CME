using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TSoft.Framework.DB;

namespace CME.Entities
{
    [Table("path_CSV")]
    public class FileCSV : BaseTable<FileCSV>
    {
        [Key]
        public Guid Id { get; set; }

        public string Path { get; set; }
        public DateTime? Time { get; set; }

    }
}
