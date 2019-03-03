using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace StateCache
{
   public class StateCacheBuilder<T>
   {
      readonly List<string> properties = new List<string>();

      public StateCacheBuilder<T> Property(string propertyName)
      {
         properties.Add(propertyName);
         return this;
      }

      public StateCache<T> Build()
      {
         var typeBuilder =
            TypeBuilder(typeof(StateCache<T>).Name + "Auto")
               .Implementing(typeof(StateCache<T>))
               .WithDefaultConstructor();

         var propertiesInT = Properties(typeof(T));

         var fields =
            propertiesInT
            .Select(p =>
               typeBuilder.DefineField(
                  p.Name.ToLower(),
                  p.DeclaringType,
                  FieldAttributes.Private
               )
            ).ToList<FieldInfo>();

         AddStoreMethod(
            typeBuilder,
            propertiesInT,
            fields
         );

         AddRestoreMethod(
            typeBuilder,
            propertiesInT,
            fields
         );

         return (StateCache<T>)
            Activator.CreateInstance(
               typeBuilder.CreateType()
            );
      }

      private void AddStoreMethod(
         TypeBuilder typeBuilder,
         List<PropertyInfo> properties,
         List<FieldInfo> fields)
      {
         var storeMethodILGen =
            typeBuilder
               .DefineMethod(
                  "Store",
                  MethodAttributes.Public | MethodAttributes.Virtual,
                  typeof(void),
                  new Type[] { typeof(T) })
               .GetILGenerator();

         for (int i = 0; i < properties.Count; i++)
         {
            storeMethodILGen.Emit(OpCodes.Ldarg_0);
            storeMethodILGen.Emit(OpCodes.Ldarg_1);

            storeMethodILGen.EmitCall(
               OpCodes.Callvirt,
               properties[i].GetMethod,
               null
            );

            storeMethodILGen.Emit(OpCodes.Stfld, fields[i]);
         }

         storeMethodILGen.Emit(OpCodes.Ret);
      }

      private void AddRestoreMethod(
         TypeBuilder typeBuilder,
         List<PropertyInfo> properties,
         List<FieldInfo> fields)
      {
         var restoreMethodILGen =
            typeBuilder
               .DefineMethod(
                  "Restore",
                  MethodAttributes.Public | MethodAttributes.Virtual,
                  typeof(void),
                  new Type[] { typeof(T) })
               .GetILGenerator();

         for (int i = 0; i < properties.Count; i++)
         {
            restoreMethodILGen.Emit(OpCodes.Ldarg_1);
            restoreMethodILGen.Emit(OpCodes.Ldarg_0);
            restoreMethodILGen.Emit(OpCodes.Ldfld, fields[i]);

            restoreMethodILGen.EmitCall(
               OpCodes.Callvirt,
               properties[i].SetMethod,
               null
            );
         }

         restoreMethodILGen.Emit(OpCodes.Ret);
      }

      private List<PropertyInfo> Properties(Type t) =>
         properties
         .Select(p => t.GetProperty(p))
         .ToList();

      private static TypeBuilder TypeBuilder(string name)
         => AppDomain
            .CurrentDomain
            .DefineDynamicAssembly(
               new AssemblyName("stateCacheDyanamic"),
               AssemblyBuilderAccess.RunAndSave
            )
            .DefineDynamicModule(
               "stateCacheDyanamic",
               "stateCacheDyanamic.dll"
            )
            .DefineType(name, TypeAttributes.Public);
   }
}
