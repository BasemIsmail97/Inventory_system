using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shards
{
    public class PurchaseOrderSpecificationParameters
    {
        private const int maxPageSize = 10;
        private const int _defaultPageSize = 5;
        public string? SearchTerm { get; set; }
        public int? SupplierId { get; set; }
        public  string? ApplicationUserId { get; set; }
        private int _pageSize = _defaultPageSize;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
        public int PageIndex { get; set; } = 1;
    }
}
