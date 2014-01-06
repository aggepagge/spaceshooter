using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceShooter.Model.GameComponents.Ships
{
    class EnemySpaceShip : SpaceShip
    {
        private List<Vector2> possitions;

        private float updateTime = 0;
        private int numberOfPossitions = 1;
        private float updatePossitionTime;
        private float shootRate;

        internal EnemySpaceShip(float height, float width, Level level, float speedX, float speedY, int healt,
                                    List<Vector2> possitions, float updatePossitionTime, float shootRate)
            : base(height, width, level, speedX, speedY, healt)
        {
            this.possitions = possitions;
            this.shootRate = shootRate;
            spaceShipPossition.X = possitions[0].X;
            spaceShipPossition.Y = possitions[0].Y;

            this.updatePossitionTime = updatePossitionTime;
        }

        internal override void Update(float elapsedTimeSeconds)
        {
            if (numberOfPossitions < possitions.Count)
            {
                updateTime += elapsedTimeSeconds;

                if (updateTime > updatePossitionTime)
                {
                    spaceShipPossition.X = possitions[numberOfPossitions].X;
                    spaceShipPossition.Y = possitions[numberOfPossitions].Y;

                    numberOfPossitions++;

                    updateTime = 0.0f;
                }
            }
            else
                RemoveMe = true;
        }

        internal override bool HasBeenShoot(FloatRectangle shotRect)
        {
            FloatRectangle shipRect = FloatRectangle.createFromLeftTop(spaceShipPossition, SpaceShipWidth, SpaceShipHeight);

            if (shipRect.isIntersecting(shotRect))
                return true;

            return false;
        }
    }
}
