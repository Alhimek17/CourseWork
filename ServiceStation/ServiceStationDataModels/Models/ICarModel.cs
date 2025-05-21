using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationDataModels.Models
{
    public interface ICarModel : IId
    {
        string CarNumber { get; }
        string CarBrand { get; }
        int ExecutorId { get; }
    }
}
