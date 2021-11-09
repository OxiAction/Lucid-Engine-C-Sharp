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
        /// Demo Constructor
        /// </summary>
        public Demo()
        {
            Debug.WriteLine("Demo started...");

            Canvas canvas = new();

            // init Engine
            _engine = new Engine(canvas, new Vector2D(500, 400), "Foo")
            {
                MaxFPS = 10
            };

            // subscribe to some methods
            _engine.RenderGame += Engine_RenderGame;
            _engine.UpdateGame += Engine_UpdateGame;
            _engine.DrawGame += Engine_DrawGame;
            
            _player = new Shape2D(new Vector2D(10, 10), new Vector2D(50, 50), "player", Color.Red);

            _engine.Start();

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
            
            // testing: move shape
            double fooPos = Math.Sin(_engine.TimeStamp) * 100;
            _player.Position.X = (float)fooPos;
            _player.Position.Y = (float)fooPos;

            // testing: stop engine after 5 seconds of running
            if (_engine.TimeStamp > 5000)
            {
                _engine.Stop();
            }
        }
        private void Engine_DrawGame(object? sender, EngineEventArgs e)
        {
            // TODO: implement
        }
    }
}