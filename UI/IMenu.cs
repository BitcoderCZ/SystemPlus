using SystemPlus.Vectors;

namespace SystemPlus.UI
{
    public interface IMenu<TRet>
    {
        public TRet Show(Vector2Int pos);

        public TRet Show(Vector2Int pos, Image image);

        public TRet Show(Vector2Int pos, ConsoleImage image);

        public void Render(Vector2Int pos);

        public void Render(Vector2Int pos, Image image);

        public void Render(Vector2Int pos, ConsoleImage image);

        public void SetSelected(int _selected);
    }
}
