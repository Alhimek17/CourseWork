using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationDatabaseImplement.Models
{
    public class CarDefect
    {
        public int Id { get; set; }

        [Required]
        public int CarId { get; set; }

        [Required]
        public int DefectId { get; set; }

        public virtual Car Car { get; set; } = new();

        public virtual Defect Defect { get; set; } = new();
    }
}
