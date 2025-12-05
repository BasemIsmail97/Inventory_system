using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shards
{
    public class SalesOrderSpecificationParameters
    {
        private const int maxPageSize = 10;
        private const int _defultPageSize = 5;
        public string? SearchTerm { get; set; }
        public int? CustomerId { get; set; }
        public string? ApplicationUserId { get; set; }
        private int _pageSize = _defultPageSize;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
        public int PageIndex { get; set; } = 1;
    }
}
