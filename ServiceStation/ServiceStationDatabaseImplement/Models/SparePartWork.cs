using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationDatabaseImplement.Models
{
    public class SparePartWork
    {
        public int Id { get; set; }

        [Required]
        public int SparePartId { get; set; }

        [Required]
        public int WorkId { get; set; }

        public virtual SparePart SparePart { get; set; } = new();
        public virtual Work Work { get; set; } = new();
    }
}
