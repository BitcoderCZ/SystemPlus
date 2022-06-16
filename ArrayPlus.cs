using System.Collections;
using System.Collections.Generic;

namespace SystemPlus
{
    public sealed class ArrayPlus<TKey, TValue> : IEnumerable, IEnumerable<(TKey index, TValue value)>
    {
        private List<(TKey key, TValue value)> list = new List<(TKey key, TValue value)>();

        public int Length => list.Count;

        public int Count => list.Count;

        public bool IsSynchronized => false;

        public bool IsReadOnly => false;

        public (TKey index, TValue value) this[int index]
        {
            get => list[index];
            set => list[index] = value;
        }

        public TValue this[TKey key]
        {
            get
            {
                for (int i = 0; i < Length; i++)
                    if (Equals(list[i].key, key))
                        return list[i].value;
                return default;
            }
            set
            {
                for (int i = 0; i < Length; i++)
                    if (Equals(list[i].key, key))
                    {
                        list[i] = (list[i].key, value);
                        return;
                    }

                list.Add((key, value));
            }
        }

        public ArrayPlus()
        {
            list = new List<(TKey key, TValue value)>();
        }

        public void Add(TKey key, TValue value) => list.Add((key, value));

        public void SetValue(TKey key, TValue value)
        {
            for (int i = 0; i < Length; i++)
                if (Equals(list[i].key, key))
                {
                    list[i] = (list[i].key, value);
                    return;
                }

            list.Add((key, value));
        }

        public void SetValueAt(int index, TValue value) => list[index] = (list[index].key, value);

        public TValue GetValue(TKey key)
        {
            for (int i = 0; i < Length; i++)
                if (Equals(list[i].key, key))
                    return list[i].value;
            return default;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            for (int i = 0; i < Length; i++)
                if (Equals(list[i].key, key))
                {
                    value = list[i].value;
                    return true;
                }
            value = default;
            return false;
        }

        public TValue GetValueAt(int index) => list[index].value;

        public void Clear() => list.Clear();

        public bool Contains(TValue item)
        {
            for (int i = 0; i < Length; i++)
                if (Equals(list[i].value, item))
                    return true;
            return false;
        }

        public void CopyTo(TValue[] array, int arrayIndex)
        {
            for (int i = arrayIndex; i < array.Length && i - arrayIndex < Length; i++)
                array[i] = list[i - arrayIndex].value;
        }

        public bool Remove(TValue item)
        {
            for (int i = 0; i < Length; i++)
                if (Equals(list[i].value, item))
                {
                    list.RemoveAt(i);
                    return true;
                }
            return false;
        }

        public void RemoveAt(int index) => list.RemoveAt(index);

        public int IndexOf(TKey key) => list.IndexOf((key, GetValue(key)));

        public bool Any() => Length > 0;

        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < Length; i++)
                yield return list[i];
        }

        IEnumerator<(TKey index, TValue value)> IEnumerable<(TKey index, TValue value)>.GetEnumerator()
        {
            for (int i = 0; i < Length; i++)
                yield return list[i];
        }
    }
}
