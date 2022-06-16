using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
