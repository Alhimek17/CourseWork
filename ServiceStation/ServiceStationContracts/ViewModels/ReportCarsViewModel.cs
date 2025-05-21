using ServiceStationDataModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationContracts.ViewModels
{
    public class ReportCarsViewModel
    {
        public string CarNumber { get; set; } = string.Empty;
        public string CarBrand { get; set; } = string.Empty;
        public string WorkType { get; set; } = string.Empty;
        public DateTime? TechnicalWorkDate { get; set; } = DateTime.Now;
        public double TechnicalWorkPrice { get; set; }
        public string RepairName { get; set; } = string.Empty;
        public DateTime? RepairStartDate { get; set; } = DateTime.Now;
        public double RepairPrice { get; set; }
    }
}
