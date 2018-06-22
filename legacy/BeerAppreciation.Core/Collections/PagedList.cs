using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerAppreciation.Core.Collections
{
    /// <summary>
    /// Generic class to enable returning of paged lists.  
    /// The class exposes an IEnumerable list and a total count property
    /// to enable paging calculations and binding.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Obsolete("PageResult exists in System.Web.Http.OData.  Why reinvent the fucking wheel?")]
    public class PagedList<T>
    {
        #region Constructors

        /// <summary>
        /// Empty constructor
        /// </summary>
        public PagedList()
        { }

        /// <summary>
        /// Constructs a paged list based on a list of items
        /// </summary>
        /// <param name="items"></param>
        public PagedList(IEnumerable<T> items)
        {
            var enumerable = items as IList<T> ?? items.ToList();
            Items = enumerable;
            TotalCount = enumerable.Count();
        }

        /// <summary>
        /// Constructs a paged list based on a list of items and a total count
        /// </summary>
        /// <param name="items"></param>
        /// <param name="totalCount"></param>
        public PagedList(IEnumerable<T> items, int totalCount)
        {
            Items = items;
            TotalCount = totalCount;
        }

        #endregion

        /// <summary>
        /// The paged set of Items
        /// </summary>
        public IEnumerable<T> Items { get; set; }
        /// <summary>
        /// The Total number of items available.
        /// </summary>
        public int TotalCount { get; set; }
    }
}
