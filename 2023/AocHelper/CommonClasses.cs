using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AocHelper
{
    public enum Direction
    {
        None,
        North,
        South,
        East,
        West
    }

    public record Point(long X, long Y);
}
