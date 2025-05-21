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
    public interface IDefectLogic
    {
        List<DefectViewModel>? ReadList(DefectSearchModel? model);
        DefectViewModel? ReadElement(DefectSearchModel? model);

        bool Create(DefectBindingModel model);
        bool Update(DefectBindingModel model);
        bool Delete(DefectBindingModel model);
        bool AddCarToDefect(DefectSearchModel model, int[] cars);
    }
}
