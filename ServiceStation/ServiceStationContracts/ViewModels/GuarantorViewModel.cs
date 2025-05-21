using ServiceStationDataModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationContracts.ViewModels
{
    public class GuarantorViewModel : IGuarantorModel
    {
        public int Id { get; set; }

        [DisplayName("ФИО поручителя")]
        public string GuarantorFIO { get; set; } = string.Empty;

        [DisplayName("Почта поручителя(логин)")]
        public string GuarantorEmail { get; set; } = string.Empty;

        [DisplayName("Пароль поручителя")]
        public string GuarantorPassword { get; set; } = string.Empty;

        [DisplayName("Номер телефона поручителя")]
        public string GuarantorNumber { get; set; } = string.Empty;
    }
}
