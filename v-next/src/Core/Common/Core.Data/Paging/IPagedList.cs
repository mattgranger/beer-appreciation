namespace BeerAppreciation.Core.Data.Paging
{
    using System.Collections.Generic;

    public interface IPagedList<T>
    {
        int PageIndex { get; set; }

        int PageSize { get; set; }

        long Count { get; set; }

        long TotalItemCount { get; set; }

        IEnumerable<T> Data { get; set; }
    }

    public class PagedList<T> : IPagedList<T>
    {
        public PagedList(int pageIndex, int pageSize, long totalItemCount, IEnumerable<T> data)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.TotalItemCount = totalItemCount;
            this.Data = data;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public long Count { get; set; }
        public long TotalItemCount { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
