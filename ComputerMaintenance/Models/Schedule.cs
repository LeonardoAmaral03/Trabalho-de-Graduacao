using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ComputerMaintenance.Models.Enum.EnumStatus;

namespace ComputerMaintenance.Models
{
    public class Schedule
    {
        public Guid Id { get; set; }
        public Guid ScheduleMaintenanceItemId { get; set; }
        public ScheduleMaintenanceItem ScheduleMaintenanceItem { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public DateTime MaintenanceRealizationDate { get; set; }
        public Status Status { get; set; }
    }
}
