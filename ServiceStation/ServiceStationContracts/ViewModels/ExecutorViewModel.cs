using ServiceStationDataModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationContracts.ViewModels
{
    public class ExecutorViewModel : IExecutorModel
    {
        public int Id { get; set; }
        [DisplayName("ФИО исполнителя")]
        public string ExecutorFIO { get; set; } = string.Empty;
        [DisplayName("Почти исполнителя(логин)")]
        public string? ExecutorEmail { get; set; }
        [DisplayName("Пароль исполнителя")]
        public string ExecutorPassword { get; set; } = string.Empty;
        [DisplayName("Номер телефона исполнителя")]
        public string ExecutorNumber { get; set; } = string.Empty;
    }
}
