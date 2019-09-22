using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ComputerMaintenance.Models.Enum.EnumStatus;

namespace ComputerMaintenance.Models.ViewModels
{
    public class ComputerScheduleViewModel
    {
        public DateTime MaintenanceDate { get; set; }
        public string ItemMaintenanceName { get; set; }
        public Status Status { get; set; }
    }
}
