using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationDatabaseImplement.Models
{
    public class SparePartRepair
    {
        public int Id { get; set; }

        [Required]
        public int SparePartId { get; set; }

        [Required]
        public int RepairId { get; set; }

        public virtual SparePart SparePart { get; set; } = new();

        public virtual Repair Repair { get; set; } = new();
    }
}
