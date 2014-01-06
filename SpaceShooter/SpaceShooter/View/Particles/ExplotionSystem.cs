using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.View;

namespace SpaceShooter.View.Particles
{
    class ExplotionSystem
    {
        //Array för smoke-objekt
        private List<Explotion> explotions;
        //Antal Smoke-objekt
        private const int MAX_EXPLOTIONS = 40;

        //Initsierar arrayen med Smoke-objekt
        internal ExplotionSystem(Vector2 startPossition, float scale)
        {
            explotions = new List<Explotion>(MAX_EXPLOTIONS);

            for (int i = 0; i < MAX_EXPLOTIONS; i++)
            {
                explotions.Add(new Explotion(i, startPossition, scale));
            }
        }

        //Uppdaterar alla Smoke-objekt i arrayen
        internal void Update(float elapsedGameTime)
        {
            foreach (Explotion explotion in explotions.ToList())
            {
                if (!explotion.DeleateMe)
                    explotion.Update(elapsedGameTime);
                else
                    explotions.Remove(explotion);
            }
        }

        //Anropar Draw-metoden för alla Smoke-objekt i arrayen
        internal void Draw(SpriteBatch spriteBatch, Camera camera, Texture2D texture)
        {
            foreach (Explotion explotion in explotions)
            {
                explotion.Draw(spriteBatch, camera, texture);
            }
        }
    }
}
