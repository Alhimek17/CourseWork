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
    public interface ICarStorage
    {
        List<CarViewModel> GetFullList();
        List<CarViewModel> GetFilteredList(CarSearchModel model);

        CarViewModel? GetElement(CarSearchModel model);
        CarViewModel? Insert(CarBindingModel model);
        CarViewModel? Update(CarBindingModel model);
        CarViewModel? Delete(CarBindingModel model);
    }
}
