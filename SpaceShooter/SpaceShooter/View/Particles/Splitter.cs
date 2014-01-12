using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter.View.Particles
{
    class Splitter
    {
        private Vector2 systemStartPossition;
        private Vector2 possition;
        private Vector2 speed;
        private Vector2 gravity;
        private float lifetime = 0;
        private static float MAX_LIFETIME = 1.6f;
        private static float minSpeed = 0.6f;
        private static float maxSpeed = 3.0f;
        private static float minSize = 0.02f;
        private static float maxSize = 0.03f;

        private float size;
        private float delayTimeSeconds;

        internal bool DeleateMe { get; private set; }

        //Konstruktor som tar seed, startpossition och start samt 
        //slut-tid för hur länge annimeringen ska köras
        public Splitter(int seed, Vector2 systemStartPossition, float startRunTime, float endRunTime)
        {
            this.systemStartPossition = systemStartPossition;
            possition = new Vector2(systemStartPossition.X, systemStartPossition.Y);

            Random rand = new Random(seed);
            //Initsiering av fart-vektorn med random-fart. Detta för att ge en mer ojämn fördelning
            speed = new Vector2((float)(rand.NextDouble() * 2 - 1), (float)(rand.NextDouble() * 2 - 1));
            speed.Normalize();

            //Farten sätts till en random-fart mellan lägsta och högsta farten
            speed *= minSpeed + ((float)(rand.NextDouble()) * (minSpeed - maxSpeed));

            //Random-initsiering av storlek mellan minsta och största storlek
            size = minSize + ((float)(rand.NextDouble()) * (minSize - maxSize));

            //För hur länge animeringen ska vänta innan den startar
            delayTimeSeconds = startRunTime + (float)(rand.NextDouble()) * endRunTime;

            //gravitationen i X och Y-led
            gravity = new Vector2(0.0f, 0.8f);

            DeleateMe = false;
        }

        internal void Update(float elapseTimeSeconds)
        {
            delayTimeSeconds -= elapseTimeSeconds;

            //Kollar om uppdateringen ska starta
            if (delayTimeSeconds <= 0.0f)
            {
                possition.X += speed.X * elapseTimeSeconds;
                possition.Y += speed.Y * elapseTimeSeconds;

                speed.X += gravity.X * elapseTimeSeconds;
                speed.Y += gravity.Y * elapseTimeSeconds;

                lifetime += elapseTimeSeconds;
            }
        }

        internal void Draw(SpriteBatch spriteBatch, Camera camera, Texture2D texture)
        {
            //Kollar om animeringen ska starta
            if (delayTimeSeconds <= 0.0f)
            {
                //Hämtar visuella kordinater från camera-klassen
                Rectangle splitterRect = camera.getVisualRectangle(possition.X, possition.Y, size, size);

                //Variabler för uträkning av opacitet
                float t = lifetime / MAX_LIFETIME;
                float endValue = 0.0f;
                float startValue = 1.0f;

                if (t > 1.0f)
                    t = 1.0f;

                //Opaciteten ökas med t
                float opacity = endValue * t + (1.0f - t) * startValue;
                Color myColor = new Color(opacity, opacity, opacity, opacity);

                if (opacity == 0)
                    DeleateMe = true;

                spriteBatch.Draw(texture, splitterRect, myColor);
            }
        }
    }
}
