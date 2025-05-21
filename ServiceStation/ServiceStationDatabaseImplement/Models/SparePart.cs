using ServiceStationContracts.BindingModels;
using ServiceStationContracts.ViewModels;
using ServiceStationDataModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationDatabaseImplement.Models
{
    public class SparePart : ISparePartModel
    {
        public int Id { get; set; }

        [Required]
        public string SparePartName { get; set; } = string.Empty;

        [Required]
        public double SparePartPrice { get; set; }

        [Required]
        public int GuarantorId { get; set; }

        public virtual Guarantor Guarantor { get; set; }

        [ForeignKey("SparePartId")]
        public virtual List<SparePartRepair> SparePartRepairs { get; set; } = new();


        [ForeignKey("SparePartId")]
        public virtual List<SparePartWork> SparePartWorks { get; set; } = new();

        public static SparePart? Create(SparePartBindingModel model)
        {
            if (model == null) return null;
            return new SparePart()
            {
                Id = model.Id,
                SparePartName = model.SparePartName,
                SparePartPrice = model.SparePartPrice,
                GuarantorId = model.GuarantorId
            };
        }

        public void Update(SparePartBindingModel model)
        {
            if (model == null) return;
            SparePartName = model.SparePartName;
            SparePartPrice = model.SparePartPrice;
            GuarantorId = model.GuarantorId;
        }

        public SparePartViewModel GetViewModel => new()
        {
            Id = Id,
            SparePartName = SparePartName,
            SparePartPrice = SparePartPrice,
            GuarantorId = GuarantorId
        };
    }
}
