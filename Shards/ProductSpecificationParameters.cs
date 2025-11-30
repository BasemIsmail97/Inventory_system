using Shards.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shards
{
    public class ProductSpecificationParameters
    {
        private const int maxPageSize = 10;
        private const int _defultPageSize = 5;
        public string ? SearchTerm { get; set; }
        public int ? MinPrice { get; set; }
        public int ? MaxPrice { get; set; }
        public int ? CategoryId { get; set; }
        public int ? SupplierId { get; set; }
        public ProductSortingOptions sort { get; set; }
        private int _pageSize = _defultPageSize;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
        public int PageIndex { get; set; } = 1;
    }
}
