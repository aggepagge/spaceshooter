using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceShooter.Model.GameComponents.Parts
{
    public enum PowerUpType
    {
        Health,
        Wepon,
    }

    class PowerUp
    {
        //Farten i Y-led
        internal float SpeedY { get; private set; }
        //Vector för possition av explotionen
        private Vector2 possition;
        private float startY;
        private float time = 0;
        private const float UPDATE_TIME = 0.05f;

        //Storleken i logisk skala (1.0-skala)
        internal float Size { get; private set; }
        internal bool RemoveMe { get; set; }
        internal float Rotation { get; private set; }
        internal PowerUpType Type { get; set; }

        internal PowerUp(float speedY, float size, Vector2 startPossition, PowerUpType type)
        {
            this.SpeedY = speedY;
            this.Size = size;
            this.possition = startPossition;
            this.startY = startPossition.Y;
            this.Rotation = 1;
            this.RemoveMe = false;
            this.Type = type;
        }

        internal float getPossitionX()
        {
            return possition.X;
        }

        internal float getPossitionY()
        {
            return possition.Y;
        }

        internal bool HasBeenShoot(FloatRectangle shotRect)
        {
            FloatRectangle shipRect = FloatRectangle.createFromCenter(possition, Size);

            if (shipRect.isIntersecting(shotRect))
                return true;

            return false;
        }

        //Uppdaterar explotionen
        internal void Update(float elapsedGameTime)
        {
            time += elapsedGameTime;

            possition.Y += SpeedY * elapsedGameTime;

            if (time > UPDATE_TIME)
            {
                //Uträkning av rotation av texturen
                Rotation += time;
                float circle = MathHelper.Pi * 2;
                Rotation = Rotation % circle;

                time = 0.0f;
            }
        }
    }
}
