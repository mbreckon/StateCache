using StateCache;
using System;
using System.Collections.Generic;

namespace Example
{
   public class ExampleStatefulObject
   {
      public int Value1 { get; set; }
      public List<string> Value2 { get; set; } 
   }

   public class ExampleStatefulObject2
   {
      public int Value3 { get; set; }
   }

   class Program
   {
      static void Main()
      {
         Console.WriteLine("Examples - Partial store: Value2 only");
         var stateCache =
            new StateCacheBuilder<ExampleStatefulObject>()
               .Property("Value2")
               .Build();

         ExampleOne(stateCache);
         Console.WriteLine();
         ExampleTwo(stateCache);
      }

      static void ExampleOne(StateCache<ExampleStatefulObject> stateCache)
      {
         var state =
            new ExampleStatefulObject()
            {
               Value1 = 5,
               Value2 = new List<string>() { "hello", "world" }
            };

         Console.WriteLine("Example 1: stateCache.Store(state); ... stateCache.Restore(state);");
         Console.WriteLine($"Before store... Value 1 = {state.Value1}");
         Console.WriteLine($"Before store... Value 2 = {string.Join(" ", state.Value2)}");

         stateCache.Store(state);

         state.Value2 = new List<string>() { "goodbye", "world" };

         Console.WriteLine($"After modification... Value 1 = {state.Value1}");
         Console.WriteLine($"After modification... Value 2 = {string.Join(" ", state.Value2)}");

         stateCache.Restore(state);

         Console.WriteLine("After restore");
         Console.WriteLine($"After restore... Value 1 = {state.Value1}");
         Console.WriteLine($"After restore... Value 2 = {string.Join(" ", state.Value2)}");
      }

      static void ExampleTwo(StateCache<ExampleStatefulObject> stateCache)
      {
         var state =
            new ExampleStatefulObject()
            {
               Value1 = 5,
               Value2 = new List<string>() { "hello", "world" }
            };

         Console.WriteLine("Example 2: using (stateCache.AutoRestoreToPreviousState(state) { ... }");
         Console.WriteLine($"Before store... Value 1 = {state.Value1}");
         Console.WriteLine($"Before store... Value 2 = {string.Join(" ", state.Value2)}");

         using (stateCache.AutoRestoreToPreviousState(state))
         {
            state.Value1 = 32;
            state.Value2 = new List<string>() { "goodbye", "world" };

            Console.WriteLine($"After modification... Value 1 = {state.Value1}");
            Console.WriteLine($"After modification... Value 2 = {string.Join(" ", state.Value2)}");
         }

         Console.WriteLine($"After restore... Value 1 = {state.Value1}");
         Console.WriteLine($"After restore... Value 2 = {string.Join(" ", state.Value2)}");
      }
   }
}
