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
    public interface ISparePartLogic
    {
        List<SparePartViewModel>? ReadList(SparePartSearchModel? model);
        SparePartViewModel? ReadElement(SparePartSearchModel? model);

        bool Create(SparePartBindingModel model);
        bool Update(SparePartBindingModel model);
        bool Delete(SparePartBindingModel model);
    }
}
