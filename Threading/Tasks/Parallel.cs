using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Threading.Tasks
{
    public static class Parallel
    {
        public static void For(int fromInclusive, int toExclusive, Action<int> body)
        {
            for (int i = fromInclusive; i < toExclusive; ++i)
            {
                var capture = i;
                Task.Run(() => body(capture));
            }
        }

        public static void For(Int64 fromInclusive, Int64 toExclusive, Action<Int64> body)
        {
            for (Int64 i = fromInclusive; i < toExclusive; ++i)
            {
                var capture = i;
                Task.Run(() => body(capture));
            }
        }
    }
}
