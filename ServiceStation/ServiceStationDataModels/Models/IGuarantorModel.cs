using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationDataModels.Models
{
    public interface IGuarantorModel : IId
    {
        string GuarantorFIO { get; }
        string GuarantorEmail { get; }
        string GuarantorPassword { get; }
        string GuarantorNumber { get; }
    }
}
