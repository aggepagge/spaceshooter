using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter.View.Particles
{
    class BackgroundSmoke
    {
        //Vector för possition av explotionen
        private Vector2 possition;
        //Storleken i logisk skala (1.0-skala)
        private float size;

        //Aktuell ruta i X-led
        private int frameX = 1;
        //Aktuell ruta i Y-led
        private int frameY = 1;
        //Totalt antal rutor på spriten
        private static int numberOfFrames = 49;
        //Antal rutor i X-led
        private static int numFramesX = 7;
        //Antal rutor i Y-led
        private static int numFramesY = 7;

        //Totala tiden explotionen ska visas
        private static float MAX_TIME = 1.6f;
        //Tiden varje bild ska visas
        private float imageTime;
        //Aktuell bildruta
        private int imageCount = 1;
        //Variabel för den förflutna tiden
        private float time = 0;

        private Vector2 speed;
        private float maxSizeXY;
        private float minXYLeft;
        private Vector2 startPossition;

        internal bool DeleateMe { get; private set; }

        internal BackgroundSmoke(int seed, Vector2 startPossition)
        {
            //Initsierar possitionen
            this.possition = new Vector2(startPossition.X, startPossition.Y);
            //Räknar ut tiden för varje bildruta genom att dela den totala tiden med antalet bildrutor
            imageTime = MAX_TIME / numberOfFrames;

            this.startPossition = startPossition;

            Random rand = new Random(seed);

            size = (float)((rand.NextDouble() * 2));
            minXYLeft = size * -1;
            maxSizeXY = size;

            float leftX = 0.1f;
            float rightX = 0.2f;

            speed = new Vector2((float)((rand.NextDouble() * rightX) - leftX), 0.4f);

            //Initsierar första rutan
            updateSprite();
            DeleateMe = false;
        }

        //Uppdaterar explotionen
        internal void Update(float elapsedGameTime)
        {
            time += elapsedGameTime;

            //Om time är större än visningstid för en bildruta så visas nästa bild 
            //och time sätts till noll igen för uppräkning av nästa bildruta
            if (time > imageTime)
            {
                if (imageCount > numberOfFrames)
                {
                    imageCount = 0;
                }
                else
                {
                    imageCount++;
                    time = 0;
                }

                //Uppdaterar bildrutan
                updateSprite();

                possition.X += speed.X * elapsedGameTime;
                possition.Y += speed.Y * elapsedGameTime;

                if (possition.X > maxSizeXY || possition.X < minXYLeft || possition.Y > maxSizeXY)
                {
                    possition.X = startPossition.X;
                    possition.Y = minXYLeft;
                }
                    
            }
        }

        //Räkar ut vilken ruta i X-led och Y-led som ska visas
        private void updateSprite()
        {
            int countRowX = 1;

            while ((countRowX * numFramesX) < imageCount)
                countRowX++;

            frameX = numFramesX - ((countRowX * numFramesX) - imageCount);
            frameY = countRowX;
        }

        //Ritar ut explotionen med Draw-funktionen som tar två rektanglar som argument.
        //Den första rektangeln skapas genom kameraklass-objektet och sätts till rätt visuell
        //storlek.
        //Den andra rektangeln sätts till del av den första rektangeln (För att visa en del av spriten)
        internal void Draw(SpriteBatch spriteBatch, Camera camera, Texture2D texture)
        {
            //Yttre rektangel med startpossition (X och Y) samt logisk storlek.
            Rectangle frameRect = camera.getVisualRectangle(possition.X, possition.Y, size, size);

            int Xrow = 0;
            int Yrow = 0;

            int spriteWidth = (int)(texture.Width / numFramesX);
            int spriteHeight = (int)(texture.Height / numFramesY);

            //Sätter rätt bredd och höjd i pixlar för aktuell ruta
            if (frameY == 1)
                Yrow = 0;
            else if (frameY > 1)
                Yrow = (frameY - 1) * spriteHeight;

            if (frameX == 1)
                Xrow = 0;
            else if (frameX > 1)
                Xrow = (frameX - 1) * spriteWidth;

            if (frameY == numFramesY && frameX > 3)
                imageCount = 1;

            Rectangle explotionRect = new Rectangle(Xrow, Yrow, spriteWidth, spriteHeight);

            spriteBatch.Draw(texture, frameRect, explotionRect, Color.White * 0.4f);
        }
    }
}
