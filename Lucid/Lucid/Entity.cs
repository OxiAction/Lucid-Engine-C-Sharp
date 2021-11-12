using System.Drawing.Drawing2D;

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
        private int _imageOffsetX = 0;
        public int ImageOffsetX { get => _imageOffsetX; set => _imageOffsetX = value; }
        private int _imageOffsetY = 0;
        public int ImageOffsetY { get => _imageOffsetY; set => _imageOffsetY = value; }
        private Image? _image = null;
        public Image? Image { get => _image; set => _image = value; }

        public EntityMovementDirections MovementDirections = new();

        public void SetImage(string path)
        {
            _image = Image.FromFile(path);
        }

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

        public void OnRender(Graphics graphics)
        {
            if (_image != null)
            {
                TextureBrush textureBrush = new(_image);
                Matrix matrix = new();
                matrix.Translate(_x + _imageOffsetX, _y + _imageOffsetY, MatrixOrder.Append);
                textureBrush.Transform = matrix;
                // TODO: implement set tileset image region dynamically
                graphics.FillRectangle(textureBrush, new RectangleF(_x, _y, _width, _height));
            }
            else
            {
                // TODO: implement set color dynamically
                graphics.FillRectangle(new SolidBrush(Color.Red), _x, _y, _width, _height);
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
