using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public class ResourceParamethers
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string? SortColumn { get; set; }
        public string SortOrder { get; set; } = "asc";
    }
}
