using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DotNetMarche.Utils.EntityFramework
{
  public static class BasicExtension
   {
        /// <summary>
        /// Reorders the entities in an entity collection by the keySelector function.
        /// </summary>
        /// <remarks>
        /// This class uses reflection to re-write the slots in the underlying hashset used by the entity
        /// collection to contain the references.
        ///
        /// The only action that should be taken on the entity collection after this method has been called
        /// is via its enumerators, as the hash set is not left in a consistant state.
        /// </remarks>
        /// <typeparam name="TSource">The type of the entity objects.</typeparam>
        /// <typeparam name="TKey">The type of the key to order by.</typeparam>
        /// <param name="source">The entity collection source to re-order.</param>
        /// <param name="keySelector">The key selector function that returns the key to order by.</param>
        public static void ReorderEntities<TSource, TKey>(
            this EntityCollection<TSource> source, 
            Func<TSource, TKey> keySelector)
            where TSource : class, IEntityWithRelationships
        {
            HashSet<TSource> relatedEntities = (HashSet<TSource>)
                typeof(EntityCollection<TSource>).GetProperty("RelatedEntities", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(source, null);
            // re-order the sets slots to fix the ordering, (but mess up everything else probably)
            Type slotType = typeof(HashSet<TSource>).Assembly.GetType("System.Collections.Generic.HashSet`1+Slot").MakeGenericType(new Type[] { typeof(TSource) });

            FieldInfo slotField = typeof(HashSet<TSource>).GetField("m_slots", BindingFlags.Instance | BindingFlags.NonPublic);
            FieldInfo lastIndex = typeof(HashSet<TSource>).GetField("m_lastIndex", BindingFlags.Instance | BindingFlags.NonPublic);
            System.Array slots = Array.CreateInstance(slotType, relatedEntities.Count);
            int index = 0;
            foreach (TSource obj in new List<TSource>(relatedEntities).OrderBy(keySelector))
            {
                object test = Activator.CreateInstance(slotType);
                slotType.GetField("hashCode", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(test, obj.GetHashCode());
                slotType.GetField("value", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(test, obj);
                slotType.GetField("next", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(test, -1);
                slots.SetValue(test, index++);
            }
            slotField.SetValue(relatedEntities, slots);
            lastIndex.SetValue(relatedEntities, slots.Length);
        }
    } 
}
