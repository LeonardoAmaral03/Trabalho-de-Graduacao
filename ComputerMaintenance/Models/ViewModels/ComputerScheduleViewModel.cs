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
        public string MaintenanceName { get; set; }
        //public string ItemMaintenanceActivity { get; set; }
        public string ItemName { get; set; }
        public Status Status { get; set; }
    }
}
