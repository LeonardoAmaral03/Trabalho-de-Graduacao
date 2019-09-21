using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerMaintenance.Models
{
    public class ScheduleMaintenanceItem
    {
        public Guid Id { get; set; }
        public Guid ScheduleItemComputerId { get; set; }
        public ScheduleItemComputer ScheduleItemComputer { get; set; }
        public Guid MaintenanceId { get; set; }
        public Maintenance Maintenance { get; set; }
    }
}
