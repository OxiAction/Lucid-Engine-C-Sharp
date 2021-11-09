using System.Diagnostics;

namespace Lucid.Lucid
{
    class Engine
    {
        // canvas being drawn
        private Canvas _canvas = new();
        public Canvas Canvas { get => _canvas; set { _canvas = value; UpdateCanvas(); }}
        // canvas / window size
        private Vector2D _screenSize = new();
        public Vector2D ScreenSize { get => _screenSize; set { _screenSize = value; UpdateCanvas(); }}

        // window title
        private string _text = "undefined";
        public string Text { get => _text; set { _text = value; UpdateCanvas(); }}
        // Thread for the main rendering loop
        private Thread _loopThread;
        // the starting time in milliseconds
        private long _timeStart = 0;
        // total time running in millisconds
        private long _timeStamp = 0;
        public long TimeStamp { get => _timeStamp; }
        // the amount of time (in milliseconds) to simulate each time update() runs
        private long _simulationTimestep = 1000 / 60;
        // the cumulative amount of in-app time that hasn't been simulated yet
        private long _frameDelta = 0;
        // the last time the loop was run
        private long _lastFrameTimeMs = 0;
        // an exponential moving average of the frames per second
        private int _fps = 10;
        public int Fps { get => _fps; }

        // a factor that affects how heavily to weight more recent seconds performance when calculating the average frames per second
        private float _fpsAlpha = 0.9f;
        // the minimum duration between updates to the frames-per-second estimate - higher values means more accuray
        private long _fpsUpdateInterval = 1000;
        // the timestamp (in milliseconds) of the last time the "fps" moving average was updated
        private long _lastFPSUpdate = 0;
        // the number of frames delivered since the last time the "fps" moving average was updated (i.e. since "lastFpsUpdate").
        private int _framesSinceLastFPSUpdate = 0;
        // set max fps
        private long _maxFPS = 0;
        public long MaxFPS
        { 
            get => _maxFPS;
            set
            {
                if (value == 0 || value < 0)
                {
                    // TODO: implement - stop engine
                    _maxFPS = 0;
                }
                else
                {
                    _maxFPS = value;
                }
            }
        }
        // states if engine is running - or not
        public bool _running = false;

        private static List<Shape2D> _shapes2D = new();

        /// <summary>
        /// Engine Constructor
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="ScreenSize"></param>
        /// <param name="Text"></param>
        public Engine(Canvas canvas, Vector2D screenSize, string text = "Untitled")
        {
            this.Canvas = canvas;
            this.ScreenSize = screenSize;
            this.Text = text;

            _loopThread = new Thread(Loop);
        }

        /// <summary>
        /// Starts the Engine's rendering
        /// </summary>
        public void Start()
        {
            if (_running)
            {
                return;
            }
            
            if (!_loopThread.IsAlive)
            {
                _loopThread.Start();
            }

            _timeStart = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            this.Canvas.Paint += OnRender;

            _running = true;
        }

        /// <summary>
        /// Stops the Engine's rendering
        /// </summary>
        public void Stop()
        {
            if (!_running)
            {
                return;
            }

            _timeStart = _timeStamp = 0;

            this.Canvas.Paint -= OnRender;

            _running = false;
        }

        public static void AddShape2D(Shape2D shape)
        {
            _shapes2D.Add(shape);
        }

        /// <summary>
        /// Main rendering loop
        /// </summary>
        /// <param name="obj"></param>
        private void Loop(object? obj)
        {
            while (_loopThread.IsAlive)
            {
                if (!_running)
                {
                    continue;
                }

                _timeStamp = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - _timeStart;

                // throttle the frame rate (if MaxFPS is set to a non-zero value)
                if (_maxFPS > 0 && _timeStamp < _lastFrameTimeMs + (1000 / _maxFPS))
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
                
                while (_frameDelta >= _simulationTimestep)
                {
                    // update stuff - e.g. positions x/y etc...
                    //RenderUpdate(SimulationTimestep / 1000);
                    OnUpdateGame();
                    _frameDelta -= _simulationTimestep;
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
            Graphics graphics = e.Graphics;
            foreach (Shape2D shape in _shapes2D)
            {
                graphics.FillRectangle(new SolidBrush(shape.Color), shape.Position.X, shape.Position.Y, shape.Size.X, shape.Size.Y);
            }

            // check for subscriber
            if (RenderGame != null)
            {
                EngineEventArgs args = new();
                args.Type = EngineEventTypes.RenderGame;
                args.CanvasGraphics = graphics;
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
                args.Type = EngineEventTypes.DrawGame;
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
                    Type = EngineEventTypes.UpdateGame
                };
                UpdateGame(this, args);
            }
        }

        public event EventHandler<EngineEventArgs>? UpdateGame;
    }

    public enum EngineEventTypes
    {
        Undefined,
        RenderGame,
        UpdateGame,
        DrawGame
    }

    public class EngineEventArgs : EventArgs
    {
        public EngineEventTypes Type { get; set; } = EngineEventTypes.Undefined;
        public Graphics? CanvasGraphics { get; set; } = null;
    }
}