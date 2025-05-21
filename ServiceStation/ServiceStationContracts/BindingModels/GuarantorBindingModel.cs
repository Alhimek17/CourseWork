using ServiceStationDataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationContracts.BindingModels
{
    public class GuarantorBindingModel : IGuarantorModel
    {
        public int Id { get; set; }

        public string GuarantorFIO {  get; set; } = string.Empty;

        public string GuarantorEmail { get; set; } = string.Empty;

        public string GuarantorPassword {  get; set; } = string.Empty;

        public string GuarantorNumber {  get; set; } = string.Empty;
    }
}
