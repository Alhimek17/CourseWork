using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationDataModels.Models
{
    public interface ISparePartModel : IId
    {
        string SparePartName { get; }

        double SparePartPrice { get; }

        int GuarantorId { get; }
    }
}
