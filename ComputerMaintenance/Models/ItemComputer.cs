﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerMaintenance.Models
{
    public class ItemComputer
    {
        public Guid ComputerId { get; set; }
        public Computer Computer { get; set; }
        public Guid ItemId { get; set; }
        public Item Item { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
