using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationContracts.SearchModels
{
    public class WorkSearchModel
    {
        public int? Id { get; set; }
        public string? WorkName { get; set; }
        public int? GuarantorId { get; set; }
        public int? TechnicalWorkId {  get; set; }
    }
}
