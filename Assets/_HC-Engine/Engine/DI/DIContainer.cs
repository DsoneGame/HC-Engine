using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

namespace Engine.DI
{
    public static class DIContainer
    {
        private static class ObjectsPovider<TType>
        {
            internal static float CleanEvery =  3f;
            internal static float LastCleanTime = Time.unscaledTime;
            internal readonly static List<TType> Values = new List<TType>();
        }

        /// <summary>
        /// Here we register just one value, Always override to the last VALUE added.
        /// </summary>
        /// <typeparam name="T"> The Data Type that we will saved. </typeparam>
        public static void RegisterAsSingle<T>(T value)
        {
            if (value == null) throw new NullReferenceException("eventObject has a null value!...");

            ObjectsPovider<T>.Values.Clear();
            ObjectsPovider<T>.Values.Add(value);
        }

        /// <summary>
        /// Register the value in the list with the other values of the same type (TType). if the value already exist, It will not added.
        /// </summary>
        public static void Register<TType>(TType value, bool cleanNulls = false)
        {
            if (cleanNulls || ObjectsPovider<TType>.LastCleanTime + ObjectsPovider<TType>.CleanEvery <= Time.unscaledTime)
                CleanNull<TType>();

            ObjectsPovider<TType>.Values.Add(value);
        }

        /// <summary>
        /// Register range of values in the list with the other values with the same type (TType).
        /// </summary>
        public static void RegisterRange<TType>(IEnumerable<TType> collection)
        {
            if (collection == null) throw new ArgumentNullException();

            ObjectsPovider<TType>.Values.AddRange(collection);
        }

        /// <summary>
        /// Get the last object added non null of type T.
        /// </summary>
        /// <typeparam name="TSource"> The Data Type that we will saved. </typeparam>
        /// <typeparam name="TResult"> The type of object that we will return. </typeparam>
        public static TResult AsSingle<TSource, TResult>()
        {
            List<TSource> values = ObjectsPovider<TSource>.Values;
            for (int i = values.Count - 1; 0 <= i; i--)
            {
                object value = values[i];
                if (value != null && !value.Equals(null) && value is TResult) return (TResult)value;
            }

            return default(TResult);
        }

        /// <summary>
        /// Get the last object added non null of type T.
        /// </summary>
        /// <typeparam name="TSource"> The Data Type that we will saved. </typeparam>
        public static TSource AsSingle<TSource>()
        {
            List<TSource> values = ObjectsPovider<TSource>.Values;
            for (int i = values.Count - 1; 0 <= i; i--)
            {
                if (values[i] != null && !values[i].Equals(null)) return values[i];
            }

            return default(TSource);
        }

        /// <summary>
        /// To a list all values of type TType and return it as an array of TType.
        /// </summary>
        /// <typeparam name="T"> The Data Type that we will searching. </typeparam>
        public static IList<TType> Collect<TType>()
        {
            return ObjectsPovider<TType>.Values;
        }

        /// <summary>
        /// Return the last value that non null where the func is returning true.
        /// </summary>
        /// <returns> We return the value that we selection on the func parameter, We have exeption if we don't have a value!... </returns>
        public static TSource WhereLast<TSource>(Func<TSource, bool> func)
        {
            return ObjectsPovider<TSource>.Values.NonNull().Where(func).Last();
        }

        /// <summary>
        /// Return the last value that non null where the func is returning true.
        /// </summary>
        /// <returns> We return the value that we selection on the func parameter, We return null if we don't have a value!... </returns>
        public static TSource WhereLastOrDefault<TSource>(Func<TSource, bool> func)
        {
            return ObjectsPovider<TSource>.Values.NonNull().Where(func).LastOrDefault();
        }

        /// <summary>
        /// ToArray all values of type TType and return it as an array of TType.
        /// </summary>
        /// <typeparam name="T"> The Data Type that we will searching. </typeparam>
        public static TType[] ToArray<TType>()
        {
            return ObjectsPovider<TType>.Values.ToArray();
        }

        /// <summary>
        /// Clear all values of type (TType).
        /// </summary>
        public static void Clear<TType>()
        {
            ObjectsPovider<TType>.Values.Clear();
        }

        /// <summary>
        /// Clean all the null values.
        /// </summary>
        public static void CleanNull<TType>()
        {
            ObjectsPovider<TType>.LastCleanTime = Time.unscaledTime;
            ObjectsPovider<TType>.Values.RemoveAll(item => (item == null || item.Equals(null)));
        }

        /// <summary>
        /// The clean every is a value that make clean all nulls every time that we will set. The default value is 3.
        /// </summary>
        public static void SetCleanEvery<TType>(float value)
        {
            ObjectsPovider<TType>.CleanEvery = value;
        }
    }
}
