using PhysicsEngine.ObjectClasses;
using PhysicsEngine.ObjectClasses.Core;
using PhysicsEngine.ObjectClasses.GameObjects;
using PhysicsEngine.ObjectClasses.Graphics;
using PhysicsEngine.ObjectClasses.Particles;
using SkiaSharp;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PhysicsEngine
{
    public class PlayerMovement
    {
        public PlayerMovement(GameEngine gameEngine, Color p1Colour, Color p2Colour)
        {
            /*
            SKBitmap GraphicsReferance = SKBitmap.Decode("Graphics.png");

            SKBitmap playerGraphics = new SKBitmap();
            SKBitmap playerLeft;
            SKBitmap playerRight;

            GraphicsReferance.ExtractSubset(playerGraphics, new SKRectI(51, 192, 61, 208));

            playerLeft = playerGraphics;
            playerRight = playerGraphics.FlipX();

            Bitmaps.Add(GraphicsReferance, "GrahicsReferance");
            Bitmaps.Add(playerLeft, "PlayerLeft");
            Bitmaps.Add(playerRight, "PlayerRight");

            BitmapDisplay bd = new BitmapDisplay(GraphicsReferance);
            */
            ColourDisplay cd = new ColourDisplay(p1Colour);

            //Player = new DynamicObject(new RectangleD(0, 0, 31.25, 50), new PhysicsProperty(5), cd)
            Player = new DynamicObject(new RectangleD(0, 0, 50, 50), new PhysicsProperty(5), cd)
            {
                LayerHeight = 1,
                CenterFromDisplaySize = false
            };
            Player.Display.Quality = SKFilterQuality.None;
            Player.OnCollision += PlayerCollision;
            //Player.Glow = true;
            //Player.DrawBorder = true;
            //Player.Border = new Pen(Color.BlueViolet, 5);

            //LightObject PlayerLight = new LightObject(Color.FromArgb(70, 230, 240, 255), 400, Player);

            //gameEngine.AddLight(PlayerLight);

            Player2 = new DynamicObject(new RectangleD(100, 0, 50, 50), new PhysicsProperty(5), new ColourDisplay(p2Colour));

            Particle1 = new SquareParticle(new PointD(0, 0), new PointD(0, 0), 30, Color.FromArgb(200, 200, 200));
            Particle2 = new SquareParticle(new PointD(0, 0), new PointD(0, 0), 30, Color.FromArgb(200, 200, 200));

            gameEngine.AddParticle(Particle1);
            gameEngine.AddParticle(Particle2);

            gameEngine.AddObject(Player);
            gameEngine.AddObject(Player2);

            gameEngine.Camera.Track = Player;
        }

        public readonly DynamicObject Player;

        public readonly DynamicObject Player2;

        //private readonly Collection<SKBitmap> Bitmaps = new Collection<SKBitmap>();

        public double PlayerMaxSpeed { get; set; } = 50;
        public double PlayerSpeedChange { get; set; } = 2;
        public double PlayerVelocityIncrease { get; set; } = 2;

        private bool Up = false;
        private bool Down = false;
        private bool Left = false;
        private bool Right = false;
        private bool W = false;
        private bool S = false;
        private bool A = false;
        private bool D = false;

        public void KeyDown(GameEngine ge, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Up)
            {
                Up = true;
            }
            else if (e.KeyCode == Keys.Down)
            {
                Down = true;
            }
            else if (e.KeyCode == Keys.Left)
            {
                Left = true;
            }
            else if (e.KeyCode == Keys.Right)
            {
                Right = true;
            }
            else if (e.KeyCode == Keys.W)
            {
                W = true;
            }
            else if (e.KeyCode == Keys.S)
            {
                S = true;
            }
            else if (e.KeyCode == Keys.A)
            {
                A = true;
            }
            else if (e.KeyCode == Keys.D)
            {
                D = true;
            }
            else if (e.KeyCode == Keys.E)
            {
                if (ge.Camera.Track == Player)
                {
                    ge.Camera.Track = Player2;
                }
                else
                {
                    ge.Camera.Track = Player;
                }
            }
        }

        public void KeyUp(KeyEventArgs e)
        {
            if (e.KeyData == Keys.Up)
            {
                Up = false;
            }
            else if (e.KeyCode == Keys.Down)
            {
                Down = false;
            }
            else if (e.KeyCode == Keys.Left)
            {
                Left = false;
            }
            else if (e.KeyCode == Keys.Right)
            {
                Right = false;
            }
            else if (e.KeyCode == Keys.W)
            {
                W = false;
            }
            else if (e.KeyCode == Keys.S)
            {
                S = false;
            }
            else if (e.KeyCode == Keys.A)
            {
                A = false;
            }
            else if (e.KeyCode == Keys.D)
            {
                D = false;
            }
        }

        public void MovePlayer(DynamicObject trackingPlayer)
        {
            DynamicObject Track = trackingPlayer;

            DynamicObject NonTrack = trackingPlayer == Player2 ? Player : Player2;

            if (Track != null)
            {
                #region Tracked movment

                if (Up && Track.VelocityY > -PlayerMaxSpeed)
                {
                    Track.SetVelocityY += -PlayerVelocityIncrease;
                }
                else if (Up)
                {
                    Track.SetVelocityY = -PlayerMaxSpeed;
                }
                if (Down && Track.VelocityY < PlayerMaxSpeed)
                {
                    if (Track.SetVelocityY < -PlayerSpeedChange)
                    {
                        Track.SetForceY += PlayerVelocityIncrease;
                    }
                    else
                    {
                        Track.SetVelocityY += PlayerVelocityIncrease;
                    }
                }
                if (Left && Track.VelocityX > -PlayerMaxSpeed)
                {
                    if (Track.VelocityX > PlayerSpeedChange)
                    {
                        Track.SetForceX -= PlayerVelocityIncrease;
                    }
                    else
                    {
                        Track.SetVelocityX += -PlayerVelocityIncrease;
                    }
                }
                else if (Left)
                {
                    Track.SetVelocityX = -PlayerMaxSpeed;
                }
                if (Right && Track.VelocityX < PlayerMaxSpeed)
                {
                    if (Track.VelocityX < -PlayerSpeedChange)
                    {
                        Track.SetForceX += PlayerVelocityIncrease;
                    }
                    else
                    {
                        Track.SetVelocityX += PlayerVelocityIncrease;
                    }
                }
                else if (Right)
                {
                    Track.SetVelocityX = PlayerMaxSpeed;
                }

                #endregion

                #region Non tracked movment

                if (W && NonTrack.VelocityY > -PlayerMaxSpeed)
                {
                    NonTrack.SetVelocityY += -PlayerVelocityIncrease;
                }
                else if (W)
                {
                    NonTrack.SetVelocityY = -PlayerMaxSpeed;
                }
                if (S && NonTrack.VelocityY < PlayerMaxSpeed)
                {
                    if (NonTrack.SetVelocityY < -PlayerSpeedChange)
                    {
                        NonTrack.SetForceY += PlayerVelocityIncrease;
                    }
                    else
                    {
                        NonTrack.SetVelocityY += PlayerVelocityIncrease;
                    }
                }
                if (A && NonTrack.VelocityX > -PlayerMaxSpeed)
                {
                    if (NonTrack.VelocityX > PlayerSpeedChange)
                    {
                        NonTrack.SetForceX -= PlayerVelocityIncrease;
                    }
                    else
                    {
                        NonTrack.SetVelocityX += -PlayerVelocityIncrease;
                    }
                }
                else if (A)
                {
                    NonTrack.SetVelocityX = -PlayerMaxSpeed;
                }
                if (D && NonTrack.VelocityX < PlayerMaxSpeed)
                {
                    if (NonTrack.VelocityX < -PlayerSpeedChange)
                    {
                        NonTrack.SetForceX += PlayerVelocityIncrease;
                    }
                    else
                    {
                        NonTrack.SetVelocityX += PlayerVelocityIncrease;
                    }
                }
                else if (D)
                {
                    NonTrack.SetVelocityX = PlayerMaxSpeed;
                }

                #endregion
            }
        }

        private readonly SquareParticle Particle1;
        private readonly SquareParticle Particle2;

        private void PlayerCollision(object sender, CollisionEventArgs e)
        {
            int m1 = 75;
            int m2 = 50;

            int velocityRequire = 45;

            RectangleD playerLocation = e.Location;

            Collisions.CollisionArea cArea = e.CollisionArea;

            bool fastX = false;
            bool fastY = false;

            if (Player.VelocityX > velocityRequire || Player.VelocityX < -velocityRequire)
            {
                fastX = true;
            }
            if (Player.VelocityY > velocityRequire || Player.VelocityY < -velocityRequire)
            {
                fastY = true;
            }

            if (cArea == Collisions.CollisionArea.Left && fastX)
            {
                Particle1.SetPoints(new PointD(playerLocation.X, playerLocation.Y), new PointD(playerLocation.X + m2, playerLocation.Y - m1));
                Particle2.SetPoints(new PointD(playerLocation.X, playerLocation.Y + playerLocation.Height), new PointD(playerLocation.X + m2, playerLocation.Y + playerLocation.Height + m1));
            }
            else if (cArea == Collisions.CollisionArea.Right && fastX)
            {
                Particle1.SetPoints(new PointD(playerLocation.X + playerLocation.Width, playerLocation.Y), new PointD(playerLocation.X + playerLocation.Width - m2, playerLocation.Y - m1));
                Particle2.SetPoints(new PointD(playerLocation.X + playerLocation.Width, playerLocation.Y + playerLocation.Height), new PointD(playerLocation.X + playerLocation.Width - m2, playerLocation.Y + playerLocation.Height + m1));
            }
            else if (cArea == Collisions.CollisionArea.Top && fastY)
            {
                Particle1.SetPoints(new PointD(playerLocation.X, playerLocation.Y), new PointD(playerLocation.X - m1, playerLocation.Y + m2));
                Particle2.SetPoints(new PointD(playerLocation.X + playerLocation.Width, playerLocation.Y), new PointD(playerLocation.X + playerLocation.Width + m1, playerLocation.Y + m2));
            }
            else if (cArea == Collisions.CollisionArea.Bottom && fastY) // Bottom
            {
                Particle1.SetPoints(new PointD(playerLocation.X, playerLocation.Y + playerLocation.Height), new PointD(playerLocation.X - m1, playerLocation.Y + playerLocation.Height - m2));
                Particle2.SetPoints(new PointD(playerLocation.X + playerLocation.Width, playerLocation.Y + playerLocation.Height), new PointD(playerLocation.X + playerLocation.Width + m1, playerLocation.Y + playerLocation.Height - m2));
            }

            if (fastY || fastX)
            {
                Particle1.Trigger();
                Particle2.Trigger();
            }
        }
    }
}
