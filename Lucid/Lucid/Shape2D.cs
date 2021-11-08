namespace Lucid.Lucid
{
    /// <summary>
    /// Simple Shape2D Class
    /// </summary>
    class Shape2D
    {
        public Vector2D Position { get; set; }
        public Vector2D Size { get; set; }
        public string ID { get; set; }

        public Shape2D(Vector2D Position, Vector2D Size, string ID = "")
        {
            this.Position = Position;
            this.Size = Size;
            this.ID = ID;
        }
    }
}