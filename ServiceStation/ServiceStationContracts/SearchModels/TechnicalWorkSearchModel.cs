using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationContracts.SearchModels
{
    public class TechnicalWorkSearchModel
    {
        public int? Id { get; set; }
        public string? WorkType { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int? ExecutorId { get; set; }
    }
}
