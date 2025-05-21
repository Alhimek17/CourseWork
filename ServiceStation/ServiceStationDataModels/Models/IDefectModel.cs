using ServiceStationDataModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationDataModels.Models
{
    public interface IDefectModel : IId
    {
        string DefectType { get; }
        double DefectPrice { get; }
        int ExecutorId { get; }
        int? RepairId { get; }
        public Dictionary<int, ICarModel> DefectCars { get; }
    }
}
