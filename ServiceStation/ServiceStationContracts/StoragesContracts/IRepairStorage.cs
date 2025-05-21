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
    public interface IRepairStorage
    {
        List<RepairViewModel> GetFullList();
        List<RepairViewModel> GetFilteredList(RepairSearchModel model);

        RepairViewModel? GetElement(RepairSearchModel model);
        RepairViewModel? Insert(RepairBindingModel model);
        RepairViewModel? Update(RepairBindingModel model);
        RepairViewModel? Delete(RepairBindingModel model);
    }
}
