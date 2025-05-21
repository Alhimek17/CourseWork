using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationContracts.SearchModels
{
    public class GuarantorSearchModel
    {
        public int? Id { get; set; }
        public string? GuarantorFIO { get; set; }
        public string? GuarantorEmail { get; set; }
        public string? GuarantorNumber { get; set; }
		public string? GuarantorPassword { get; set; }
	}
}
