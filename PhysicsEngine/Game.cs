using PhysicsEngine.ObjectClasses.Core;
using PhysicsEngine.ObjectClasses.GameObjects;
using PhysicsEngine.ObjectClasses.Graphics;
using PhysicsEngine.ObjectClasses.Particles;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PhysicsEngine
{
    public class Game : GameEngine
    {
        public Game()
        {
            #region Size, Location of Labels and Forms

            Location = new Point(0, 0);
            Size = new Size(1600, 900);
            Icon = new Icon("PhysicsEIcon.ico");

            Shown += boderChange;
            void boderChange(object sender, EventArgs e)
            {
                FormBorderStyle = FormBorderStyle.None;
                WindowState = FormWindowState.Maximized;
            }

            // 
            // Controls
            // 

            Controls.Add(DisplayCon);
            DisplayCon.Anchor = AnchorStyles.None;
            DisplayCon.Location = new Point((Width / 2) - (DisplayCon.Width / 2), (Height / 2) - (DisplayCon.Height / 2));
            DisplayCon.DefualtPos = DisplayCon.Location;
            DisplayCon.Size = new Size(400, 400);
            DisplayCon.Text = "E:Change Target Object; ArrowKeys:Control Target Object; WASD:Control Non-Target Object; Left-Click:View Properties of Clicked Object; Right-Click:Restart; TAB:Switch out of Full Screen; CTRL:Zoom out; SHIFT:Zoom in; SPACE:Switch to 10FPS; ESC:Exit, P:Game Settings, G:World Generation M:Hide Mouse";
            DisplayCon.BorderStyle = BorderStyle.FixedSingle;
            DisplayCon.BackColor = Color.FromArgb(240, 240, 240);
            DisplayCon.Font = new Font("Serif", 15, FontStyle.Regular);
            DisplayCon.Hide();

            // 
            // Console
            // 

            Controls.Add(console);
            console.Anchor = AnchorStyles.None;
            console.Location = new Point(0, 0);
            console.DefualtPos = console.Location;
            console.Size = new Size(110, 130);
            console.Text = "ee";
            console.BorderStyle = BorderStyle.FixedSingle;
            console.BackColor = Color.FromArgb(240, 240, 240);
            console.Font = new Font("Serif", 15, FontStyle.Regular);

            // 
            // Console 2
            // 

            Controls.Add(console2);
            console2.Anchor = AnchorStyles.None;
            console2.Location = new Point(0, 0);
            console2.DefualtPos = console2.Location;
            console2.Size = new Size(110, 200);
            console2.Text = "";
            console2.BorderStyle = BorderStyle.FixedSingle;
            console2.BackColor = Color.FromArgb(240, 240, 240);
            console2.Font = new Font("Serif", 15, FontStyle.Regular);
            console2.Hide();

            // 
            // Collision Display
            // 

            Controls.Add(CollisionDisplay);
            CollisionDisplay.Anchor = AnchorStyles.None;
            CollisionDisplay.Size = new Size(200, 100);
            CollisionDisplay.Location = new Point(Width - CollisionDisplay.Size.Width, 0);
            CollisionDisplay.DefualtPos = CollisionDisplay.Location;
            CollisionDisplay.Text = "ee";
            CollisionDisplay.BorderStyle = BorderStyle.FixedSingle;
            CollisionDisplay.BackColor = Color.FromArgb(240, 240, 240);
            CollisionDisplay.Font = new Font("Serif", 15, FontStyle.Regular);

            // 
            // FPS Display
            // 

            Controls.Add(FPSDisplay);
            FPSDisplay.Anchor = AnchorStyles.None;
            FPSDisplay.AutoSize = true;
            FPSDisplay.Location = new Point(0, Height - FPSDisplay.Height);
            FPSDisplay.DefualtPos = FPSDisplay.Location;
            FPSDisplay.Text = "ee";
            FPSDisplay.BorderStyle = BorderStyle.FixedSingle;
            FPSDisplay.BackColor = Color.FromArgb(240, 240, 240);
            FPSDisplay.Font = new Font("Serif", 15, FontStyle.Regular);

            Graphics.SendToBack();

            #endregion

            #region Setup Game Objects

            GlobalGravity = 1.75;
            GameSpeed = 1;
            speed = GameSpeed;

            // Add Game Boundaries and Platforms
            BackColor = GameColours[0];
            Platforms = new Platforms(this, GameColours[1], GameColours[2]);

            // Add Players
            //Players = new PlayerMovement(this, Color.FromArgb(255, 100, 100, 255), Color.FromArgb(255, 100, 255, 100));
            Players = new PlayerMovement(this, GameColours[3], GameColours[4]);

            #endregion

            //BackColor = Color.White;

            CameraWidth = 1600;
            CameraHeight = 900;
            Camera.Width = CameraWidth;
            Camera.Height = CameraHeight;

            GameLight = Color.FromArgb(245, 0, 0, 2);
            LightQuality = 0.05;

            //DrawLighting = false;
            SmoothCameraTracking = true;

            Candle.CreateCandleDisplay();
            Candle.LightColour = Color.FromArgb(245, 221, 177);

            new Candle(1000, 1000, 255, 1000, this);
            new Candle(-3000, -2400, 100, 600, this);
            new Candle(1000, -1000, 50, 800, this);
            new Candle(3900, 2000, 250, 500, this);
            new Candle(-3000, 3000, 150, 2000, this);

            Start();
        }

        #region Game Objects

        private readonly PlayerMovement Players;

        protected readonly Platforms Platforms;

        /*
        protected readonly Color[] GameColours = { 
            Color.FromArgb(232, 242, 61), 
            Color.FromArgb(136, 204, 41), 
            Color.FromArgb(6, 128, 27), 
            Color.FromArgb(15, 78, 99), 
            Color.FromArgb(18, 34, 52) 
        };*/

        protected readonly Color[] GameColours = {
            Color.White,
            Color.FromArgb(200, 200, 200),
            Color.Red,
            Color.FromArgb(255, 100, 100, 255),
            Color.FromArgb(255, 100, 255, 100)
        };

        #endregion

        #region Winform Objects

        private readonly CLabel DisplayCon = new CLabel();

        private readonly CLabel console = new CLabel();

        private readonly CLabel console2 = new CLabel();

        private readonly CLabel CollisionDisplay = new CLabel();

        private readonly CLabel FPSDisplay = new CLabel();

        private const int DistanceY = 10;

        private delegate void ChangeTextCallback(string text, Label applience);

        private void ChangeText(string text, Label applience)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (applience.InvokeRequired)
            {
                ChangeTextCallback d = new ChangeTextCallback(ChangeText);
                applience.Invoke(d, new object[] { text, applience });
            }
            else
            {
                applience.Text = text;
            }
        }

        #endregion

        private double scale = 1;

        private readonly int speed;

        private readonly double CameraWidth;
        private readonly double CameraHeight;

        private readonly int GBD = 8000;
        private readonly int PNumber = 60;
        private readonly int PWidth = 1000;
        private readonly int PHeight = 500;

        private int Multiplier = 1;

        private readonly Random Random = new Random(0);

        private void UpdateCameraSize()
        {
            if (ClientSize.Width > ClientSize.Height)
            {
                double heightPercent = (ClientSize.Height * scale) / (ClientSize.Width * scale);

                Camera.Width = CameraWidth / scale;

                Camera.Height = CameraWidth * heightPercent / scale;
            }
            else
            {
                double widthPercent = (ClientSize.Width * scale) / (ClientSize.Height * scale);

                Camera.Height = CameraHeight / scale;

                Camera.Width = CameraHeight * widthPercent / scale;
            }
        }

        protected override void OnFrameProcess()
        {
            //Platforms.PlatformColour = GameColours.GetRandom();

            DynamicObject Track = Camera.Track as DynamicObject;

            if (Track != null)
            {
                ChangeText($"X:{(int)Math.Round(Track.X):D4}, Y:{(int)Math.Round(Track.Y):D4}, VX:{Track.VelocityX:F2}, VY:{Track.VelocityY:F2}", console);

                /*
                ChangeText($"" +
                    $"Top:{(Track.TopCollision.Count != 0 ? Track.TopCollision[0].UID : false.ToString())} " +
                    $"Bottom:{(Track.BottomCollision.Count != 0 ? Track.BottomCollision[0].UID : false.ToString())} " +
                    $"Left:{(Track.LeftCollision.Count != 0 ? Track.LeftCollision[0].UID : false.ToString())} " +
                    $"Right:{(Track.RightCollision.Count != 0 ? Track.RightCollision[0].UID : false.ToString())}",
                    CollisionDisplay
                );*/

                Players.MovePlayer(Track);
            }

            ChangeText($"{AverageFPS} FPS", FPSDisplay);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            Players.KeyDown(this, e);

            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
            else if (e.Control)
            {
                scale -= 0.1;

                if (scale < 0.01)
                {
                    scale = 0.01;
                }

                UpdateCameraSize();

                //ScaleCamera = scale;
            }
            else if (e.Shift)
            {
                scale += 0.1;

                UpdateCameraSize();

                //ScaleCamera = scale;
            }
            else if (e.KeyCode == Keys.Space && GameSpeed != 100)
            {
                GameSpeed = 100;
            }
            else if (e.KeyCode == Keys.Tab)
            {
                if (FormBorderStyle == FormBorderStyle.None)
                {
                    FormBorderStyle = FormBorderStyle.Sizable;
                    WindowState = FormWindowState.Normal;
                }
                else
                {
                    FormBorderStyle = FormBorderStyle.None;
                    WindowState = FormWindowState.Maximized;
                }
            }
            else if (e.KeyCode == Keys.L)
            {
                //new Candle(Camera.Track.X, Camera.Track.Y, (byte)Random.Next(50, 255), Random.Next(100, 2000), this);
                AddLight(new LightObject(Color.FromArgb((byte)Random.Next(50, 255), Candle.LightColour), Random.Next(100, 2000), Camera.Track.X, Camera.Track.Y));
            }
            else if (e.KeyCode == Keys.Q)
            {
                Multiplier *= 2;

                RegeneratePlatforms();
            }
            else if (e.KeyCode == Keys.E)
            {
                if (Multiplier < 2)
                {
                    return;
                }

                Multiplier /= 2;

                RegeneratePlatforms();
            }
            else if (e.KeyCode == Keys.OemPeriod)
            {
                DrawLighting = !DrawLighting;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            Players.KeyUp(e);

            if (e.KeyCode == Keys.Space)
            {
                GameSpeed = speed;
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            UpdateCameraSize();

            //ScaleCamera = scale;

            CLabel.BoundarySize = Size;

            if (console.IsDefualtPos)
            {
                console.Location = new Point(DistanceY, DistanceY);
                console.DefualtPos = console.Location;
            }
            if (console2.IsDefualtPos)
            {
                console2.Location = new Point(DistanceY, console.Height + (DistanceY * 2));
                console2.DefualtPos = console2.Location;
            }
            if (CollisionDisplay.IsDefualtPos)
            {
                CollisionDisplay.Location = new Point(ClientSize.Width - (CollisionDisplay.Size.Width + DistanceY), DistanceY);
                CollisionDisplay.DefualtPos = CollisionDisplay.Location;
            }
            if (DisplayCon.IsDefualtPos)
            {
                DisplayCon.Location = new Point((Width / 2) - (DisplayCon.Width / 2), (ClientSize.Height / 2) - (DisplayCon.Height / 2));
                DisplayCon.DefualtPos = DisplayCon.Location;
            }
            if (FPSDisplay.IsDefualtPos)
            {
                FPSDisplay.Location = new Point(0, ClientSize.Height - FPSDisplay.Height);
                FPSDisplay.DefualtPos = FPSDisplay.Location;
            }/*
            if (ISetDefualtLocation && !GISetDefualtLocation)
            {
                Interface.Location = new Point((Width / 2) - (Interface.Width / 2), DistanceY);
            }
            else if (ISetDefualtLocation && GISetDefualtLocation)
            {
                Interface.Location = new Point((Width / 2) - (Interface.Width + (gap / 2)), DistanceY);
                GInterface.Location = new Point((Width / 2) + (gap / 2), DistanceY);
            }
            else if (GISetDefualtLocation)
            {
                GInterface.Location = new Point((Width / 2) - (GInterface.Width / 2), DistanceY);
            }*/
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Right)
            {
                Reset();
            }

            Graphics.Focus();
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            /*
            Interface.Hide();
            GInterface.Hide();
            Core.Graphics.Focus();
            if (!CursorVis)
            {
                Cursor.Hide();
            }*/
        }

        private void RegeneratePlatforms()
        {
            Platforms.GameBoundaryDistance = GBD * Multiplier;

            Platforms.ClearPlatforms(this);

            Platforms.GeneratePlatforms(PNumber * Multiplier, PWidth * Multiplier, PHeight * Multiplier, this);

            Platforms.UpdateGameBoundaries();
        }
    }
}
