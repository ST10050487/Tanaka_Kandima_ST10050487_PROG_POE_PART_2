using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows.Media.Media3D;

namespace PROG_POE_PART_2.Classes
{
    public class IssueNode
    {
        public Issue Data { get; set; }
        public List<Issue> Issues { get; set; } = new List<Issue>();
        public IssueNode Left { get; set; }
        public IssueNode Right { get; set; }
        public int Height { get; set; }

        public IssueNode(Issue data)
        {
            Data = data;
            Issues.Add(data);
        }
    }

}
