﻿namespace Lucid.Lucid
{
    /// <summary>
    /// Simple Vector2D Class
    /// </summary>
    public class Vector2D
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Vector2D(float X = 0, float Y = 0)
        {
            this.X = X;
            this.Y = Y;
        }
    }
}