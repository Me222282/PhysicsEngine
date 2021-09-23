using PhysicsEngine.ObjectClasses.GameObjects;
using PhysicsEngine.ObjectClasses.Graphics;
using PhysicsEngine.ObjectClasses.Particles;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhysicsEngine.ObjectClasses.Core
{
    public delegate void NewFrameEventHandler();

    public class GameEngine : Form
    {
        public GameEngine(Camera camera = null)
        {
            Key.Hook();
            Key.KeyDown += OnKeyDown;
            Key.KeyUp += OnKeyUp;
            Graphics.KeyDown += OnKeyDown;
            Graphics.KeyDown += OnKeyUp;

            Graphics.MouseDown += OnMouseDown;
            Graphics.MouseUp += OnMouseUp;
            Graphics.MouseMove += OnMouseMove;
            Graphics.MouseEnter += OnMouseEnter;
            Graphics.MouseLeave += OnMouseLeave;
            Graphics.MouseHover += OnMouseHover;
            Graphics.MouseWheel += OnMouseWheel;
            Graphics.MouseClick += OnMouseClick;
            Graphics.MouseDoubleClick += OnMouseDoubleClick;

            Controls.Add(Graphics);
            Graphics.BackColor = Color.White;
            Graphics.Location = new Point(0, 0);
            Graphics.Size = new Size(Width, Height);
            Graphics.VSync = true;
            Graphics.TabIndex = 0;
            Graphics.SendToBack();

            ObjectSurface = SKSurface.Create(new SKImageInfo(Graphics.Width, Graphics.Height));
            LightingSurface = SKSurface.Create(new SKImageInfo(Graphics.Width, Graphics.Height));
            ParticleSurface = SKSurface.Create(new SKImageInfo(Graphics.Width, Graphics.Height));
            //GlowingSurface = SKSurface.Create(new SKImageInfo(Graphics.Width, Graphics.Height));

            Graphics.PaintSurface += Draw;
            Graphics.SizeChanged += AdjustGraphics;
            SizeChanged += FullSizeGraphics;

            ProcessLoop = new System.Timers.Timer(GameSpeed);
            ProcessLoop.Elapsed += TestNewFrame; // Adds an event handler to the elapsed time event

            Camera = camera ?? new Camera(Width, Height);
        }

        /// <summary>
        /// The graphics object.
        /// </summary>
        protected SKGLControl Graphics { get; set; } = new SKGLControl();

        private SKSurface ObjectSurface; // A buffer so the graphics card only has to be drawn to once
        private SKSurface LightingSurface; // A seperate drawing suface for processing lighting
        private SKSurface ParticleSurface; // A seperate drawing suface for processing particles
        //private SKSurface GlowingSurface; // A seperate drawing suface for processing graphics ontop of lighting

        public IDisplayable Background { get; set; } = new ColourDisplay(Color.White);

        public Color GameLight { get; set; }

        private bool InsideView(RectangleD rectangle)
        {
            if (rectangle.Right < 0 || rectangle.Bottom < 0 || rectangle.Left > Graphics.Width || rectangle.Top > Graphics.Height)
            {
                return false;
            }

            return true;
        }

        public RectangleD ShiftToScreen(RectangleD rectanlge)
        {
            return new RectangleD(
                ((rectanlge.X - Camera.X) * CameraXScale) + (Graphics.Width / 2), 
                ((rectanlge.Y - Camera.Y) * CameraXScale) + (Graphics.Height / 2), 
                rectanlge.Width * CameraYScale, rectanlge.Height * CameraYScale);
        }

        public RectangleD ShiftToScreen(RectangleD rectanlge, double scale)
        {
            return new RectangleD(
                ((rectanlge.X - Camera.X) * scale) + (Graphics.Width / 2),
                ((rectanlge.Y - Camera.Y) * scale) + (Graphics.Height / 2),
                rectanlge.Width * scale, rectanlge.Height * scale);
        }

        public RectangleD ShiftToScreen(RectangleD rectanlge, double xScale, double yScale)
        {
            return new RectangleD(
                ((rectanlge.X - Camera.X) * xScale) + (Graphics.Width / 2),
                ((rectanlge.Y - Camera.Y) * xScale) + (Graphics.Height / 2),
                rectanlge.Width * yScale, rectanlge.Height * yScale);
        }

        private int CompareLayerHeight(IGraphicsObject x, IGraphicsObject y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (y == null)
                {
                    return 1;
                }
                else
                {
                    int retval = x.LayerHeight.CompareTo(y.LayerHeight);

                    if (retval != 0)
                    {
                        return retval;
                    }
                    else
                    {
                        return x.Y.CompareTo(y.Y);
                    }
                }
            }
        }

        private void DrawGraphicsObject(SKCanvas canvas, IGraphicsObject obj, double xScale, double yScale)
        {
            RectangleD SL = ShiftToScreen(obj.SizeLocation, xScale, yScale);

            RectangleD displaySize = obj.Display.GetDisplayOffset();

            if (obj.UseDisplaySize)
            {
                SL.Width = displaySize.Width * xScale;
                SL.Height = displaySize.Height * yScale;
            }

            SL.X += displaySize.X * xScale;
            SL.Y += displaySize.Y * yScale;

            if (SL.Width < 1)
            {
                SL.Width = 1;
            }
            if (SL.Height < 1)
            {
                SL.Height = 1;
            }

            if (!InsideView(SL))
            {
                return;
            }

            if (!obj.Display.FromColour)
            {
                SKBitmap b = obj.Display.GetBitmapDisplay();

                bool remake = obj.ObjectState.CheckForRemake(b, SL.Width, SL.Height, obj.Display.Quality);

                if (remake)
                {
                    b = b.Resize(new SKImageInfo( // Resize it to the correct width and height
                        (int)Math.Round(SL.Width),
                        (int)Math.Round(SL.Height)),
                        obj.Display.Quality); // Quality of resize

                    obj.ObjectState.Display = b;
                }
                else
                {
                    b = obj.ObjectState.Display;
                }

                if (b != null)
                {
                    canvas.DrawBitmap(b, (float)SL.X, (float)SL.Y);
                }
            }
            else
            {
                Color colour = obj.Display.GetColourDisplay();

                canvas.DrawRect((float)SL.X, (float)SL.Y, (float)SL.Width, (float)SL.Height,
                    new SKPaint()
                    {
                        Color = new SKColor(colour.R, colour.G, colour.B, colour.A)
                    });
            }

            /*
            // Draw border
            if (obj.DrawBorder)
            {
                float thickness = (float)(obj.Border.Width * xScale) < 1 ? 1 : (float)(obj.Border.Width * yScale);

                SKPaint lineStyle = new SKPaint()
                {
                    StrokeWidth = thickness,
                    Color = new SKColor(obj.Border.Color.R, obj.Border.Color.G, obj.Border.Color.B, obj.Border.Color.A)
                };

                // Top border
                canvas.DrawLine(
                    new SKPoint(
                        (float)(SL.Left),
                        (float)(SL.Top)),
                    new SKPoint(
                        (float)(SL.Right + (thickness / 2)),
                        (float)(SL.Top)),
                    lineStyle
                );

                // Bottom border
                canvas.DrawLine(
                    new SKPoint(
                        (float)(SL.Left - (thickness / 2)),
                        (float)(SL.Bottom)),
                    new SKPoint(
                        (float)(SL.Right),
                        (float)(SL.Bottom)),
                    lineStyle
                );

                // Left border
                canvas.DrawLine(
                    new SKPoint(
                        (float)(SL.Left),
                        (float)(SL.Top - (thickness / 2))),
                    new SKPoint(
                        (float)(SL.Left),
                        (float)(SL.Bottom)),
                    lineStyle
                );

                // Right border
                canvas.DrawLine(
                    new SKPoint(
                        (float)(SL.Right),
                        (float)(SL.Top)),
                    new SKPoint(
                        (float)(SL.Right),
                        (float)(SL.Bottom + (thickness / 2))),
                    lineStyle
                );
            }
            */
        }

        private void Draw(object sender, SKPaintGLSurfaceEventArgs g)
        {
            #region Object display

            VisableObjects.Sort(CompareLayerHeight);

            SKCanvas Canvas = ObjectSurface.Canvas;

            Color c = Background.GetColourDisplay();

            Canvas.Clear(new SKColor(c.R, c.G, c.B, c.A));

            if (!Background.FromColour)
            {
                SKBitmap backing = Background.GetBitmapDisplay();

                RectangleD r = Background.GetDisplayOffset();

                backing.Resize(new SKImageInfo(Width, Height), Background.Quality);

                Canvas.DrawBitmap(backing, (float)r.X, (float)r.Y);
            }

            double xScale = CameraXScale;
            double yScale = CameraYScale;

            foreach (IGraphicsObject obj in VisableObjects)
            {
                DrawGraphicsObject(Canvas, obj, xScale, yScale);
            }

            g.Surface.Canvas.DrawSurface(ObjectSurface, 0, 0);

            #endregion

            #region Particles

            SKCanvas pCanvas = ParticleSurface.Canvas;

            pCanvas.Clear();

            foreach (IParticle particle in Particles)
            {
                if (!particle.Active)
                {
                    continue;
                }

                particle.DrawParticle(this, pCanvas);
            }

            g.Surface.Canvas.DrawSurface(ParticleSurface, 0, 0);

            #endregion

            #region Lighting

            if (DrawLighting)
            {
                SKCanvas l = LightingSurface.Canvas;

                l.Clear(new SKColor(GameLight.R, GameLight.G, GameLight.B, GameLight.A));

                for (int i = 0; i < LightObjects.Count; i++)
                {
                    LightObject light = LightObjects[i];
                    light.UpdateTracking();

                    double lx = light.X;
                    double ly = light.Y;

                    if (!light.TrackObject)
                    {
                        lx -= light.Range;
                        ly -= light.Range;
                    }

                    RectangleD SL = ShiftToScreen(new RectangleD(lx, ly, light.Size, light.Size), xScale, yScale);

                    if (SL.Width < 1)
                    {
                        SL.Width = 1;
                    }

                    if (SL.Height < 1)
                    {
                        SL.Height = 1;
                    }

                    if (!InsideView(SL))
                    {
                        continue;
                    }

                    #region Getting Light Bitmap

                    bool remake = light.ObjectState.CheckForRemake(light, SL.Width, SL.Height, LightQuality);

                    SKBitmap b;

                    if (remake)
                    {
                        int range = (int)Math.Round(light.Range * LightQuality);

                        Color colour = light.Colour;

                        SKPaint gradient = new SKPaint
                        {
                            Shader = SKShader.CreateRadialGradient(
                                new SKPoint(range, range),
                                range,
                                new SKColor[]
                                {
                            new SKColor(colour.R, colour.G, colour.B, colour.A),
                            new SKColor(colour.R, colour.G, colour.B, 0)
                                },
                                SKShaderTileMode.Clamp)
                        };

                        SKBitmap b2 = new SKBitmap(range * 2, range * 2);

                        SKCanvas canvas = new SKCanvas(b2);

                        canvas.DrawRect(0, 0, range * 2, range * 2, gradient);

                        b = b2.Resize(new SKImageInfo( // Resize it to the correct width and height
                            (int)Math.Round(SL.Width),
                            (int)Math.Round(SL.Height)),
                            light.Quality); // Quality of resize

                        b2.Dispose();

                        light.ObjectState.Display = b;
                    }
                    else
                    {
                        b = light.ObjectState.Display;
                    }

                    #endregion

                    if (b != null)
                    {
                        l.DrawBitmap(b, (float)SL.X, (float)SL.Y);
                    }
                }

                g.Surface.Canvas.DrawSurface(LightingSurface, 0, 0, new SKPaint() { BlendMode = SKBlendMode.Multiply });
            }

            #endregion

            //g.Surface.Canvas.DrawSurface(GlowingSurface, 0, 0);
        }

        public bool DrawLighting { get; set; } = true;

        public double LightQuality { get; set; } = 1;

        public int GameWidth
        {
            get
            {
                return Graphics.Width;
            }
            set
            {
                Graphics.Width = value;
            }
        }
        public int GameHeight
        {
            get
            {
                return Graphics.Height;
            }
            set
            {
                Graphics.Height = value;
            }
        }
        public int GameX
        {
            get
            {
                return Graphics.Location.X;
            }
            set
            {
                Graphics.Location = new Point(value, Graphics.Location.Y);
            }
        }
        public int GameY
        {
            get
            {
                return Graphics.Location.Y;
            }
            set
            {
                Graphics.Location = new Point(Graphics.Location.X, value);
            }
        }

        public bool FullSizeDisplay { get; set; } = true;

        private void FullSizeGraphics(object sender, EventArgs e)
        {
            if (FullSizeDisplay)
            {
                Graphics.Size = new Size(ClientSize.Width, ClientSize.Height);
            }
        }

        private void AdjustGraphics(object sender, EventArgs e)
        {
            ObjectSurface = SKSurface.Create(new SKImageInfo(Graphics.Width, Graphics.Height));
            LightingSurface = SKSurface.Create(new SKImageInfo(Graphics.Width, Graphics.Height));
            ParticleSurface = SKSurface.Create(new SKImageInfo(Graphics.Width, Graphics.Height));
            //GlowingSurface = SKSurface.Create(new SKImageInfo(Graphics.Width, Graphics.Height));
        }

        /// <summary>
        /// All object to be displayed.
        /// </summary>
        protected List<IGraphicsObject> VisableObjects { get; } = new List<IGraphicsObject>();

        /// <summary>
        /// All <see cref="IGraphicsObject"/> objects attachted to this <see cref="GameEngine"/>.
        /// </summary>
        protected List<IGraphicsObject> AllObjects { get; } = new List<IGraphicsObject>();

        /// <summary>
        /// List of all objects to be processed.
        /// </summary>
        protected List<DynamicObject> ProcessObjects { get; } = new List<DynamicObject>();

        /// <summary>
        /// List of all <see cref="StaticObject"/> and <see cref="StaticObject"/> objects.
        /// </summary>
        protected List<PhysicalObject> PhysicalObjects { get; } = new List<PhysicalObject>();

        /// <summary>
        /// List of all <see cref="StaticObject"/> objects.
        /// </summary>
        protected List<DynamicObject> DynamicObjects { get; } = new List<DynamicObject>();

        /// <summary>
        /// List of all <see cref="StaticObject"/> objects.
        /// </summary>
        protected List<StaticObject> StaticObjects { get; } = new List<StaticObject>();

        /// <summary>
        /// List of all event objects.
        /// </summary>
        protected List<EventObject> EventObjects { get; } = new List<EventObject>();

        /// <summary>
        /// List of all <see cref="LightObject"/> objects.
        /// </summary>
        protected List<LightObject> LightObjects { get; } = new List<LightObject>();

        /// <summary>
        /// List of all <see cref="IParticle"/> particles.
        /// </summary>
        protected List<IParticle> Particles { get; } = new List<IParticle>();

        public Camera Camera { get; set; }

        public bool SmoothCameraTracking { get; set; } = false;

        public double CameraXScale
        {
            get
            {
                return Graphics.Width / Camera.Width;
            }
            set
            {
                Camera.Width = Graphics.Width / value;
            }
        }

        public double CameraYScale
        {
            get
            {
                return Graphics.Height / Camera.Height;
            }
            set
            {
                Camera.Height = Graphics.Height / value;
            }
        }

        private int CurrentFrameInterval;

        private int gSpeed = 10;
        public int GameSpeed
        {
            get
            {
                return gSpeed;
            }
            set
            {
                gSpeed = value;
                ProcessLoop.Interval = value;
            }
        }

        private double gravity = 1;
        public double GlobalGravity
        {
            get
            {
                return gravity;
            }
            set
            {
                gravity = value;

                for (int i = 0; i < DynamicObjects.Count; i++)
                {
                    if (!DynamicObjects[i].UseLocalGravity)
                    {
                        DynamicObjects[i].LocalGravity = gravity;
                        DynamicObjects[i].UseLocalGravity = false;
                    }
                }
            }
        }

        private readonly System.Timers.Timer ProcessLoop; // Time the interval between frames

        private readonly KeyboardHook Key = new KeyboardHook(true)
        {
            HookAllKeys = true,
        };

        /// <summary>
        /// When keys are pressed. Excluding Ctrl, Shift and Alt.
        /// </summary>
        new public event KeyEventHandler KeyDown;

        /// <summary>
        /// When keys are released. Excluding Ctrl, Shift and Alt.
        /// </summary>
        new public event KeyEventHandler KeyUp;

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (sender == Graphics && (e.Control || e.Shift || e.Alt))
            {
                KeyDown?.Invoke(sender, e);
                OnKeyDown(e);
            }
            else if (sender != Graphics)
            {
                KeyDown?.Invoke(sender, e);
                OnKeyDown(e);
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (sender == Graphics && (e.Control || e.Shift || e.Alt))
            {
                KeyUp?.Invoke(sender, e);
                OnKeyUp(e);
            }
            else if (sender != Graphics)
            {
                KeyUp?.Invoke(sender, e);
                OnKeyUp(e);
            }
        }

        #region Mouse Events

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            OnMouseDown(e);
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            OnMouseUp(e);
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            OnMouseClick(e);
        }

        private void OnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            OnMouseDoubleClick(e);
        }

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            OnMouseWheel(e);
        }

        private void OnMouseEnter(object sender, EventArgs e)
        {
            OnMouseEnter(e);
        }

        private void OnMouseHover(object sender, EventArgs e)
        {
            OnMouseHover(e);
        }

        private void OnMouseLeave(object sender, EventArgs e)
        {
            OnMouseLeave(e);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            OnMouseMove(e);
        }

        #endregion

        /// <summary>
        /// Resests all object's properties to their original state.
        /// </summary>
        public void Reset()
        {
            Stop();
            AllObjects.ForEach(a => a.SetDefaults());
            PhysicalObjects.ForEach(p => p.UpdateProperties());
            Start();
        }

        public bool IsRunning { get; private set; } = false;

        /// <summary>
        /// Start the simulation.
        /// </summary>
        public void Start()
        {
            IsRunning = true;
            ProcessLoop.Start();
            FrameFinish = true;

            fpsCounter.Start();
        }

        /// <summary>
        /// Stop the simulation.
        /// </summary>
        public void Stop()
        {
            IsRunning = false;
            ProcessLoop.Stop();

            fpsCounter.Stop();
        }

        /// <summary>
        /// Occurs on every new frame.
        /// </summary>
        public event NewFrameEventHandler OnFrame;

        private bool OnInterval = false;
        private bool FrameFinish = false;

        // A method for calling a new frame if the previous frame has finished and it's on the interval - Used to decrease the frame rate if processing is to high
        private void TestNewFrame(object sender, EventArgs e)
        {
            if (sender == ProcessLoop)
            {
                OnInterval = true;
            }

            if (OnInterval && FrameFinish)
            {
                OnNewFrame();
            }
            else if (OnInterval)
            {
                ProcessLoop.Stop();
            }
        }

        private void OnNewFrame()
        {
            #region FPS

            try
            {
                CurrentFrameInterval = (int)fpsCounter.ElapsedMilliseconds;
            }
            catch (Exception)
            {
                CurrentFrameInterval = int.MaxValue;
            }

            try
            {
                FPS = (double)(1000 / CurrentFrameInterval);
                fpss.Add(FPS);
            }
            catch (Exception)
            {
                FPS = double.PositiveInfinity;
            }

            double fpsm = FPS;

            for (int i = 0; i < fpss.Count; i++)
            {
                fpsm += fpss[i];
            }

            AverageFPS = Math.Round(fpsm / fpss.Count, 2);
            if (fpss.Count > 50)
            {
                int dif = fpss.Count - 50;
                for (int i = 0; i < dif; i++)
                {
                    fpss.RemoveAt(0);
                }
            }

            fpsCounter.Restart();

            #endregion

            OnInterval = false;
            FrameFinish = false;

            if (ProcessLoop.Enabled == false)
            {
                ProcessLoop.Start();
            }

            OnFrame?.Invoke();
            OnFrameProcess();

            // Calcualte physics for all object that need it
            ProcessObjects.ForEach(p => p.CreateFramePhysics());
            ProcessObjects.ForEach(p => p.CreateFrameCollisions(PhysicalObjects));
            //Parallel.ForEach(ProcessObjects, p => p.CreateFramePhysics());
            //Parallel.ForEach(ProcessObjects, p => p.CreateFrameCollisions(PhysicalObjects));

            if (SmoothCameraTracking)
            {
                Camera.UpdateTracking();
            }

            PhysicalObjects.ForEach(p => p.UpdateProperties());
            //Parallel.ForEach(PhysicalObjects, p => p.UpdateProperties());

            if (!SmoothCameraTracking)
            {
                Camera.UpdateTracking();
            }

            Graphics.Invalidate();

            // Calling for next frame if this frame took longer than the speed of the simulation
            FrameFinish = true;
            EventHandler e = new EventHandler(TestNewFrame);
            try
            {
                Invoke(e, new object[] { new object(), new EventArgs() });
            }
            catch (Exception)
            {
                
            }
        }

        protected virtual void OnFrameProcess()
        {

        }

        #region FPS Counter

        private readonly Stopwatch fpsCounter = new Stopwatch();

        public double FPS { get; private set; }

        private readonly List<double> fpss = new List<double>();
        public double AverageFPS { get; private set; }

        #endregion

        #region Adding and Removing objects

        /// <summary>
        /// Adds an <see cref="IGraphicsObject"/> to this <see cref="GameEngine"/>.
        /// </summary>
        /// <param name="obj"></param>
        public virtual void AddObject(IGraphicsObject obj, bool visable = true)
        {
            if (obj == null)
            {
                return;
            }

            AllObjects.Add(obj);
            if (visable)
            {
                VisableObjects.Add(obj);
            }

            if (obj as DynamicObject != null)
            {
                DynamicObject dO = obj as DynamicObject;

                ProcessObjects.Add(dO);
                PhysicalObjects.Add(dO);
                DynamicObjects.Add(dO);

                if (!dO.UseLocalGravity)
                {
                    dO.LocalGravity = GlobalGravity;
                    dO.UseLocalGravity = false;
                }
            }
            else if (obj as StaticObject != null)
            {
                StaticObject sO = obj as StaticObject;

                PhysicalObjects.Add(sO);
                StaticObjects.Add(sO);
            }
            else if (obj as PhysicalObject != null)
            {
                PhysicalObjects.Add(obj as PhysicalObject);
            }
        }

        /// <summary>
        /// Removes an <see cref="IGraphicsObject"/> from this <see cref="GameEngine"/>.
        /// </summary>
        /// <param name="obj"></param>
        public virtual void RemoveObject(IGraphicsObject obj)
        {
            if (obj == null)
            {
                return;
            }

            AllObjects.Remove(obj);
            VisableObjects.Remove(obj);

            if (obj as DynamicObject != null)
            {
                DynamicObject dO = obj as DynamicObject;

                ProcessObjects.Remove(dO);
                PhysicalObjects.Remove(dO);
                DynamicObjects.Remove(dO);
            }
            else if (obj as StaticObject != null)
            {
                StaticObject sO = obj as StaticObject;

                PhysicalObjects.Remove(sO);
                StaticObjects.Remove(sO);
            }
            else if (obj as PhysicalObject != null)
            {
                PhysicalObjects.Remove(obj as PhysicalObject);
            }
        }

        /// <summary>
        /// Adds a <see cref="LightObject"/> to this <see cref="GameEngine"/>.
        /// </summary>
        /// <param name="obj"></param>
        public void AddLight(LightObject obj)
        {
            LightObjects.Add(obj);
        }

        /// <summary>
        /// Removes a <see cref="LightObject"/> from this <see cref="GameEngine"/>.
        /// </summary>
        /// <param name="obj"></param>
        public void RemoveLight(LightObject obj)
        {
            LightObjects.Remove(obj);
        }

        public void ShowObject(IGraphicsObject obj)
        {
            if (!VisableObjects.Contains(obj))
            {
                VisableObjects.Add(obj);
            }
        }

        public void HideObject(IGraphicsObject obj)
        {
            VisableObjects.Remove(obj);
        }

        public void AddParticle(IParticle particle)
        {
            Particles.Add(particle);
        }

        public void AddEventObject(EventObject obj)
        {
            EventObjects.Add(obj);
        }

        public void RemoveEventObject(EventObject obj)
        {
            EventObjects.Remove(obj);
        }

        #endregion
    }
}
