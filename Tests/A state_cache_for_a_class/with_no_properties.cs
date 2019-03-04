using StateCache;
using TestClasses;
using Xunit;

namespace A_state_cache_for_a_class
{
   public class with_no_properties
   {
      [Fact]
      public void builds_when_no_properties_are_requested()
      {
         new StateCacheBuilder<ClassWithNoProperties>()
            .Build();
      }
      
      [Fact]
      public void doesnt_crash_when_asked_to_store()
      {
         new StateCacheBuilder<ClassWithNoProperties>()
            .Build()
            .Store(new ClassWithNoProperties());
      }

      [Fact]
      public void doesnt_crash_when_asked_to_restore()
      {
         var stateCache =
            new StateCacheBuilder<ClassWithNoProperties>()
               .Build();

         var state = new ClassWithNoProperties();

         stateCache.Store(state);
         stateCache.Restore(state);
      }
   }
}
