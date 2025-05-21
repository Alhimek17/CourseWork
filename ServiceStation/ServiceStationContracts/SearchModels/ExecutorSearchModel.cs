using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationContracts.SearchModels
{
    public class ExecutorSearchModel
    {
        public int? Id { get; set; }
        public string? ExecutorFIO { get; set; }
        public string? ExecutorNumber { get; set; }
        public string? ExecutorEmail { get; set; }
        public string? ExecutorPassword { get; set; }
    }
}
