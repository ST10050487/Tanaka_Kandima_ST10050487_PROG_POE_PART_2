using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG_POE_PART_2.Classes
{
    public class ServiceRequestNode
    {
        public ServiceRequest Data { get; set; }
        public ServiceRequestNode Left { get; set; }
        public ServiceRequestNode Right { get; set; }

        public ServiceRequestNode(ServiceRequest data)
        {
            Data = data;
        }
    }
}
