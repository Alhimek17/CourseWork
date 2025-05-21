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
    public interface IGuarantorStorage
    {
        List<GuarantorViewModel> GetFullList();
        List<GuarantorViewModel> GetFilteredList(GuarantorSearchModel model);

        GuarantorViewModel? GetElement(GuarantorSearchModel model);
        GuarantorViewModel? Insert(GuarantorBindingModel model);
        GuarantorViewModel? Update(GuarantorBindingModel model);
        GuarantorViewModel? Delete(GuarantorBindingModel model);
    }
}
