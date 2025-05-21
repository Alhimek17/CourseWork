using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationDataModels.Models
{
    public interface ITechnicalWorkModel : IId
    {
        string WorkType { get; }
        DateTime?  DateStartWork { get; }
        double WorkPrice { get; }
        int ExecutorId { get; }
        public Dictionary<int, ICarModel> TechnicalWorkCars { get; }
    }
}
