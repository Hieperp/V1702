using System;
using System.Collections.Generic;
using System.Linq;

namespace TotalBase
{
    /// <summary>
    /// In AutoMapper.dll, v3.2.1.0: this Extension is PUBLIC
    /// In New Version 5.1.1, this Extension is INTERNAL
    /// SO: I copy this Extension for myself, from this site
    /// https://github.com/paulbatum/automapper/blob/master/src/AutoMapper/Internal/EnumerableExtensions.cs
    /// </summary>
    public static class EnumerableExtensions
    {
        public static void Each<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (T item in items)
            {
                action(item);
            }
        }
    }




    public static class CollectionExtensions
    {
        /// <summary>
        /// ICollection interface has List type most of time, so this Extension allows to call RemoveAll() method with the same signature like on List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="predicate"></param>
        public static void RemoveAll<T>(this ICollection<T> @this, Func<T, bool> predicate)
        {
            List<T> list = @this as List<T>;

            if (list != null)
            {
                list.RemoveAll(new Predicate<T>(predicate));
            }
            else
            {
                List<T> itemsToDelete = @this.Where(predicate).ToList();

                foreach (var item in itemsToDelete)
                {
                    @this.Remove(item);
                }
            }
        }
    }




}
