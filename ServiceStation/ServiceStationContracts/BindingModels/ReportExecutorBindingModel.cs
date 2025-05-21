using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ServiceStationContracts.BindingModels
{
    public class ReportExecutorBindingModel
    {
        public Stream FileStream { get; set; } = new MemoryStream();

        public int ExecutorId { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public List<int>? Ids { get; set; }
    }
}
