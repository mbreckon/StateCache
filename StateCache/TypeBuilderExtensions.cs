using System;
using System.Reflection;
using System.Reflection.Emit;

namespace StateCache
{
   internal static class TypeBuilderExtensions
   {
      public static TypeBuilder WithDefaultConstructor(
         this TypeBuilder typeBuilder)
      {
         typeBuilder
            .DefineConstructor(
               MethodAttributes.Public,
               CallingConventions.Standard,
               new Type[] { }
            )
            .GetILGenerator()
            .Emit(OpCodes.Ret);

         return typeBuilder;
      }

      public static TypeBuilder Implementing(
         this TypeBuilder typeBuilder,
         Type interfaceType)
      {
         typeBuilder.AddInterfaceImplementation(interfaceType);
         return typeBuilder;
      }
   }
}
