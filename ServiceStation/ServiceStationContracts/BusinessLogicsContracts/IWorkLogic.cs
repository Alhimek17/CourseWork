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
    public interface IWorkLogic
    {
        List<WorkViewModel>? ReadList(WorkSearchModel? model);
        WorkViewModel? ReadElement(WorkSearchModel? model);

        bool Create(WorkBindingModel model);
        bool Update(WorkBindingModel model);
        bool Delete(WorkBindingModel model);
        bool AddSparePartToWork(WorkSearchModel model, int[] spareparts);
	}
}
