using ServiceStationContracts.BindingModels;
using ServiceStationContracts.SearchModels;
using ServiceStationContracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationContracts.BusinessLogicsContracts
{
    public interface IExecutorLogic
    {
        List<ExecutorViewModel>? ReadList(ExecutorSearchModel? model);
        ExecutorViewModel? ReadElement(ExecutorSearchModel? model);

        bool Create(ExecutorBindingModel model);
        bool Update(ExecutorBindingModel model);
        bool Delete(ExecutorBindingModel model);
    }
}
