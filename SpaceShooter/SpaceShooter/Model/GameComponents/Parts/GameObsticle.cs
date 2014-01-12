using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.View;

namespace SpaceShooter.Model.GameComponents.Parts
{
    class GameObsticle
    {
        //Farten i Y-led
        internal float SpeedY { get; private set; }
        //Vector för possition av explotionen
        private Vector2 possition;
        private float startY;
        //Storleken i logisk skala (1.0-skala)
        internal float Size { get; private set; }

        //Aktuell ruta i X-led
        internal int FrameX { get; set; }
        //Aktuell ruta i Y-led
        internal int FrameY { get; set; }
        //Totalt antal rutor på spriten
        internal int NumberOfFrames { get; private set; }
        //Antal rutor i X-led
        internal int NumFramesX { get; private set; }
        //Antal rutor i Y-led
        internal int NumFramesY { get; private set; }

        //Totala tiden explotionen ska visas
        private static float MAX_TIME = 1.0f;
        //Tiden varje bild ska visas
        private float imageTime;
        //Variabel för den förflutna tiden
        private float time = 0;
        
        private float wait_time;
        private static float minSize = 0.24f;
        private static float maxSize = 0.4f;

        internal bool RemoveMe { get; set; }
        internal float Rotation { get; private set; }
        //Aktuell bildruta
        internal int ImageCount { get; set; }
        internal int Damage { get; set; }
        internal int Healt { get; set; }
        internal int DeathPoint { get; private set; }

        internal GameObsticle(int seed, float speedY, float size, Vector2 startPossition, int damage)
        {
            this.SpeedY = speedY;
            this.Size = size;
            this.possition = startPossition;
            this.startY = startPossition.Y;
            this.ImageCount = 1;
            this.Rotation = 1;
            this.RemoveMe = false;
            this.Damage = damage;
            this.NumberOfFrames = 20;
            this.NumFramesX = 5;
            this.NumFramesY = 4;
            this.Healt = damage * 3;
            this.DeathPoint = Healt;

            Random rand = new Random(seed);

            //Storleken sätts mellan minsta storlek och största storlek med random
            size = minSize + ((float)(rand.NextDouble()) * (minSize - maxSize));

            //Räknar ut tiden för varje bildruta genom att dela den totala tiden med antalet bildrutor
            imageTime = MAX_TIME / NumberOfFrames;
            wait_time = imageTime;

            //Initsierar första rutan
            updateSprite();
        }

        internal float getPossitionX()
        {
            return possition.X;
        }

        internal float getPossitionY()
        {
            return possition.Y;
        }

        internal bool HasBeenShoot(FloatRectangle shotRect)
        {
            FloatRectangle shipRect = FloatRectangle.createFromCenter(possition, Size);

            if (shipRect.isIntersecting(shotRect))
                return true;

            return false;
        }

        //Uppdaterar explotionen
        internal void Update(float elapsedGameTime)
        {
            time += elapsedGameTime;

            //Om time är större än visningstid för en bildruta så visas nästa bild 
            //och time sätts till noll igen för uppräkning av nästa bildruta
            if (time > imageTime)
            {
                if (ImageCount > NumberOfFrames)
                {
                    ImageCount = 0;
                }
                else
                {
                    ImageCount++;
                    time = 0;
                }

                possition.Y += SpeedY * elapsedGameTime;

                if (possition.Y > XNAController.BOARD_LOGIC_HEIGHT + Size)
                {
                    RemoveMe = true;
                }

                //Uppdaterar bildrutan
                updateSprite();
            }
        }

        //Räkar ut vilken ruta i X-led och Y-led som ska visas
        private void updateSprite()
        {
            int countRowX = 1;

            while ((countRowX * NumFramesX) < ImageCount)
                countRowX++;

            FrameX = NumFramesX - ((countRowX * NumFramesX) - ImageCount);
            FrameY = countRowX;

            //Uträkning av rotation av texturen
            Rotation += wait_time;
            float circle = MathHelper.Pi * 2;
            Rotation = Rotation % circle;
        }
    }
}
