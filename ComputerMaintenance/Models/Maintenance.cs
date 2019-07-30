using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerMaintenance.Models
{
    public class Maintenance
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ActiActivity { get; set; }
        public List<MaintenanceItem> MaintenanceItems { get; set; }
    }
}
