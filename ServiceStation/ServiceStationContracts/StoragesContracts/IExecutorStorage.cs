using ServiceStationContracts.BindingModels;
using ServiceStationContracts.SearchModels;
using ServiceStationContracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationContracts.StoragesContracts
{
    public interface IExecutorStorage
    {
        List<ExecutorViewModel> GetFullList();
        List<ExecutorViewModel> GetFilteredList(ExecutorSearchModel model);

        ExecutorViewModel? GetElement(ExecutorSearchModel model);
        ExecutorViewModel? Insert(ExecutorBindingModel model);
        ExecutorViewModel? Update(ExecutorBindingModel model);
        ExecutorViewModel? Delete(ExecutorBindingModel model);
    }
}
