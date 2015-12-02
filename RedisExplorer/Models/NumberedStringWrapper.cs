using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisExplorer.Models
{
    public class NumberedStringWrapper : StringWrapper
    {
        public int RowNumber { get; set; }
    }
}
