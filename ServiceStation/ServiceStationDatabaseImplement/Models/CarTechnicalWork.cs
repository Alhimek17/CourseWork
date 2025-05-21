using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationDatabaseImplement.Models
{
    public class CarTechnicalWork
    {
        public int Id { get; set; }

        [Required]
        public int CarId { get; set; }

        [Required]
        public int TechnicalWorkId { get; set; }

        public virtual Car Car { get; set; } = new();
        public virtual TechnicalWork TechnicalWork { get; set; } = new();
    }
}
