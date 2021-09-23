using PhysicsEngine.ObjectClasses.Core;
using PhysicsEngine.ObjectClasses.GameObjects;
using PhysicsEngine.ObjectClasses.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace PhysicsEngine
{
    public class Platforms
    {
        public Platforms(GameEngine gameEngine, Color borderColour = new Color(), Color platformColour = new Color())
        {
            if (borderColour == Color.Empty)
            {
                this.borderColour = Color.FromArgb(200, 200, 200);
            }

            else { this.borderColour = borderColour; }
            if (platformColour == Color.Empty)
            {
                this.platformColour = Color.Red;
            }
            else { this.platformColour = platformColour; }

            PlayColour = gameEngine.BackColor;
            gameEngine.Background = new ColourDisplay(BorderColour);
            borderColourDisplay = new ColourDisplay(BorderColour);

            UpdateGameBoundaries();

            gameEngine.AddObject(ObjBoundaryLeft);
            gameEngine.AddObject(ObjBoundaryBottom);
            gameEngine.AddObject(ObjBoundaryTop);
            gameEngine.AddObject(ObjBoundaryRight);
            gameEngine.AddObject(GameArea);

            colourDisplay = new ColourDisplay(PlatformColour);
            //bitmapDisplay = new BitmapDisplay(SkiaSharp.SKBitmap.Decode("Graphics.png"));

            GeneratePlatforms(60, 1000, 500, gameEngine, 0);

            //GenerateJunk(250, 50, 50, gameEngine, 0);
        }

        private Color borderColour;
        public Color BorderColour
        {
            get
            {
                return borderColour;
            }
            set
            {
                borderColour = value;

                borderColourDisplay.Colour = borderColour;
            }
        }

        public Color PlayColour { get; set; }

        private readonly int BorderSise = 3000;

        private int gbd = 8000;
        public int GameBoundaryDistance
        {
            get
            {
                return gbd;
            }
            set
            {
                gbd = value;

                UpdateGameBoundaries();
            }
        }

        public int GBL { get; private set; }

        public int GBR { get; private set; }

        public int GBT { get; private set; }

        public int GBB { get; private set; }

        private readonly PhysicsProperty PLatformProperty = new PhysicsProperty(50);

        private StaticObject ObjBoundaryLeft;
        private StaticObject ObjBoundaryRight;
        private StaticObject ObjBoundaryTop;
        private StaticObject ObjBoundaryBottom;

        private GraphicsObject GameArea;

        private readonly List<StaticObject> PlatformList = new List<StaticObject>();

        private readonly List<DynamicObject> JunkList = new List<DynamicObject>();

        private Color platformColour;
        public Color PlatformColour
        {
            get
            {
                return platformColour;
            }
            set
            {
                platformColour = value;

                colourDisplay.Colour = platformColour;
            }
        }

        private readonly ColourDisplay colourDisplay;

        private readonly ColourDisplay junkDisplay = new ColourDisplay(Color.BurlyWood);

        private readonly ColourDisplay borderColourDisplay;

        //private readonly BitmapDisplay bitmapDisplay;

        public void GeneratePlatforms(int platformNumber, int maxWidth, int maxHeight, GameEngine ge, float generationSeed = float.NaN)
        {
            Random r;

            if (generationSeed == float.NaN)
            {
                r = new Random();
            }
            else
            {
                r = new Random((int)Math.Floor(generationSeed));
            }

            for (int i = 0; i < platformNumber; i++)
            {
                StaticObject so = new StaticObject(new RectangleD(
                        r.Next(GBL, GBR),
                        r.Next(GBT, GBB),
                        r.Next(10, maxWidth),
                        r.Next(10, maxHeight)),
                    PLatformProperty, colourDisplay);

                PlatformList.Add(so);

                so.LayerHeight = -1;

                ge.AddObject(so);
            }
        }

        public void GenerateJunk(int jumkNumber, int width, int height, GameEngine ge, float generationSeed = float.NaN)
        {
            Random r;

            if (generationSeed == float.NaN)
            {
                r = new Random();
            }
            else
            {
                r = new Random((int)Math.Floor(generationSeed));
            }

            for (int i = 0; i < jumkNumber; i++)
            {
                DynamicObject dO = new DynamicObject(new RectangleD(
                        r.Next(GBL, GBR),
                        r.Next(GBT, GBB),
                        width,
                        height),
                    PLatformProperty, junkDisplay);

                JunkList.Add(dO);

                ge.AddObject(dO);
            }
        }

        public void ClearPlatforms(GameEngine ge)
        {
            for (int i = 0; i < PlatformList.Count; i++)
            {
                ge.RemoveObject(PlatformList[i]);
            }

            PlatformList.Clear();
        }

        public void ClearJunk(GameEngine ge)
        {
            for (int i = 0; i < JunkList.Count; i++)
            {
                ge.RemoveObject(JunkList[i]);
            }

            JunkList.Clear();
        }

        public void UpdateGameBoundaries()
        {
            GBL = -(GameBoundaryDistance / 2);
            GBR = (GameBoundaryDistance / 2);
            GBT = -(GameBoundaryDistance / 2);
            GBB = (GameBoundaryDistance / 2);

            ObjBoundaryLeft = new StaticObject(new RectangleD(
                GBL - BorderSise, 
                GBT - 50, 
                BorderSise, 
                (GBB - GBT) + 100), PLatformProperty, borderColourDisplay);
            ObjBoundaryRight = new StaticObject(new RectangleD(
                GBR,
                GBT - 50,
                BorderSise,
                (GBB - GBT) + 100), PLatformProperty, borderColourDisplay);
            ObjBoundaryTop = new StaticObject(new RectangleD(
                GBL - BorderSise,
                GBT - BorderSise,
                (GBR - GBL) + (BorderSise * 2),
                BorderSise), PLatformProperty, borderColourDisplay);
            ObjBoundaryBottom = new StaticObject(new RectangleD(
                GBL - BorderSise, 
                GBB, 
                (GBR - GBL) + (BorderSise * 2), 
                BorderSise), PLatformProperty, borderColourDisplay);

            GameArea = new GraphicsObject(new RectangleD(GBL, GBT, GameBoundaryDistance, GameBoundaryDistance), new ColourDisplay(PlayColour))
            {
                LayerHeight = -5,
                //CreateShadows = false
            };
        }
    }
}
