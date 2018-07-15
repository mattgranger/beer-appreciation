namespace BeerAppreciation.Core.WebApi.Extensions
{
    using Data.Paging;
    using Models;

    public static class PagedResultExtensions
    {
        public static PagedResultModel<T> ToPagedResultModel<T>(this IPagedList<T> pagedList) where T : class
        {
            return new PagedResultModel<T>(pagedList.PageIndex, pagedList.PageSize, pagedList.TotalItemCount, pagedList.Data);
        }
    }
}
