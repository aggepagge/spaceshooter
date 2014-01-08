using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SpaceShooter.View.Particles
{
    class Explotion
    {
        //Vector för possition av explotionen
        private Vector2 possition;
        private float startY;
        private Vector2 speed;
        //Storleken i logisk skala (1.0-skala)
        private float size;

        //Aktuell ruta i X-led
        private int frameX = 1;
        //Aktuell ruta i Y-led
        private int frameY = 1;
        //Totalt antal rutor på spriten
        private static int numberOfFrames = 48;
        //Antal rutor i X-led
        private static int numFramesX = 8;
        //Antal rutor i Y-led
        private static int numFramesY = 6;

        //Totala tiden explotionen ska visas
        private static float MAX_TIME = 1.5f;
        //Tiden varje bild ska visas
        private float imageTime;
        //Aktuell bildruta
        private int imageCount = 1;
        //Variabel för den förflutna tiden
        private float time = 0;

        private float rotation = 0;
        private float rotationValue = 1.0f; 
        private float wait_time;
        private bool stopExplotion = false;
        private Vector2 gravity;
        private float sizeIncrease;
        private static float minSize = 0.24f;
        private static float maxSize = 0.4f;

        internal bool DeleateMe { get; private set; }

        internal Explotion(int seed, Vector2 startPossition, float scale)
        {
            Random rand = new Random(seed);

            //Initsiering av fart-vektorn med random-fart. Detta för att ge en mer ojämn fördelning
            speed = new Vector2((float)(rand.NextDouble() - 0.50), (float)(rand.NextDouble() * 2 - 1));
            speed.Normalize();
            speed.Y = -0.6f;

            //Initsierar possitionen
            possition = new Vector2(startPossition.X + ((float)((rand.NextDouble() * 2 - 1) * (scale * 0.0001))), startPossition.Y);

            //Storleken sätts mellan minsta storlek och största storlek med random
            size = minSize + ((float)(rand.NextDouble()) * (minSize - maxSize));

            if (seed % 2 == 0)
                rotationValue *= -1;

            startY = possition.Y - (float)(size * 2.0);

            //Initsiering med hjälp av uträkning som ökar storleken på rök-partickeln
            sizeIncrease = (size * 30 * ((float)(rand.NextDouble()) * (float)(scale * 0.002))) / ((float)scale);

            //Räknar ut tiden för varje bildruta genom att dela den totala tiden med antalet bildrutor
            imageTime = MAX_TIME / numberOfFrames;
            wait_time = imageTime;

            //gravitationen i X och Y-led
            gravity = new Vector2(0.0f, 2.8f);

            //Initsierar första rutan
            updateSprite();

            DeleateMe = false;
        }

        //Uppdaterar explotionen
        internal void Update(float elapsedGameTime)
        {
            if (!stopExplotion)
            {
                time += elapsedGameTime;

                //Om time är större än visningstid för en bildruta så visas nästa bild 
                //och time sätts till noll igen för uppräkning av nästa bildruta
                if (time > imageTime)
                {
                    if (imageCount > numberOfFrames)
                    {
                        imageCount = 0;
                        stopExplotion = true;
                        DeleateMe = true;
                    }
                    else
                    {
                        imageCount++;
                        time = 0;

                        //Ökar storleken
                        size += sizeIncrease;
                    }

                    possition.Y += speed.Y * elapsedGameTime;
                    possition.X += (speed.X / 3) * elapsedGameTime;

                    if (possition.Y < startY)
                    {
                        possition.X -= (speed.X / 2) * elapsedGameTime;
                        possition.Y -= (float)((speed.Y / 1.04f) * elapsedGameTime);
                    }

                    //Uppdaterar bildrutan
                    updateSprite();
                }
            }
        }

        //Räkar ut vilken ruta i X-led och Y-led som ska visas
        private void updateSprite()
        {
            if (!stopExplotion)
            {
                int countRowX = 1;

                while ((countRowX * numFramesX) < imageCount)
                    countRowX++;

                frameX = numFramesX - ((countRowX * numFramesX) - imageCount);
                frameY = countRowX;

                //Uträkning av rotation av texturen
                rotation += wait_time;
                float circle = MathHelper.Pi * 2;
                rotation = rotation % circle;
            }
        }

        //Ritar ut explotionen med Draw-funktionen som tar två rektanglar som argument.
        //Den första rektangeln skapas genom kameraklass-objektet och sätts till rätt visuell
        //storlek.
        //Den andra rektangeln sätts till del av den första rektangeln (För att visa en del av spriten)
        internal void Draw(SpriteBatch spriteBatch, Camera camera, Texture2D texture)
        {
            if (!stopExplotion)
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

                //Skapar den inre rektangeln där spriten ritas ut
                Rectangle explotionRect = new Rectangle(Xrow, Yrow, spriteWidth, spriteHeight);

                spriteBatch.Draw(texture, frameRect, explotionRect, new Color(255, 255, 255, 105), rotation * rotationValue, new Vector2((texture.Width / numFramesX) / 2, (texture.Height / numFramesY) / 2), SpriteEffects.None, 0);
            }
        }
    }
}
