using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationDataModels.Models
{
    public interface IWorkModel : IId
    {
        string WorkName { get; }

        Enums.WorkStatus Status { get; }

        double WorkPrice { get; }

        int GuarantorId { get; }

        int? TechnicalWorkId { get; }

        public Dictionary<int, ISparePartModel> WorkSpareParts { get; }
    }
}
