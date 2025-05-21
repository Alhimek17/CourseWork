using ServiceStationContracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationBusinessLogic.OfficePackage.HelperModels
{
    public class PdfInfoExecutor
    {
        public string FileName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;

        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public List<ReportCarsViewModel> Cars { get; set; } = new();
    }
}
