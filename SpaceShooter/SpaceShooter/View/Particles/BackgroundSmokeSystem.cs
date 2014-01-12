using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceShooter.View.Particles
{
    class BackgroundSmokeSystem
    {
        private List<BackgroundSmoke> smokes;
        private const int MAX_SMOKES = 30;

        //Initsierar arrayen med Smoke-objekt
        internal BackgroundSmokeSystem()
        {
            smokes = new List<BackgroundSmoke>(MAX_SMOKES);

            for (int i = 0; i < MAX_SMOKES; i++)
            {
                smokes.Add(new BackgroundSmoke(i, getPossitionsForSmoke(i)));
            }
        }

        private Vector2 getPossitionsForSmoke(int seed)
        {
            Random rand = new Random(seed);
            float leftX = -0.2f;
            float rightX = 1.2f;
            float topY = -0.2f;
            float bottomY = 1.2f;

            Vector2 possition = new Vector2((float)((rand.NextDouble() * rightX) + leftX),
                                            (float)((rand.NextDouble() * bottomY) + topY));
            return possition;
        }

        //Uppdaterar alla Smoke-objekt i arrayen
        internal void Update(float elapsedGameTime)
        {
            foreach (BackgroundSmoke smoke in smokes.ToList())
            {
                if (!smoke.DeleateMe)
                    smoke.Update(elapsedGameTime);
                else
                    smokes.Remove(smoke);
            }
        }

        //Anropar Draw-metoden för alla Smoke-objekt i arrayen
        internal void Draw(SpriteBatch spriteBatch, Camera camera, Texture2D texture)
        {
            foreach (BackgroundSmoke smoke in smokes)
            {
                smoke.Draw(spriteBatch, camera, texture);
            }
        }

    }
}
