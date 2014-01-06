using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceShooter.Model.GameComponents.Ships
{
    abstract class SpaceShip
    {
        protected Vector2 spaceShipPossition;
        protected Level level;
        internal float SpaceShipHeight { get; private set; }
        internal float SpaceShipWidth { get; set; }
        internal Vector2 SpaceShipSpeed { get; set; }
        internal int Healt { get; set; }
        internal bool RemoveMe { get; set; }

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

        internal float getPossitionX()
        {
            return spaceShipPossition.X;
        }

        internal float getPossitionY()
        {
            return spaceShipPossition.Y;
        }

        internal Vector2 getShipPossition()
        {
            return spaceShipPossition;
        }

        internal abstract void Update(float elapsedTimeSeconds);

        internal abstract bool HasBeenShoot(FloatRectangle shotRect);
    }
}
