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
    public interface ITechnicalWorkStorage
    {
        List<TechnicalWorkViewModel> GetFullList();
        List<TechnicalWorkViewModel> GetFilteredList(TechnicalWorkSearchModel model);

        TechnicalWorkViewModel? GetElement(TechnicalWorkSearchModel model);
        TechnicalWorkViewModel? Insert(TechnicalWorkBindingModel model);
        TechnicalWorkViewModel? Update(TechnicalWorkBindingModel model);
        TechnicalWorkViewModel? Delete(TechnicalWorkBindingModel model);
    }
}
