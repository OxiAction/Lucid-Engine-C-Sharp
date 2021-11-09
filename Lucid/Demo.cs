using Lucid.Lucid;
using System.Diagnostics;

namespace Lucid
{
    public class Demo
    {
        // an instance of the Engine
        private Engine _engine;
        // player shape
        private Shape2D _player;

        /// <summary>
        /// Constructor for the Demo Project
        /// </summary>
        public Demo()
        {
            Debug.WriteLine("Demo started...");

            Canvas canvas = new();

            // init Engine
            _engine = new Engine(canvas, new Vector2D(500, 400), "Foo");

            // subscribe to some methods
            _engine.RenderGame += Engine_RenderGame;
            _engine.UpdateGame += Engine_UpdateGame;
            _engine.DrawGame += Engine_DrawGame;
            
            _player = new Shape2D(new Vector2D(10, 10), new Vector2D(50, 50), "player", Color.Red);

            // run the Form
            Application.Run(canvas);
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

            double fooPos = Math.Sin(_engine.TimeStamp) * 100;
            _player.Position.X = (float)fooPos;
            _player.Position.Y = (float)fooPos;
        }
        private void Engine_DrawGame(object? sender, EngineEventArgs e)
        {
            // TODO: implement
        }
    }
}