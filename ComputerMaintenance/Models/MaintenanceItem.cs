using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ComputerMaintenance.Models.Enum.EnumStatus;

namespace ComputerMaintenance.Models
{
    public class MaintenanceItem
    {
        public Guid Id { get; set; }
        public int Period { get; set; }
        public Status Status { get; set; }
        public Item Item { get; set; }
        public Maintenance Maintenance { get; set; }
    }
}
