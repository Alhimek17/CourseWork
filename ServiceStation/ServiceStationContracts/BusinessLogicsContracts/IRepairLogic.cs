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
    public interface IRepairLogic
    {
        List<RepairViewModel>? ReadList(RepairSearchModel? model);
        RepairViewModel? ReadElement(RepairSearchModel? model);

        bool Create(RepairBindingModel model);
        bool Update(RepairBindingModel model);
        bool Delete(RepairBindingModel model);
        bool AddSparePartToRepair(RepairSearchModel model, int[] spareparts);
	}
}
