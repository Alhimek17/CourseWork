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
    public class Work : IWorkModel
    {
        public int Id { get; set; }

        [Required]
        public string WorkName { get; set; } = string.Empty;

        [Required]
        public WorkStatus Status { get; set; }

        [Required]
        public double WorkPrice { get; set; }

        [Required]
        public int GuarantorId { get; set; }

        public int? TechnicalWorkId { get; set; }

        public virtual Guarantor Guarantor { get; set; }
        public virtual TechnicalWork? TechnicalWork { get; set; }

        private Dictionary<int, ISparePartModel>? _workSpareParts = null;

        [NotMapped]
        public Dictionary<int, ISparePartModel> WorkSpareParts
        {
            get
            {
                if (_workSpareParts == null)
                {
                    _workSpareParts = SpareParts.ToDictionary(rec => rec.SparePartId, rec => rec.SparePart as ISparePartModel);
                }
                return _workSpareParts;
            }
        }

        [ForeignKey("WorkId")]
        public virtual List<SparePartWork> SpareParts { get; set; } = new();

        public static Work? Create(ServiceStationDatabase context, WorkBindingModel model)
        {
            if (model == null) return null;
            return new Work()
            {
                Id = model.Id,
                WorkName = model.WorkName,
                Status = model.Status,
                WorkPrice = model.WorkPrice,
                GuarantorId = model.GuarantorId,
                SpareParts = model.WorkSpareParts.Select(x => new SparePartWork
                {
                    SparePart = context.SpareParts.First(y => y.Id == x.Key)
                }).ToList()
            };
        }

        public void Update(WorkBindingModel model)
        {
            if(model == null) return;
            if (model.TechnicalWorkId.HasValue)
            {
                TechnicalWorkId = model.TechnicalWorkId;
                return;
            }
            if (model.Status != WorkStatus.Неизвестен)
            {
                Status = model.Status;
                return;
            }
            WorkName = model.WorkName;
            WorkPrice = model.WorkPrice;
            GuarantorId = model.GuarantorId;
        }
        public WorkViewModel GetViewModel => new()
        {
            Id = Id,
            WorkName = WorkName,
            Status = Status,
            WorkPrice = WorkPrice,
            GuarantorId = GuarantorId,
            TechnicalWorkId = TechnicalWorkId,
            WorkSpareParts = WorkSpareParts,
        };

        public void UpdateSpareParts(ServiceStationDatabase context, WorkBindingModel model)
        {
            var sparepartWorks = context.SparePartWorks.Where(rec => rec.WorkId == model.Id).ToList();
            if (sparepartWorks != null && sparepartWorks.Count > 0)
            {
                context.SparePartWorks.RemoveRange(sparepartWorks.Where(rec => !model.WorkSpareParts.ContainsKey(rec.SparePartId)));
                context.SaveChanges();

                foreach (var updateCar in sparepartWorks)
                {
                    model.WorkSpareParts.Remove(updateCar.SparePartId);
                }
                context.SaveChanges();
            }

            var work = context.Works.First(x => x.Id == Id);
            foreach (var cd in model.WorkSpareParts)
            {
                context.SparePartWorks.Add(new SparePartWork
                {
                    Work = work,
                    SparePart = context.SpareParts.First(x => x.Id == cd.Key)
                });
                context.SaveChanges();
            }
            _workSpareParts = null;
        }
    }
}
