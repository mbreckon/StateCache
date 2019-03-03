using System;

namespace StateCache
{
   internal class AutoCacheState<T> : IDisposable
   {
      readonly StateCache<T> cache;
      readonly T statefulObject;

      public AutoCacheState(StateCache<T> cache, T statefulObject)
      {
         this.cache = cache;
         this.statefulObject = statefulObject;
         cache.Store(statefulObject);
      }

      public void Dispose()
      {
         cache.Restore(statefulObject);
      }
   }
}
