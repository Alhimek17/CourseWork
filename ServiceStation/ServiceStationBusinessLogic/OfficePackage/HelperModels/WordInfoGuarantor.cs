using ServiceStationContracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationBusinessLogic.OfficePackage.HelperModels
{
    public class WordInfoGuarantor
    {
        public string FileName { get; set; } = string.Empty;
        public Stream FileStream { get; set; } = new MemoryStream();
        public string Title { get; set; } = string.Empty;
        public List<ReportDefectsViewModel> DefectsBySparePart { get; set; } = new();
    }
}
