using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationContracts.SearchModels
{
    public class SparePartSearchModel
    {
        public int? Id { get; set; }
        public string? SparePartName { get; set; }
        public int? GuarantorId { get; set; }
    }
}
