using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerMaintenance.Models
{
    public class UpdateStatus
    {
        public Guid ComputerId { get; set; }
        public Guid ItemId { get; set; }
        public Guid MaintenanceId { get; set; }
        public DateTime MaintenanceDate { get; set; }
    }
}
