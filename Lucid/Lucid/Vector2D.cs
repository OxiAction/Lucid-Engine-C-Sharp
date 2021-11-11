namespace Lucid.Lucid
{
    /// <summary>
    /// Simple Vector2D Class
    /// </summary>
    public class Vector2D
    {
        public float X { get; set; } = 0;
        public float Y { get; set; } = 0;

        public Vector2D(float x = 0, float y = 0)
        {
            this.X = x;
            this.Y = y;
        }
    }
}