using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.ObjectPool;
using Simple.Arena;

namespace Simple.Url
{
    internal static class Cache
    {
        internal static ObjectPool<List<(IntPtr ptr, int size)>> SegmentListCache = new DefaultObjectPool<List<(IntPtr ptr, int size)>>(new ListOfPtrPooledObjectPolicy());
    }

    internal class ListOfPtrPooledObjectPolicy : PooledObjectPolicy<List<(IntPtr ptr, int size)>>
    {
        private readonly ConcurrentQueue<List<(IntPtr ptr, int size)>> _pool = new ConcurrentQueue<List<(IntPtr ptr, int size)>>();
        public override List<(IntPtr ptr, int size)> Create() => 
            _pool.TryDequeue(out var list) ? list : new List<(IntPtr ptr, int size)>();

        public override bool Return(List<(IntPtr ptr, int size)> obj)
        {
            obj.Clear();
            _pool.Enqueue(obj);

            return true;
        }
    }
}
