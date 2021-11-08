using Lucid.Lucid;
using System.Diagnostics;

namespace Lucid
{
    public class Demo
    {
        // an instance of the Engine
        private Engine engine;

        /// <summary>
        /// Constructor for the Demo Project
        /// </summary>
        public Demo()
        {
            Debug.WriteLine("Demo started...");

            Canvas canvas = new();

            // init Engine
            engine = new Engine(canvas, new Vector2D(500, 400), "Foo");

            // subscribe to some methods
            engine.RenderGame += Engine_RenderGame;
            engine.UpdateGame += Engine_UpdateGame;
            engine.DrawGame += Engine_DrawGame;

            // run the Form
            Application.Run(canvas);
        }

        private void Engine_RenderGame(object? sender, EngineEventArgs e)
        {
            if (e.CanvasGraphics == null)
            {
                return;
            }

            Graphics g = e.CanvasGraphics;
            g.Clear(Color.Yellow);
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