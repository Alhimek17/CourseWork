using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationContracts.BindingModels
{
    public class PdfReportBindingModel
    {
        public string FileName { get; set; } = string.Empty;

        public int ExecutorId { get; set; }

        public int GuarantorId { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public List<int>? Ids { get; set; }
    }
}
