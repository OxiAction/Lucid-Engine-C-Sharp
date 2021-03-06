using Lucid.Lucid;
using System.Diagnostics;

namespace Lucid
{
    public class Demo
    {
        // an instance of the Engine
        private Engine _engine;
        // player shape
        private Entity _player;

        /// <summary>
        /// Demo Constructor
        /// </summary>
        public Demo()
        {
            Debug.WriteLine("Demo started...");

            // init Engine
            _engine = new Engine(new Canvas(), new Vector2D(500, 400), "Foo")
            {
                MaxFPS = 60
            };

            // listener for core Engine methods
            _engine.RenderGame += Engine_RenderGame;
            _engine.UpdateGame += Engine_UpdateGame;
            _engine.DrawGame += Engine_DrawGame;

            // setup player Entity
            _player = new Entity()
            {
                X = 10,
                Y = 10,
                Width = 75,
                Height = 104,
                Speed = 200,
                ImageOffsetX = 0,
                ImageOffsetY = 0
            };
            _player.SetImage(System.AppDomain.CurrentDomain.BaseDirectory + "../../../Assets/player.png");
            _engine.AddEntity(_player);

            // key event listener
            _engine.Canvas.KeyDown += new KeyEventHandler(Canvas_KeyDown);
            _engine.Canvas.KeyUp += new KeyEventHandler(Canvas_KeyUp);

            // start Engine
            _engine.Start();

            // run the Form
            Application.Run(_engine.Canvas);
        }

        private void Canvas_KeyDown(object? sender, KeyEventArgs e)
        {
            SetPlayerMovement(e, true);
        }

        private void Canvas_KeyUp(object? sender, KeyEventArgs e)
        {
            SetPlayerMovement(e, false);
        }

        private void SetPlayerMovement(KeyEventArgs e, bool value)
        {
            if (e.KeyCode == Keys.Right)
            {
                _player.MovementDirections.Right = value;
            }
            if (e.KeyCode == Keys.Left)
            {
                _player.MovementDirections.Left = value;
            }
            if (e.KeyCode == Keys.Down)
            {
                _player.MovementDirections.Down = value;
            }
            if (e.KeyCode == Keys.Up)
            {
                _player.MovementDirections.Up = value;
            }
        }

        private void Engine_RenderGame(object? sender, EngineEventArgs e)
        {
            if (e.CanvasGraphics == null)
            {
                return;
            }

            // TODO: implement
        }

        private void Engine_UpdateGame(object? sender, EngineEventArgs e)
        {
            // TODO: implement
        }
        private void Engine_DrawGame(object? sender, EngineEventArgs e)
        {
            // TODO: implement
        }
    }
}