using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationBusinessLogic.OfficePackage.HelperModels
{
    public class ExcelMergeParameters
    {
        public string CellFromName { get; set; } = string.Empty;

        public string CellToName { get; set; } = string.Empty;

        public string Merge => $"{CellFromName}:{CellToName}";
    }
}
