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
    public interface IWorkStorage
    {
        List<WorkViewModel> GetFullList();
        List<WorkViewModel> GetFilteredList(WorkSearchModel model);

        WorkViewModel? GetElement(WorkSearchModel model);
        WorkViewModel? Insert(WorkBindingModel model);
        WorkViewModel? Update(WorkBindingModel model);
        WorkViewModel? Delete(WorkBindingModel model);
    }
}
