using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationContracts.BindingModels
{
    public class ReportGuarantorBindingModel
    {
        public Stream FileStream { get; set; } = new MemoryStream();
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int GuarantorId { get; set; }

        public List<int>? Ids { get; set; }
    }
}
