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
        public Color Color { get; set; }

        public Shape2D(Vector2D position, Vector2D size, string id = "", Color color = new Color())
        {
            this.Position = position;
            this.Size = size;
            this.ID = id;
            this.Color = color;
            Engine.AddShape2D(this);
        }
    }
}