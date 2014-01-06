using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Model;

namespace SpaceShooter.View.Particles
{
    class SplitterSystem
    {
        //Skapar en array av Splitter
        private List<Splitter> particles;
        //statisk variabler för antalet splitter
        private static int MAX_PARTICLES = 40;

        //Statiska variabler för starttid och sluttid
        //(Anger hur länge splittret + splitterGrov ska köras)
        private static float startRunTime = 0.0f;
        private static float endRunTime = 0.001f;

        //Konstruktor som initsierar arrayerna och particklarna i dessa
        internal SplitterSystem(Vector2 startPossition, float scale)
        {
            particles = new List<Splitter>(MAX_PARTICLES);

            for (int i = 0; i < MAX_PARTICLES; i++)
            {
                particles.Add(new Splitter(i, startPossition, startRunTime, endRunTime));
            }
        }

        //Uppdaterar particklarna i arrayerna
        internal void Update(float elapsedGameTime)
        {
            foreach (Splitter splitter in particles.ToList())
            {
                if (!splitter.DeleateMe)
                    splitter.Update(elapsedGameTime);
                else
                    particles.Remove(splitter);
            }
        }

        //Anropar Draw-funktionen på alla objekt i arrayerna
        internal void Draw(SpriteBatch spriteBatch, Camera camera, Texture2D texture)
        {
            foreach (Splitter splitter in particles)
            {
                splitter.Draw(spriteBatch, camera, texture);
            }
        }
    }
}
