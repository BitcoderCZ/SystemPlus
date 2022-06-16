namespace SystemPlus.ClassSupport
{
    public interface ISaveSupport<T> where T : ISaveSupport<T>
    {
        T Default { get; }

        void Save(string path);
        void Load(string path, bool exept = true);

        string SaveToString();
        void LoadFromString(string data, bool exept = true);
    }
}
