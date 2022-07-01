namespace SystemPlus
{
    public readonly struct ArrayRef<T>
    {
        public readonly int index;
        private readonly T[] array;
        public T Value { get => array[index]; set => array[index] = value; }

        public ArrayRef(int _index, T[] _array)
        {
            index = _index;
            array = _array;
        }

        public static implicit operator T(ArrayRef<T> ar)
            => ar.Value;

        public static T operator +(ArrayRef<T> ar, int i)
            => new ArrayRef<T>(ar.index + i, ar.array);
        public static T operator -(ArrayRef<T> ar, int i)
           => new ArrayRef<T>(ar.index - i, ar.array);
        public static T operator *(ArrayRef<T> ar, int i)
           => new ArrayRef<T>(ar.index * i, ar.array);
        public static T operator /(ArrayRef<T> ar, int i)
           => new ArrayRef<T>(ar.index / i, ar.array);
    }
}
