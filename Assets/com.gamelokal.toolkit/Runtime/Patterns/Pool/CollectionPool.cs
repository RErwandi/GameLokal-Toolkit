using System;
using System.Collections.Generic;

namespace GameLokal.Toolkit
{
    /// <summary>
    ///   <para>A Collection such as List, HashSet, Dictionary etc can be pooled and reused by using a CollectionPool.</para>
    /// </summary>
    public class CollectionPool<TCollection, TItem> where TCollection : class, ICollection<TItem>, new()
    {
        internal static readonly ObjectPool<TCollection> s_Pool = new ObjectPool<TCollection>((Func<TCollection>) (() => new TCollection()), actionOnRelease: ((Action<TCollection>) (l => l.Clear())));

        public static TCollection Get() => CollectionPool<TCollection, TItem>.s_Pool.Get();

        public static PooledObject<TCollection> Get(out TCollection value) => CollectionPool<TCollection, TItem>.s_Pool.Get(out value);

        public static void Release(TCollection toRelease) => CollectionPool<TCollection, TItem>.s_Pool.Release(toRelease);
    }
}