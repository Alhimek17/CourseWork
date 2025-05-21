using ServiceStationBusinessLogic.OfficePackage.HelperEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationBusinessLogic.OfficePackage.HelperModels
{
    public class ExcelCellParameters
    {
        public string ColumnName { get; set; } = string.Empty;

        public uint RowIndex { get; set; }

        public string Text { get; set; } = string.Empty;

        public string CellReference => $"{ColumnName}{RowIndex}";

        public ExcelStyleInfoType StyleInfo { get; set; }
    }
}
