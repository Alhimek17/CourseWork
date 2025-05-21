using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationDataModels.Models
{
    public interface IRepairModel : IId
    {
        string RepairName { get; }

        DateTime? RepairStartDate { get; }

        double RepairPrice { get; }

        int GuarantorId { get; }

        public Dictionary<int, ISparePartModel> RepairSpareParts { get; }
    }
}
