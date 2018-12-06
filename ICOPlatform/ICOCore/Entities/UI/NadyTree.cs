using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICOCore.Entities.UI
{
    public class NadyTree
    {

        public NadyTree()
        {
            children = new List<NadyTree>(); // 2 phần tử left & right
        }

        public string head { set; get; }
        public long id { set; get; }
        public string username { set; get; }
        public string contents { set; get; }
        public string country { set; get; }
        public int totalleft { set; get; }
        public int totalright { set; get; }
        public string parent { set; get; }
        /// <summary>
        /// children[0] = left
        /// children[1] = right
        /// </summary>
        public List<NadyTree> children { set; get; }

        public long ParentId { set; get; }
        public long LeftPosId { set; get; }
        public long RightPosId { set; get; }
        public double TotalPHDownside { set; get; }
        public bool Gender { set; get; }
        public string level { set; get; }


        public static NadyTree CreateLeftEmptyNode(long parentId)
        {
            NadyTree emptyNode = new NadyTree();
            emptyNode.head = " -- None --";
            emptyNode.id = -1;
            emptyNode.username = null;
            emptyNode.contents = " -- Add New Left --";
            emptyNode.children = null;
            emptyNode.ParentId = parentId;
            emptyNode.LeftPosId = 0;
            emptyNode.RightPosId = 0;
            emptyNode.TotalPHDownside = 0;

            return emptyNode;
        }

        public static NadyTree CreateRightEmptyNode(long parentId)
        {
            NadyTree emptyNode = new NadyTree();
            emptyNode.head = " -- None --";
            emptyNode.id = -2;
            emptyNode.username = null;
            emptyNode.contents = " -- Add New Right --";
            emptyNode.children = null;
            emptyNode.ParentId = parentId;
            emptyNode.LeftPosId = 0;
            emptyNode.RightPosId = 0;
            emptyNode.TotalPHDownside = 0;

            return emptyNode;
        }

    }
}
