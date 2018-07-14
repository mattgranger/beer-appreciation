﻿namespace BeerAppreciation.Core.WebApi.Models
{
    using System.Collections.Generic;

    public class PagedResultModel<TEntity> where TEntity : class
    {
        public PagedResultModel(int pageIndex, int pageSize, long count, IEnumerable<TEntity> data)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.Count = count;
            this.Data = data;
        }

        public int PageIndex { get; private set; }

        public int PageSize { get; private set; }

        public long Count { get; private set; }

        public IEnumerable<TEntity> Data { get; private set; }
    }
}
