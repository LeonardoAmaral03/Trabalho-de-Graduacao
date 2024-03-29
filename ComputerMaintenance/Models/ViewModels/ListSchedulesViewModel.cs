﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ComputerMaintenance.Models.Enum.EnumStatus;

namespace ComputerMaintenance.Models.ViewModels
{
    public class ListSchedulesViewModel
    {
        public Guid ComputerId { get; set; }
        public string ComputerName { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public Guid MaintenanceId { get; set; }
        public string MaintenanceName { get; set; }
        public Status Status { get; set; }
        public DateTime MaintenanceDate { get; set; }
    }
}
