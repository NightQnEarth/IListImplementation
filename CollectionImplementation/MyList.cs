using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace CollectionImplementation
{
    public class MyList<T> : IList<T>
    {
        private const int DefaultCapacity = 4;
        private const int EnlargeArrayCoefficient = 2;

        private T[] items;

        public MyList(int capacity = DefaultCapacity) => items = new T[capacity];

        public int Count { get; private set; }

        public bool IsReadOnly => false;

        public T this[int index]
        {
            get
            {
                ValidateIndex(index, Count);

                return items[index];
            }
            set
            {
                ValidateIndex(index, Count);

                items[index] = value;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
                yield return items[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(T item)
        {
            if (Count == items.Length)
                items = GetEnlargedArray(items);

            items[Count] = item;
            Count++;
        }

        public void Clear()
        {
            Array.Clear(items, 0, Count);
            Count = 0;
        }

        public bool Contains(T item)
        {
            if (item is IComparable)
                return Array.BinarySearch(items, item) >= 0;

            return items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array), "array is null.");

            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), "arrayIndex is less than 0.");

            if (array.Length - arrayIndex < Count)
                throw new ArgumentException("The number of elements in the source ICollection<T> is greater " +
                                            "than the available space from arrayIndex to the end of the " +
                                            "destination array.");

            Array.Copy(items, 0, array, arrayIndex, Count);
        }

        public bool Remove(T item)
        {
            var indexOfRemovedItem = IndexOf(item);

            if (indexOfRemovedItem < 0)
                return false;

            RemoveAt(indexOfRemovedItem);

            return true;
        }

        public int IndexOf(T item)
        {
            Func<T, T, bool> itemsComparisionCondition;

            if (item != null)
                itemsComparisionCondition = (firstItem, secondItem) => item.Equals(secondItem);
            else
                itemsComparisionCondition = (firstItem, secondItem) => secondItem is null;

            return Array.FindIndex(items, sourceItem => itemsComparisionCondition(item, sourceItem));
        }

        public void Insert(int index, T item)
        {
            ValidateIndex(index, Count);

            if (Count == items.Length)
                items = GetEnlargedArray(items);

            for (var i = Count - 1; i >= index; i--)
                items[i + 1] = items[i];

            items[index] = item;
            Count++;
        }

        public void RemoveAt(int index)
        {
            ValidateIndex(index, Count);

            for (var i = index; i < Count; i++)
                if (i == Count - 1)
                    items[i] = default;
                else
                    items[i] = items[i + 1];

            Count--;
        }

        private static T[] GetEnlargedArray(T[] sourceArray, int enlargeCoefficient = EnlargeArrayCoefficient)
        {
            var newArray = new T[sourceArray.Length * enlargeCoefficient];
            Array.Copy(sourceArray, newArray, sourceArray.Length);

            return newArray;
        }

        [AssertionMethod]
        private static void ValidateIndex(int index, int maxCount)
        {
            if (index < 0 || index >= maxCount)
                throw new ArgumentOutOfRangeException(nameof(index), "index is not a valid index in the MyList<T>.");
        }
    }
}