using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace NHibernate.Linq.Query
{
    /// <summary>
    /// Type related helper methods
    /// </summary>
    internal static class TypeSystem {
        private static System.Type FindIEnumerable(System.Type seqType)
        {
            if (seqType == null || seqType == typeof(string))
                return null;
            if (seqType.IsArray)
                return typeof(IEnumerable<>).MakeGenericType(seqType.GetElementType());
            if (seqType.IsGenericType) {
                foreach (System.Type arg in seqType.GetGenericArguments())
                {
                    System.Type ienum = typeof(IEnumerable<>).MakeGenericType(arg);
                    if (ienum.IsAssignableFrom(seqType)) {
                        return ienum;
                    }
                }
            }
            System.Type[] ifaces = seqType.GetInterfaces();
            if (ifaces != null && ifaces.Length > 0) {
                foreach (System.Type iface in ifaces)
                {
                    System.Type ienum = FindIEnumerable(iface);
                    if (ienum != null) return ienum;
                }
            }
            if (seqType.BaseType != null && seqType.BaseType != typeof(object)) {
                return FindIEnumerable(seqType.BaseType);
            }
            return null;
        }
        internal static System.Type GetSequenceType(System.Type elementType)
        {
            return typeof(IEnumerable<>).MakeGenericType(elementType);
        }
        internal static System.Type GetElementType(System.Type seqType)
        {
            System.Type ienum = FindIEnumerable(seqType);
            if (ienum == null) return seqType;
            return ienum.GetGenericArguments()[0];
        }
        internal static bool IsNullableType(System.Type type)
        {
            return type != null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
        internal static bool IsNullAssignable(System.Type type)
        {
            return !type.IsValueType || IsNullableType(type);
        }
        internal static System.Type GetNonNullableType(System.Type type)
        {
            if (IsNullableType(type)) {
                return type.GetGenericArguments()[0];
            }
            return type;
        }
        internal static System.Type GetMemberType(System.Reflection.MemberInfo mi)
        {
            FieldInfo fi = mi as FieldInfo;
            if (fi != null) return fi.FieldType;
            PropertyInfo pi = mi as PropertyInfo;
            if (pi != null) return pi.PropertyType;
            EventInfo ei = mi as EventInfo;
            if (ei != null) return ei.EventHandlerType;
            return null;
        }
    }
}
