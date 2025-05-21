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
    public interface ITechnicalWorkLogic
    {
        List<TechnicalWorkViewModel>? ReadList(TechnicalWorkSearchModel? model);
        TechnicalWorkViewModel? ReadElement(TechnicalWorkSearchModel? model);

        bool Create(TechnicalWorkBindingModel model);
        bool Update(TechnicalWorkBindingModel model);
        bool Delete(TechnicalWorkBindingModel model);
        bool AddCarToTechnicalWork(TechnicalWorkSearchModel model, int[] cars);

    }
}
