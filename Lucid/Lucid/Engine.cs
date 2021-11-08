using System.Diagnostics;

namespace Lucid.Lucid
{
    class Engine
    {
        private Canvas _canvas = new();
        /// <summary>
        /// Canvas
        /// </summary>
        public Canvas Canvas
        {
            get => _canvas;
            set { _canvas = value; UpdateCanvas(); }
        }

        private Vector2D _screenSize = new();
        /// <summary>
        /// ScreenSize
        /// </summary>
        public Vector2D ScreenSize
        {
            get => _screenSize;
            set { _screenSize = value; UpdateCanvas(); }
        }

        private string _text = "undefined";
        /// <summary>
        /// Text
        /// </summary>
        public string Text
        {
            get => _text;
            set { _text = value; UpdateCanvas(); }
        }

        // Thread for the main rendering loop
        private Thread _loopThread;
        // the starting time in milliseconds
        private long _timeStart = 0;
        // total time running in millisconds
        private long _timeStamp = 0;
        // the amount of time (in milliseconds) to simulate each time update() runs
        private long _simulationTimestep = 1000 / 60;
        // the cumulative amount of in-app time that hasn't been simulated yet
        private long _frameDelta = 0;
        // the last time the loop was run
        private long _lastFrameTimeMs = 0;
        // an exponential moving average of the frames per second
        private int _fps = 10;
        // a factor that affects how heavily to weight more recent seconds performance when calculating the average frames per second
        private float _fpsAlpha = 0.9f;
        // the minimum duration between updates to the frames-per-second estimate - higher values means more accuray
        private long _fpsUpdateInterval = 1000;
        // the timestamp (in milliseconds) of the last time the "fps" moving average was updated
        private long _lastFPSUpdate = 0;
        // the number of frames delivered since the last time the "fps" moving average was updated (i.e. since "lastFpsUpdate").
        private int _framesSinceLastFPSUpdate = 0;
        // the number of times update() is called in a given frame
        private int _numUpdateSteps = 0;
        // the minimum amount of time in milliseconds that must pass since the last frame was executed before another frame can be executed
        private long _minFrameDelay = 0;
        // whether the simulation has fallen too far behind real time
        //private bool _panic = false;

        /// <summary>
        /// Engine
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="ScreenSize"></param>
        /// <param name="Text"></param>
        public Engine(Canvas canvas, Vector2D ScreenSize, string Text = "Untitled")
        {
            this.Canvas = canvas;
            this.ScreenSize = ScreenSize;
            this.Text = Text;

            _timeStart = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            UpdateCanvas();

            this.Canvas.Paint += OnRender;

            _loopThread = new Thread(Loop);
            _loopThread.Start();


        }

        /// <summary>
        /// Main rendering loop
        /// </summary>
        /// <param name="obj"></param>
        private void Loop(object? obj)
        {
            while (_loopThread.IsAlive)
            {
                _timeStamp = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - _timeStart;


                // throttle the frame rate (if MinFrameDelay is set to a non-zero value)
                if (_timeStamp < _lastFrameTimeMs + _minFrameDelay)
                {
                    continue;
                }

                // frameDelta is the cumulative amount of in-app time that hasn't been simulated yet.
                _frameDelta += _timeStamp - _lastFrameTimeMs;
                _lastFrameTimeMs = _timeStamp;

                // render begin (debug related stuff)
                // TODO: implement - for now just output FPS
                Debug.WriteLine(_fps);

                // calculate frames per second
                if (_timeStamp > _lastFPSUpdate + _fpsUpdateInterval)
                {
                    // Compute the new exponential moving average.
                    _fps =
                        // Divide the number of frames since the last FPS update by the
                        // amount of time that has passed to get the mean frames per second
                        // over that period. This is necessary because slightly more than a
                        // second has likely passed since the last update.
                        (int)(_fpsAlpha * _framesSinceLastFPSUpdate * 1000 / (_timeStamp - _lastFPSUpdate) +
                        (1 - _fpsAlpha) * _fps);

                    // Reset the frame counter and last-updated timestamp since their
                    // latest values have now been incorporated into the FPS estimate.
                    _lastFPSUpdate = _timeStamp;
                    _framesSinceLastFPSUpdate = 0;
                }
                _framesSinceLastFPSUpdate++;

                _numUpdateSteps = 0;
                while (_frameDelta >= _simulationTimestep)
                {
                    // update stuff - e.g. positions x/y etc...
                    //RenderUpdate(SimulationTimestep / 1000);
                    OnUpdateGame();
                    _frameDelta -= _simulationTimestep;

                    // sanity check: bail if we run the loop too many times. Triggers
                    // after 4 seconds because most browsers will alert after 5 seconds
                    // TODO: edit
                    if (++_numUpdateSteps >= 240)
                    {
                        //_panic = true;
                        break;
                    }
                }

                // draw stuff
                OnDrawGame();

                // if the canvas handle is created...
                if (this.Canvas.IsHandleCreated)
                {
                    // ... refresh it to trigger its Paint method (redraw)
                    this.Canvas.BeginInvoke((MethodInvoker)delegate { Canvas.Refresh(); });
                }

                // little break for the OS
                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// Updates visual properties of the Canvas
        /// </summary>
        public void UpdateCanvas()
        {
            this.Canvas.Size = new Size((int)this.ScreenSize.X, (int)this.ScreenSize.Y);
            this.Canvas.Text = this.Text;
        }

        private void OnRender(object? sender, PaintEventArgs e)
        {
            // check for subscriber
            if (RenderGame != null)
            {
                EngineEventArgs args = new();
                args.Type = EngineEventTypes.RENDER_GAME;
                args.CanvasGraphics = e.Graphics;
                RenderGame(this, args);
            }
        }

        public event EventHandler<EngineEventArgs>? RenderGame;

        public void OnDrawGame()
        {
            // check for subscriber
            if (DrawGame != null)
            {
                EngineEventArgs args = new();
                args.Type = EngineEventTypes.DRAW_GAME;
                DrawGame(this, args);
            }
        }

        public event EventHandler<EngineEventArgs>? DrawGame;

        public void OnUpdateGame()
        {
            // check for subscriber
            if (UpdateGame != null)
            {
                EngineEventArgs args = new()
                {
                    Type = EngineEventTypes.UPDATE_GAME
                };
                UpdateGame(this, args);
            }
        }

        public event EventHandler<EngineEventArgs>? UpdateGame;
    }

    public static class EngineEventTypes
    {
        public const string RENDER_GAME = "RENDER_GAME";
        public const string UPDATE_GAME = "UPDATE_GAME";
        public const string DRAW_GAME = "DRAW_GAME";
    }

    public class EngineEventArgs : EventArgs
    {
        public string Type { get; set; } = "";
        public Graphics? CanvasGraphics { get; set; } = null;
    }
}
