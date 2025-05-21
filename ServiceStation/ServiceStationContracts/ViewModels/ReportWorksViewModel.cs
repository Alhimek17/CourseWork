using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationContracts.ViewModels
{
    public class ReportWorksViewModel
    {
        public string CarNumber { get; set; } = string.Empty;
        public double FullPrice { get; set; }
        public List<(string, double)> WorksInfo { get; set; } = new();
    }
}
