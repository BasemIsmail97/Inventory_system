using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shards
{
    public record PagenatedResult<TData>(int pageIndex, int pageSize, int TotalCount, IEnumerable<TData> Data)
    {

    }
}
