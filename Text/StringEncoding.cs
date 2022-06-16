using SystemPlus.Extensions;

namespace SystemPlus.Text
{
    public sealed class StringEncoding : IEncoding<string, string, string>
    {
        public string Encode(string value, string key)
        {
            byte[] valAr = value.ToCharArray().ChangeType(ch => (byte)ch);
            byte[] keyAr = key.ToCharArray().ChangeType(ch => (byte)ch);
            byte[] retAr = new byte[valAr.Length];
            byte hash = (byte)key.GetHashCode();
            for (int i = 0; i < valAr.Length; i++)
                retAr[i] = (byte)(valAr[i] + ~keyAr[i % keyAr.Length] + hash);

            return new string(retAr.ChangeType(b => (char)b));
        }

        public string Decode(string value, string key)
        {
            byte[] valAr = new byte[value.Length];
            for (int i = 0; i < value.Length; i++)
                valAr[i] = (byte)value[i];
            byte[] keyAr = key.ToCharArray().ChangeType(ch => (byte)ch);
            byte[] retAr = new byte[valAr.Length];
            byte hash = (byte)key.GetHashCode();
            for (int i = 0; i < valAr.Length; i++)
                retAr[i] = (byte)(valAr[i] - ~keyAr[i % keyAr.Length] - hash);

            return new string(retAr.ChangeType(b => (char)b));
        }
    }
}
