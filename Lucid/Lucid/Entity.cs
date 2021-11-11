using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucid.Lucid
{
    internal class Entity
    {
        private float _x = 0;
        public float X { get => _x; set => _x = value; }
        private float _y = 0;
        public float Y { get => _y; set => _y = value; }
        private float _width = 0;
        public float Width { get => _width; set => _width = value; }
        private float _height = 0;
        public float Height { get => _height; set => _height = value; }
        private float _speed = 0;
        public float Speed { get => _speed; set => _speed = value; }

        public EntityMovementDirections MovementDirections = new();
        

        public void OnUpdateGame(float delta)
        {
            if (MovementDirections.Right)
            {
                _x += _speed * delta;
            }
            if (MovementDirections.Left)
            {
                _x -= _speed * delta;
            }
            if (MovementDirections.Down)
            {
                _y += _speed * delta;
            }
            if (MovementDirections.Up)
            {
                _y -= _speed * delta;
            }
        }
    }

    public class EntityMovementDirections
    {
        public bool Right { get; set; } = false;
        public bool Left { get; set; } = false;
        public bool Up { get; set; } = false;
        public bool Down { get; set; } = false;
    }
}
