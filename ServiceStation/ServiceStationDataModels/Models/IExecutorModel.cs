using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationDataModels.Models
{
    public interface IExecutorModel : IId
    {
        string ExecutorFIO { get; }
        string? ExecutorEmail { get; }
        string ExecutorPassword { get; }
        string ExecutorNumber { get; }
    }
}
