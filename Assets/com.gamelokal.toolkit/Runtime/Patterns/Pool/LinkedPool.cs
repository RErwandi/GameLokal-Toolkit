using System;

namespace GameLokal.Toolkit
{
  /// <summary>
  ///   <para>A linked list version of Pool.IObjectPool_1.</para>
  /// </summary>
  public class LinkedPool<T> : IDisposable, IObjectPool<T> where T : class
  {
    private readonly Func<T> m_CreateFunc;
    private readonly Action<T> m_ActionOnGet;
    private readonly Action<T> m_ActionOnRelease;
    private readonly Action<T> m_ActionOnDestroy;
    private readonly int m_Limit;
    internal LinkedPool<T>.LinkedPoolItem m_PoolFirst;
    internal LinkedPool<T>.LinkedPoolItem m_NextAvailableListItem;
    private bool m_CollectionCheck;

    public LinkedPool(
      Func<T> createFunc,
      Action<T> actionOnGet = null,
      Action<T> actionOnRelease = null,
      Action<T> actionOnDestroy = null,
      bool collectionCheck = true,
      int maxSize = 10000)
    {
      if (createFunc == null)
        throw new ArgumentNullException(nameof (createFunc));
      if (maxSize <= 0)
        throw new ArgumentException(nameof (maxSize), "Max size must be greater than 0");
      this.m_CreateFunc = createFunc;
      this.m_ActionOnGet = actionOnGet;
      this.m_ActionOnRelease = actionOnRelease;
      this.m_ActionOnDestroy = actionOnDestroy;
      this.m_Limit = maxSize;
      this.m_CollectionCheck = collectionCheck;
    }

    public int CountInactive { get; private set; }

    public T Get()
    {
      T obj2;
      if (this.m_PoolFirst == null)
      {
        obj2 = this.m_CreateFunc();
      }
      else
      {
        LinkedPool<T>.LinkedPoolItem poolFirst = this.m_PoolFirst;
        obj2 = poolFirst.value;
        this.m_PoolFirst = poolFirst.poolNext;
        poolFirst.poolNext = this.m_NextAvailableListItem;
        this.m_NextAvailableListItem = poolFirst;
        this.m_NextAvailableListItem.value = default (T);
        --this.CountInactive;
      }
      Action<T> actionOnGet = this.m_ActionOnGet;
      if (actionOnGet != null)
        actionOnGet(obj2);
      return obj2;
    }

    public PooledObject<T> Get(out T v) => new PooledObject<T>(v = this.Get(), (IObjectPool<T>) this);

    public void Release(T item)
    {
      if (this.m_CollectionCheck)
      {
        for (LinkedPool<T>.LinkedPoolItem linkedPoolItem = this.m_PoolFirst; linkedPoolItem != null; linkedPoolItem = linkedPoolItem.poolNext)
        {
          if ((object) linkedPoolItem.value == (object) item)
            throw new InvalidOperationException("Trying to release an object that has already been released to the pool.");
        }
      }
      Action<T> actionOnRelease = this.m_ActionOnRelease;
      if (actionOnRelease != null)
        actionOnRelease(item);
      if (this.CountInactive < this.m_Limit)
      {
        LinkedPool<T>.LinkedPoolItem linkedPoolItem = this.m_NextAvailableListItem;
        if (linkedPoolItem == null)
          linkedPoolItem = new LinkedPool<T>.LinkedPoolItem();
        else
          this.m_NextAvailableListItem = linkedPoolItem.poolNext;
        linkedPoolItem.value = item;
        linkedPoolItem.poolNext = this.m_PoolFirst;
        this.m_PoolFirst = linkedPoolItem;
        ++this.CountInactive;
      }
      else
      {
        Action<T> actionOnDestroy = this.m_ActionOnDestroy;
        if (actionOnDestroy != null)
          actionOnDestroy(item);
      }
    }

    public void Clear()
    {
      if (this.m_ActionOnDestroy != null)
      {
        for (LinkedPool<T>.LinkedPoolItem linkedPoolItem = this.m_PoolFirst; linkedPoolItem != null; linkedPoolItem = linkedPoolItem.poolNext)
          this.m_ActionOnDestroy(linkedPoolItem.value);
      }
      this.m_PoolFirst = (LinkedPool<T>.LinkedPoolItem) null;
      this.m_NextAvailableListItem = (LinkedPool<T>.LinkedPoolItem) null;
      this.CountInactive = 0;
    }

    public void Dispose() => this.Clear();

    internal class LinkedPoolItem
    {
      internal LinkedPool<T>.LinkedPoolItem poolNext;
      internal T value;
    }
  }
}