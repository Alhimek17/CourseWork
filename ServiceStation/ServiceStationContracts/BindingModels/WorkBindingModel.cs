using ServiceStationDataModels.Enums;
using ServiceStationDataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationContracts.BindingModels
{
    public class WorkBindingModel : IWorkModel
    {
        public int Id { get; set; }

        public string WorkName { get; set; } = string.Empty;

        public WorkStatus Status { get; set; } = WorkStatus.Неизвестен;

        public double WorkPrice { get; set; }

        public int GuarantorId { get; set; }

        public int? TechnicalWorkId { get; set; }

        public Dictionary<int, ISparePartModel> WorkSpareParts { get; set; } = new();
    }
}
