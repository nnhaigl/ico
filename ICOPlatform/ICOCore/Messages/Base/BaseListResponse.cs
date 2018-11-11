using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICOCore.Messages.Base
{
    public class BaseListResponse<T> : BaseResponse where T : class
    {
        public List<T> Data { get; set; }
        public int TotalItems { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public int PageCount
        {
            get
            {
                if (PageSize <= 0) return 0;
                return (int)Math.Ceiling((decimal)TotalItems / PageSize);
            }
            internal set { }
        }

    }
}
