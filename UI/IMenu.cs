using SystemPlus.Vectors;

namespace SystemPlus.UI
{
    public interface IMenu<TRet>
    {
        public TRet Show(Vector2Int pos);

        public void Render(Vector2Int pos);

        public void SetSelected(int _selected);
    }
}
