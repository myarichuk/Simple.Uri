using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.ObjectPool;

namespace Simple.Url
{
    internal static class Cache
    {
        internal readonly static ObjectPool<List<StringSegment>> SegmentListCache = new DefaultObjectPool<List<StringSegment>>(new ListOfPtrPooledObjectPolicy<StringSegment>());
    }

    internal class ListOfPtrPooledObjectPolicy<T> : PooledObjectPolicy<List<T>>
    {
        private readonly ConcurrentQueue<List<T>> _pool = new ConcurrentQueue<List<T>>();

        public override List<T> Create() => 
            _pool.TryDequeue(out var list) ? list : new List<T>();

        public override bool Return(List<T> obj)
        {
            obj.Clear();
            _pool.Enqueue(obj);

            return true;
        }
    }
}
