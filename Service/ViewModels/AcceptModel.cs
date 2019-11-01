using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    public class AcceptModel
    {
        public int OrderId { set; get; }
        public string Decision { set; get; }
        public string Reason { set; get; }
        public DateTime ReturnTime { set; get; }
        public string Description { set; get; }
        public double CostRepair { set; get; }
    }
}
