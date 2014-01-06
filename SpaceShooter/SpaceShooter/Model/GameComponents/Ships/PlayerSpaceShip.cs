using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceShooter.Model.GameComponents.Ships
{
    class PlayerSpaceShip : SpaceShip
    {
        internal PlayerSpaceShip(float height, float width, Level level, float speedX, float speedY, int healt) 
            : base (height, width, level, speedX, speedY, healt)
        {}

        internal Vector2 getCenterTopPossition()
        {
            return new Vector2(spaceShipPossition.X + (SpaceShipWidth / 2), spaceShipPossition.Y);
        }

        internal void setPossitionY(float moveDirection = 1)
        {
            spaceShipPossition.Y += SpaceShipSpeed.Y * moveDirection;
        }

        internal void setPossitionX(float moveDirection = 1)
        {
            spaceShipPossition.X += SpaceShipSpeed.X * moveDirection;
        }

        internal Vector2 spaceShipCenterPossition()
        {
            return spaceShipPossition;
        }

        internal override void Update(float elapsedTimeSeconds)
        {
            if (spaceShipPossition.X < -(SpaceShipWidth / 2))
                spaceShipPossition.X = -(SpaceShipWidth / 2);

            if (spaceShipPossition.X > level.BoardTotalWidth - (SpaceShipWidth / 2))
                spaceShipPossition.X = level.BoardTotalWidth - (SpaceShipWidth / 2);

            if (spaceShipPossition.Y < -(SpaceShipHeight / 2))
                spaceShipPossition.Y = -(SpaceShipHeight / 2);

            if (spaceShipPossition.Y > level.BoardHeight - (SpaceShipHeight / 2))
                spaceShipPossition.Y = level.BoardHeight - (SpaceShipHeight / 2);
        }

        internal override bool HasBeenShoot(FloatRectangle shotRect)
        {
            FloatRectangle shipRect = new FloatRectangle(
                                                            spaceShipPossition, 
                                                            new Vector2(spaceShipPossition.X + SpaceShipWidth,
                                                                        spaceShipPossition.Y + SpaceShipHeight)
                                                        );

            if (shipRect.isIntersecting(shotRect))
                return true;

            return false;
        }
    }
}
