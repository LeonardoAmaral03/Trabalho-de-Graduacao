using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerMaintenance.Models.ViewModels
{
    public class ItemComputerViewModel
    {
        public List<Item> Items { get; set; }
        public Computer Computer { get; set; }
        public List<ItemComputer> ItemComputers { get; set; }
    }
}
