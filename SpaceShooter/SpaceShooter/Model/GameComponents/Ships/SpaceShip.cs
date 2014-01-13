using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceShooter.Model.GameComponents.Ships
{
    /// <summary>
    /// Super-klass för rymdskepp
    /// </summary>
    abstract class SpaceShip
    {
        //Variabler för possition m.m.
        protected Vector2 spaceShipPossition;
        protected Level level;
        internal float SpaceShipHeight { get; set; }
        internal float SpaceShipWidth { get; set; }
        internal Vector2 SpaceShipSpeed { get; set; }
        internal int Healt { get; set; }
        internal bool RemoveMe { get; set; }

        //Konstruktor som tar höjd, bredd, Level-referens, fart (X och Y) samt hälsa
        internal SpaceShip(float height, float width, Level level, float speedX, float speedY, int healt)
        {
            this.spaceShipPossition.X = level.StartPossition.X - (width / 2);
            this.spaceShipPossition.Y = level.StartPossition.Y - (height / 2);
            this.SpaceShipHeight = height;
            this.SpaceShipWidth = width;
            this.level = level;
            this.Healt = healt;

            SpaceShipSpeed = new Vector2(speedX, speedY);
            RemoveMe = false;
        }

        //Returnerar possition X
        internal float getPossitionX()
        {
            return spaceShipPossition.X;
        }

        //Returnerar possition X
        internal float getPossitionY()
        {
            return spaceShipPossition.Y;
        }


        //Returnerar vector med possitionen
        internal Vector2 getShipPossition()
        {
            return spaceShipPossition;
        }

        //Abstrakta metoder som måste överskuggas
        internal abstract void Update(float elapsedTimeSeconds);
        internal abstract bool HasBeenShoot(FloatRectangle shotRect);
    }
}
