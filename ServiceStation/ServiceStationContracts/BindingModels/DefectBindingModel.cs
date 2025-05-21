using ServiceStationDataModels.Enums;
using ServiceStationDataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationContracts.BindingModels
{
    public class DefectBindingModel : IDefectModel
    {
        public int Id { get; set; }

        public string DefectType { get; set; } = string.Empty;

        public double DefectPrice { get; set; }

        public int ExecutorId { get; set; }

        public int? RepairId { get; set; }

        public Dictionary<int, ICarModel> DefectCars { get; set; } = new();
    }
}
