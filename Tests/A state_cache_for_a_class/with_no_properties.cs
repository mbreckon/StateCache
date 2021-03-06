﻿using StateCache;
using TestClasses;
using Xunit;

namespace A_state_cache_for_a_public_class
{
   public class with_no_properties
   {
      [Fact]
      public void builds_when_no_properties_are_requested()
      {
         new StateCacheBuilder<PublicClassWithNoProperties>()
            .Build();
      }
      
      [Fact]
      public void doesnt_crash_when_asked_to_store()
      {
         new StateCacheBuilder<PublicClassWithNoProperties>()
            .Build()
            .Store(new PublicClassWithNoProperties());
      }

      [Fact]
      public void doesnt_crash_when_asked_to_restore()
      {
         var stateCache =
            new StateCacheBuilder<PublicClassWithNoProperties>()
               .Build();

         var state = new PublicClassWithNoProperties();

         stateCache.Store(state);
         stateCache.Restore(state);
      }
   }
}
