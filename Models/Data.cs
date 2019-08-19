using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GenomixDataManager.Formatters.MultipartDataMediaFormatter.Infrastructure;
using Newtonsoft.Json.Linq;

namespace GenomixDataManager.Models
{
    public class Data
    {
        public HttpFile jsonFile { get; set; }
        public List<string> fields { get; set; }
    }
}
