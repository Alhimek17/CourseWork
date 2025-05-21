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
    public interface IGuarantorLogic
    {
        List<GuarantorViewModel>? ReadList(GuarantorSearchModel? model);
        GuarantorViewModel? ReadElement(GuarantorSearchModel? model);

        bool Create(GuarantorBindingModel model);
        bool Update(GuarantorBindingModel model);
        bool Delete(GuarantorBindingModel model);
    }
}
