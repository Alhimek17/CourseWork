using ServiceStationDataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationContracts.BindingModels
{
    public class ExecutorBindingModel : IExecutorModel
    {
        public int Id { get; set; }

        public string ExecutorFIO { get; set; } = string.Empty;

        public string ExecutorEmail { get; set; } = string.Empty;

        public string ExecutorPassword { get; set; } = string.Empty;

        public string ExecutorNumber { get; set; } = string.Empty;
    }
}
