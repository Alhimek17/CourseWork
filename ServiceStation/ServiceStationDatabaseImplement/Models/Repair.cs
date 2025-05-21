using ServiceStationContracts.BindingModels;
using ServiceStationContracts.ViewModels;
using ServiceStationDataModels.Enums;
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
    public class Repair : IRepairModel
    {
        public int Id { get; set; }

        [Required]
        public string RepairName { get; set; } = string.Empty;

        public DateTime? RepairStartDate { get; set; } = DateTime.Now;

        [Required]
        public double RepairPrice { get; set; }

        [Required]
        public int GuarantorId { get; set; }

        public virtual Guarantor Guarantor { get; set; }

        private Dictionary<int, ISparePartModel>? _repairSpareParts = null;

        [NotMapped]
        public Dictionary<int, ISparePartModel> RepairSpareParts
        {
            get
            {
                if (_repairSpareParts == null)
                {
                    _repairSpareParts = SpareParts.ToDictionary(rec => rec.SparePartId, rec => rec.SparePart as ISparePartModel);
                }
                return _repairSpareParts;
            }
        }

        [ForeignKey("RepairId")]
        public virtual List<SparePartRepair> SpareParts { get; set; } = new();
        [ForeignKey("RepairId")]
        public virtual List<Defect> Defects { get; set; } = new();


        public static Repair? Create(ServiceStationDatabase context, RepairBindingModel model)
        {
            if (model == null) return null;
            return new Repair()
            {
                Id = model.Id,
                RepairName = model.RepairName,
                RepairStartDate = model.RepairStartDate,
                RepairPrice = model.RepairPrice,
                GuarantorId = model.GuarantorId,
                SpareParts = model.RepairSpareParts.Select(x => new SparePartRepair
                {
                    SparePart = context.SpareParts.First(y => y.Id == x.Key)
                }).ToList()
            };
        }

        public void Update(RepairBindingModel model)
        {
            if (model == null) return;
            RepairName = model.RepairName;
            RepairStartDate = model.RepairStartDate;
            RepairPrice = model.RepairPrice;
        }

        public RepairViewModel GetViewModel => new()
        {
            Id = Id,
            RepairName = RepairName,
            RepairStartDate = RepairStartDate,
            RepairPrice = RepairPrice,
            GuarantorId = GuarantorId,
            RepairSpareParts = RepairSpareParts
        };

        public void UpdateSpareParts(ServiceStationDatabase context, RepairBindingModel model)
        {
            var sparepartRepairs = context.SparePartRepairs.Where(rec => rec.RepairId == model.Id).ToList();
            if (sparepartRepairs != null && sparepartRepairs.Count > 0)
            {
                context.SparePartRepairs.RemoveRange(sparepartRepairs.Where(rec => !model.RepairSpareParts.ContainsKey(rec.SparePartId)));
                context.SaveChanges();

                foreach (var updateSparePart in sparepartRepairs)
                {
                    model.RepairSpareParts.Remove(updateSparePart.SparePartId);
                }
                context.SaveChanges();
            }

            var repair = context.Repairs.First(x => x.Id == Id);
            foreach (var cd in model.RepairSpareParts)
            {
                context.SparePartRepairs.Add(new SparePartRepair
                {
                    Repair = repair,
                    SparePart = context.SpareParts.First(x => x.Id == cd.Key)
                });
                context.SaveChanges();
            }
            _repairSpareParts = null;
        }
    }
}
