namespace SystemPlus.UI
{
    public class Animation
    {
        protected int currentFrame = 0;
        protected string[] frames;
        public string GetFrame(int frame) => frames[frame];
        public string GetNextFrame()
        {
            string toReturn = frames[currentFrame];
            currentFrame++;
            if (currentFrame >= frames.Length)
                currentFrame = 0;
            return toReturn;
        }
        public void Reset() => currentFrame = 0;
    }

    public sealed class LoadingAnim_1 : Animation
    {
        public LoadingAnim_1()
        {
            frames = new string[] {
                "│",
                "╱",
                "──",
                "╲",
            };
        }
    }

    public sealed class BigLoadingAnim_1 : Animation
    {
        public BigLoadingAnim_1()
        {
            frames = new string[] {
                " │ \n │ \n │ ",
                "  ╱\n ╱ \n╱  ",
                "   \n─────\n   ",
                "╲  \n ╲ \n  ╲",
            };
        }
    }
}
