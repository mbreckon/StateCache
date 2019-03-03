using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using StateCache;
using System.Collections.Generic;

namespace Benchmark
{
   public class BenchmarkStatefulObject
   {
      public int Value1 { get; set; }
      public List<string> Value2 { get; set; }
   }

   [ClrJob(baseline: true)]
   [RPlotExporter, RankColumn]
   public class Test1
   {
      BenchmarkStatefulObject statefulObject;
      StateCache<BenchmarkStatefulObject> stateCache;

      [Params(1000, 10000)]
      public int N;

      [GlobalSetup]
      public void Setup()
      {
         statefulObject = new BenchmarkStatefulObject()
         {
            Value1 = 2,
            Value2 = new List<string>() { "hello", "world" }
         };

         stateCache =
            new StateCacheBuilder<BenchmarkStatefulObject>()
               .Property("Value1")
               .Property("Value2")
               .Build();
      }

      [Benchmark]
      public void StateCache()
      {
         stateCache.Store(statefulObject);

         statefulObject.Value1 = 32;
         statefulObject.Value2 = new List<string>() { "goodbye", "world" };

         stateCache.Restore(statefulObject);
      }

      [Benchmark]
      public void Traditional()
      {
         var oldValue1 = statefulObject.Value1;
         var oldValue2 = statefulObject.Value2;

         statefulObject.Value1 = 6;
         statefulObject.Value2 = new List<string>() { "goodbye", "world" };

         statefulObject.Value1 = oldValue1;
         statefulObject.Value2 = oldValue2;
      }

      [Benchmark]
      public void Using()
      {
         using (stateCache.AutoRestoreToPreviousState(statefulObject))
         {
            statefulObject.Value1 = 32;
            statefulObject.Value2 = new List<string>() { "goodbye", "world" };
         }
      }
   }

   class Program
   {
      static void Main()
      {
         BenchmarkRunner.Run<Test1>();
      }
   }
}
