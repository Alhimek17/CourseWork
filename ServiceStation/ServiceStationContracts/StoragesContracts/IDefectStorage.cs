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
    public interface IDefectStorage
    {
        List<DefectViewModel> GetFullList();
        List<DefectViewModel> GetFilteredList(DefectSearchModel model);

        DefectViewModel? GetElement(DefectSearchModel model);
        DefectViewModel? Insert(DefectBindingModel model);
        DefectViewModel? Update(DefectBindingModel model);
        DefectViewModel? Delete(DefectBindingModel model);
    }
}
