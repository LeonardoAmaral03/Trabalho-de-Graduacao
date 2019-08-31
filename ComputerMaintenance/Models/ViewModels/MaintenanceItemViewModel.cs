using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerMaintenance.Models.ViewModels
{
    public class MaintenanceItemViewModel
    {
        public List<Maintenance> Maintenances { get; set; }
        public Item Item { get; set; }
        public List<MaintenanceItem> MaintenanceItems { get; set; }
    }
}
