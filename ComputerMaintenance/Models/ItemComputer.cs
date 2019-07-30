using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerMaintenance.Models
{
    public class ItemComputer
    {
        public Guid Id { get; set; }
        public DateTime AcquisitionDate { get; set; }
        public Computer Computer { get; set; }
        public Item Item { get; set; }
    }
}
