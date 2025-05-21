using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationContracts.SearchModels
{
    public class RepairSearchModel
    {
        public int? Id { get; set; }
        public string? RepairName { get; set; }
        public int? GuarantorId { get; set; }
        public int? DefectId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
