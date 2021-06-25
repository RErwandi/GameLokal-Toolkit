using System;

namespace GameLokal.Toolkit.Pattern
{
    /// <summary>
    ///   <para>Provides a static implementation of Pool.ObjectPool_1.</para>
    /// </summary>
    public class GenericPool<T> where T : class, new()
    {
        internal static readonly ObjectPool<T> s_Pool = new ObjectPool<T>((Func<T>) (() => new T()));

        public static T Get() => GenericPool<T>.s_Pool.Get();

        public static PooledObject<T> Get(out T value) => GenericPool<T>.s_Pool.Get(out value);

        public static void Release(T toRelease) => GenericPool<T>.s_Pool.Release(toRelease);
    }
}