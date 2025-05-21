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
    public interface ISparePartStorage
    {
        List<SparePartViewModel> GetFullList();
        List<SparePartViewModel> GetFilteredList(SparePartSearchModel model);

        SparePartViewModel? GetElement(SparePartSearchModel model);
        SparePartViewModel? Insert(SparePartBindingModel model);
        SparePartViewModel? Update(SparePartBindingModel model);
        SparePartViewModel? Delete(SparePartBindingModel model);
    }
}
