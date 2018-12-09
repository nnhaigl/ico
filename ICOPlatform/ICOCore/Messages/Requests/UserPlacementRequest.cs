using ICOCore.Messages.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICOCore.Messages.Requests
{
    public class UserPlacementRequest : BaseRequest
    {
        /// <summary>
        /// 1 : Left
        /// 2 : Right
        /// </summary>
        public int Position { set; get; }
        /// <summary>
        /// Username nào đc gắn vào cây
        /// </summary>
        public string UserToPlace { set; get; }
        /// <summary>
        /// Username được gắn vào vị trí nào
        /// </summary>
        public string UserToPlaceInto { set; get; }


    }
}
