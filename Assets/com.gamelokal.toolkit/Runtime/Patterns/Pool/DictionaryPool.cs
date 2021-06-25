using System.Collections.Generic;

namespace GameLokal.Toolkit
{
  /// <summary>
  ///   <para>A version of Pool.CollectionPool_2 for Dictionaries.</para>
  /// </summary>
  public class DictionaryPool<TKey, TValue> : 
    CollectionPool<Dictionary<TKey, TValue>, KeyValuePair<TKey, TValue>>
  {
  }
}
