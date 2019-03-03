using System;

namespace StateCache
{
   public static class StateCacheExtensions
   {
      public static IDisposable AutoRestoreToPreviousState<T>(
         this StateCache<T> cache,
         T statefulObject)
      {
         return new AutoCacheState<T>(cache, statefulObject);
      }
   }
}
