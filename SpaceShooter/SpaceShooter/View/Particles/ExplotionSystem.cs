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
        //Array för Explotion-objekt
        private List<Explotion> explotions;
        //Antal Explotion-objekt
        private const int MAX_EXPLOTIONS = 40;

        //Initsierar arrayen med Explotion-objekt
        internal ExplotionSystem(Vector2 startPossition, float scale)
        {
            explotions = new List<Explotion>(MAX_EXPLOTIONS);

            for (int i = 0; i < MAX_EXPLOTIONS; i++)
            {
                explotions.Add(new Explotion(i, startPossition, scale));
            }
        }

        //Alternativ konstruktor som tar ett par med Vector2. Detta används för att ange riktning (gravitation)
        //för explotionen (Så explotionen fortsätter åt samma håll som rymdskeppet åkte åt)
        internal ExplotionSystem(KeyValuePair<Vector2, Vector2> currentAndPreviousPossition, float scale)
        {
            explotions = new List<Explotion>(MAX_EXPLOTIONS);

            float gravityX = currentAndPreviousPossition.Key.X - currentAndPreviousPossition.Value.X;
            float gravityY = currentAndPreviousPossition.Key.Y - currentAndPreviousPossition.Value.Y;

            for (int i = 0; i < MAX_EXPLOTIONS; i++)
            {
                explotions.Add(new Explotion(i, currentAndPreviousPossition.Key, scale, gravityX, gravityY));
            }
        }

        //Uppdaterar alla Explotion-objekt i arrayen
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

        //Anropar Draw-metoden för alla Explotion-objekt i arrayen
        internal void Draw(SpriteBatch spriteBatch, Camera camera, Texture2D texture)
        {
            foreach (Explotion explotion in explotions)
            {
                explotion.Draw(spriteBatch, camera, texture);
            }
        }
    }
}
