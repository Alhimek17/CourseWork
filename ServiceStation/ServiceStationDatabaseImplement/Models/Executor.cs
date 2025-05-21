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
    public class Executor : IExecutorModel
    {
        public int Id { get; set; }

        [Required]
        public string ExecutorFIO { get; set; } = string.Empty;

        public string? ExecutorEmail { get; set; } = string.Empty;

        [Required]
        public string ExecutorPassword { get; set; } = string.Empty;

        [Required]
        public string ExecutorNumber { get; set; } = string.Empty;

        [ForeignKey("ExecutorId")]
        public virtual List<Car> Cars { get; set; } = new();
        [ForeignKey("ExecutorId")]
        public virtual List<Defect> Defects { get; set; } = new();
        [ForeignKey("ExecutorId")]
        public virtual List<TechnicalWork> TechnicalWorks { get; set; } = new();

        public static Executor? Create(ExecutorBindingModel model)
        {
            if (model == null) return null;
            return new Executor()
            {
                Id = model.Id,
                ExecutorFIO = model.ExecutorFIO,
                ExecutorEmail = model.ExecutorEmail,
                ExecutorPassword = model.ExecutorPassword,
                ExecutorNumber = model.ExecutorNumber
            };
        }

        public void Update(ExecutorBindingModel model)
        {
            if(model == null) return;
            ExecutorFIO = model.ExecutorFIO;
            ExecutorEmail = model.ExecutorEmail;
            ExecutorPassword = model.ExecutorPassword;
            ExecutorNumber = model.ExecutorNumber;
        }

        public ExecutorViewModel GetViewModel => new()
        {
            Id = Id,
            ExecutorFIO = ExecutorFIO,
            ExecutorEmail = ExecutorEmail,
            ExecutorPassword = ExecutorPassword,
            ExecutorNumber = ExecutorNumber
        };
    }
}
