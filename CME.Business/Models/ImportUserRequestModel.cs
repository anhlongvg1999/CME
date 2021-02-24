using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CME.Business.Models
{
    public class ImportUserRequestModel
    {
        public IFormFile File { get; set; }
    }
}
