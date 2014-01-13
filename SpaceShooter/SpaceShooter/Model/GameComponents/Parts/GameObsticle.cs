using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.View;

namespace SpaceShooter.Model.GameComponents.Parts
{
    /// <summary>
    /// Game obsticle är tänkt att var en klass för olika typer av föremål på banorna. 
    /// Men jag han tyvär bara skapa kometer, så man kna säga att det här är komet-klassen.
    /// Kometen ritas ut med ett ett spritesheet och den roterar dessutom. Så därför
    /// görs beräkningar för vilken ruta som ska visas samt rotationen
    /// </summary>
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
        private static float minSize = 0.2f;
        private static float maxSize = 0.3f;

        internal bool RemoveMe { get; set; }
        internal float Rotation { get; private set; }
        //Aktuell bildruta
        internal int ImageCount { get; set; }
        internal int Damage { get; set; }
        internal int Healt { get; set; }
        internal int DeathPoint { get; private set; }

        //Konstruktor som tar en seed, fart (i Y-led), startpossition och int för skadan den åsamkar
        internal GameObsticle(int seed, float speedY, Vector2 startPossition, int damage)
        {
            this.SpeedY = speedY;
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
            Size = minSize + ((float)(rand.NextDouble()) * (minSize - maxSize));

            //Räknar ut tiden för varje bildruta genom att dela den totala tiden med antalet bildrutor
            imageTime = MAX_TIME / NumberOfFrames;
            wait_time = imageTime;

            //Initsierar första rutan
            updateSprite();
        }

        //Returnerar possition X
        internal float getPossitionX()
        {
            return possition.X;
        }

        //Returnerar possition Y
        internal float getPossitionY()
        {
            return possition.Y;
        }

        //Kollar om detta objekt har kolliderat med ett annat objekt
        internal bool HasBeenShoot(FloatRectangle shotRect)
        {
            FloatRectangle shipRect = FloatRectangle.createFromCenter(possition, Size);

            if (shipRect.isIntersecting(shotRect))
                return true;

            return false;
        }

        //Uppdaterar kometen
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

                //Om kometen har rört sig nedanför spelplanen så ska den raderas från samlingen av kometer
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
