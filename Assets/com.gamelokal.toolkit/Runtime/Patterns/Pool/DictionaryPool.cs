using System.Collections.Generic;

namespace GameLokal.Toolkit.Pattern
{
  /// <summary>
  ///   <para>A version of Pool.CollectionPool_2 for Dictionaries.</para>
  /// </summary>
  public class DictionaryPool<TKey, TValue> : 
    CollectionPool<Dictionary<TKey, TValue>, KeyValuePair<TKey, TValue>>
  {
  }
}
