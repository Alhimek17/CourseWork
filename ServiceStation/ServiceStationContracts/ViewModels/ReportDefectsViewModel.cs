using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationContracts.ViewModels
{
    public class ReportDefectsViewModel
    {
        public string SparePartName { get; set; } = string.Empty;
        public double FullPrice { get; set; }
        public List<(string, double)> DefectsInfo { get; set; } = new();
    }
}
