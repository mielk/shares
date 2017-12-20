using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Utils
{
    public static class ContainerHelperMethods
    {

        public static bool HasTheSameItems<T>(this IEnumerable<T> list1, IEnumerable<T> list2)
        {
            List<T> removableList2 = list2.ToList();
            foreach (T baseObject in list1)
            {
                bool isFound = false;
                foreach (T comparedObject in removableList2)
                {
                    Type comparedObjectType = comparedObject.GetType();
                    if (comparedObjectType.IsPrimitive || comparedObjectType == typeof(Decimal) || comparedObjectType == typeof(String))
                    {
                        isFound = (baseObject.Equals(comparedObject));
                    }
                    else
                    {
                        isFound = (Object.ReferenceEquals(baseObject, comparedObject));
                    }

                    if (isFound)
                    {
                        removableList2.Remove(comparedObject);
                        break;
                    }

                }

                if (!isFound)
                {
                    return false;
                }

            }

            return (removableList2.Count == 0);

        }

        public static bool HasEqualItems<T>(this IEnumerable<T> list1, IEnumerable<T> list2)
        {
            List<T> removableList = list2.ToList();
            foreach (T baseObject in list1)
            {
                bool isFound = false;
                foreach (T comparedObject in removableList)
                {
                    if (baseObject.Equals(comparedObject))
                    {
                        removableList.Remove(comparedObject);
                        isFound = true;
                        break;
                    }
                }

                if (!isFound)
                {
                    return false;
                }

            }

            return (removableList.Count == 0);

        }

        public static bool HasEqualItemsInTheSameOrder<T>(this T[] array1, T[] array2)
        {
            if (array1.Length != array2.Length) return false;

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] == null)
                {
                    if (array2[i] != null)
                    {
                        return false;
                    }
                } else if (!array1[i].Equals(array2[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool HasTheSameValues<K, V>(this Dictionary<K, V> baseDict, Dictionary<K, V> comparedDict)
        {

            if (baseDict.Count != comparedDict.Count) return false;

            foreach (var key in baseDict.Keys)
            {
                V value;
                V comparedValue;

                try
                {
                    baseDict.TryGetValue(key, out value);
                    comparedDict.TryGetValue(key, out comparedValue);
                    if (!value.Equals(comparedValue))
                    {
                        return false;
                    }
                } 
                catch (Exception ex)
                {
                    return false;
                }
            }

            return true;

        }

    }
}
