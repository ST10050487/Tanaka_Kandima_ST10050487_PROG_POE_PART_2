using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG_POE_PART_2.Classes
{
    public class BinarySearchTree
    {
        public ServiceRequestNode Root { get; private set; }

        public void Insert(ServiceRequest request)
        {
            Root = Insert(Root, request);
        }

        private ServiceRequestNode Insert(ServiceRequestNode node, ServiceRequest request)
        {
            if (node == null)
                return new ServiceRequestNode(request);

            if (request.Id < node.Data.Id)
                node.Left = Insert(node.Left, request);
            else if (request.Id > node.Data.Id)
                node.Right = Insert(node.Right, request);

            return node;
        }

        public ServiceRequest Search(int id)
        {
            return Search(Root, id);
        }

        private ServiceRequest Search(ServiceRequestNode node, int id)
        {
            if (node == null || node.Data.Id == id)
                return node?.Data;

            if (id < node.Data.Id)
                return Search(node.Left, id);
            else
                return Search(node.Right, id);
        }
    }
}
