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
            Root = InsertAVL(Root, request);
        }

        private int Height(ServiceRequestNode node) => node?.Height ?? 0;

        private int GetBalanceFactor(ServiceRequestNode node) =>
            node == null ? 0 : Height(node.Left) - Height(node.Right);

        private ServiceRequestNode RotateRight(ServiceRequestNode y)
        {
            var x = y.Left;
            var T = x.Right;

            x.Right = y;
            y.Left = T;

            y.Height = Math.Max(Height(y.Left), Height(y.Right)) + 1;
            x.Height = Math.Max(Height(x.Left), Height(x.Right)) + 1;

            return x;
        }

        private ServiceRequestNode RotateLeft(ServiceRequestNode x)
        {
            var y = x.Right;
            var T = y.Left;

            y.Left = x;
            x.Right = T;

            x.Height = Math.Max(Height(x.Left), Height(x.Right)) + 1;
            y.Height = Math.Max(Height(y.Left), Height(y.Right)) + 1;

            return y;
        }

        private ServiceRequestNode InsertAVL(ServiceRequestNode node, ServiceRequest request)
        {
            if (node == null) return new ServiceRequestNode(request);

            if (request.Id < node.Data.Id)
                node.Left = InsertAVL(node.Left, request);
            else if (request.Id > node.Data.Id)
                node.Right = InsertAVL(node.Right, request);
            else
                return node; // Duplicate IDs are not allowed

            node.Height = Math.Max(Height(node.Left), Height(node.Right)) + 1;

            int balance = GetBalanceFactor(node);

            // Left Heavy
            if (balance > 1 && request.Id < node.Left.Data.Id)
                return RotateRight(node);

            // Right Heavy
            if (balance < -1 && request.Id > node.Right.Data.Id)
                return RotateLeft(node);

            // Left-Right
            if (balance > 1 && request.Id > node.Left.Data.Id)
            {
                node.Left = RotateLeft(node.Left);
                return RotateRight(node);
            }

            // Right-Left
            if (balance < -1 && request.Id < node.Right.Data.Id)
            {
                node.Right = RotateRight(node.Right);
                return RotateLeft(node);
            }

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

            return id < node.Data.Id ? Search(node.Left, id) : Search(node.Right, id);
        }
    }
}
