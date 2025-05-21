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
    public interface ICarLogic
    {
        List<CarViewModel>? ReadList(CarSearchModel? model);
        CarViewModel? ReadElement(CarSearchModel? model);

        bool Create(CarBindingModel model);
        bool Update(CarBindingModel model);
        bool Delete(CarBindingModel model);
    }
}
