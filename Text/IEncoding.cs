namespace SystemPlus.Text
{
    public interface IEncoding<TValue, TValOut, TKey>
    {
        TValOut Encode(TValue value, TKey key);
        TValue Decode(TValOut value, TKey key);
    }
}
