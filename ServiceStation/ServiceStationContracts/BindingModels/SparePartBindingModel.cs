using ServiceStationDataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationContracts.BindingModels
{
    public class SparePartBindingModel : ISparePartModel
    {
        public int Id { get; set; }

        public string SparePartName { get; set; } = string.Empty;

        public double SparePartPrice { get; set; }

        public int GuarantorId { get; set; }
    }
}
