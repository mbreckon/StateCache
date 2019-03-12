using StateCache;
using Tests.TestClasses;
using Xunit;

namespace A_state_cache_for_a_public_class
{
   public class with_only_methods
   {
      [Fact]
      public void builds_when_no_properties_are_requested()
      {
         new StateCacheBuilder<PublicClassWithOnlyMethods>()
            .Build();
      }

      [Fact]
      public void doesnt_crash_when_asked_to_store()
      {
         new StateCacheBuilder<PublicClassWithOnlyMethods>()
            .Build()
            .Store(new PublicClassWithOnlyMethods());
      }

      [Fact]
      public void doesnt_crash_when_asked_to_restore()
      {
         var stateCache =
            new StateCacheBuilder<PublicClassWithOnlyMethods>()
               .Build();

         var state = new PublicClassWithOnlyMethods();

         stateCache.Store(state);
         stateCache.Restore(state);
      }
   }
}
