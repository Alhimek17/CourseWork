using ServiceStationDataModels.Enums;
using ServiceStationDataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationContracts.BindingModels
{
    public class RepairBindingModel : IRepairModel
    {
        public int Id { get; set; }

        public string RepairName { get; set; } = string.Empty;

        public DateTime? RepairStartDate {  get; set; } = DateTime.Now;

        public double RepairPrice { get; set; }

        public int GuarantorId { get; set; }

        public int? DefectId { get; set; }

        public Dictionary<int, ISparePartModel> RepairSpareParts { get; set; } = new();
    }
}
