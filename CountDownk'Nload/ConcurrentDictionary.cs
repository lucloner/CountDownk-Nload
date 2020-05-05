using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CountDownk_Nload
{
    internal class ConcurrentDictionary<T> : ConcurrentDictionary<string, IList<object>>
    {
    }
}