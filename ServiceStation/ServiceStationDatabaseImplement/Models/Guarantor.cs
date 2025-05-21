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
    public class Guarantor : IGuarantorModel
    {
        public int Id { get; set; }

        [Required]
        public string GuarantorFIO { get; set; } = string.Empty;

        public string? GuarantorEmail { get; set; } = string.Empty;

        [Required]
        public string GuarantorPassword { get; set; } = string.Empty;

        [Required]
        public string GuarantorNumber { get; set; } = string.Empty;

        [ForeignKey("GuarantorId")]
        public virtual List<Repair> Repairs { get; set; } = new();

        [ForeignKey("GuarantorId")]
        public virtual List<SparePart> SpareParts { get; set; } = new();

        [ForeignKey("GuarantorId")]
        public virtual List<Work> Works { get; set; } = new();

        public static Guarantor? Create(GuarantorBindingModel model)
        {
            if (model == null) return null;
            return new Guarantor()
            {
                Id = model.Id,
                GuarantorFIO = model.GuarantorFIO,
                GuarantorEmail = model.GuarantorEmail,
                GuarantorPassword = model.GuarantorPassword,
                GuarantorNumber = model.GuarantorNumber
            };
        }

        public void Update(GuarantorBindingModel model)
        {
            if (model == null) return;
            GuarantorFIO = model.GuarantorFIO;
            GuarantorEmail = model.GuarantorEmail;
            GuarantorPassword = model.GuarantorPassword;
            GuarantorNumber = model.GuarantorNumber;
        }

        public GuarantorViewModel GetViewModel => new()
        {
            Id = Id,
            GuarantorFIO = GuarantorFIO,
            GuarantorEmail = GuarantorEmail,
            GuarantorPassword = GuarantorPassword,
            GuarantorNumber = GuarantorNumber
        };
    }
}
