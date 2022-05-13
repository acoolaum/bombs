using System;
using System.Collections.Generic;

namespace MyCompany.Library.Utils
{
    public static class TypeUtil
    {
        public static IEnumerable<Type> GetBaseTypesAndInterfaces(this Type type)
        {
            if (type == null)
            {
                yield break;
            }

            foreach (var i in type.GetInterfaces())
            {
                yield return i;
            }

            var currentBaseType = type.BaseType;
            while (currentBaseType != null)
            {
                yield return currentBaseType;
                currentBaseType = currentBaseType.BaseType;
            }
        }
    }
}