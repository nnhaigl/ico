namespace ICOCore.Queries.Base
{
    public class BaseQuery<T>
    {
        public BaseQuery()
        {
            PageSize = 25;
            SortAscending = true;
            Filter = string.Empty;
            Status = -1;
        }

        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string SortColumn { get; set; }
        public bool SortAscending { get; set; }
        public string Filter { get; set; }
        public int Status { get; set; }

    }
}
