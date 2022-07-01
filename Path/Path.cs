namespace SystemPlus.Path
{
    public abstract class Path
    {
        public abstract IPoint GetPointAt(float percent, float threshold);
    }
}
