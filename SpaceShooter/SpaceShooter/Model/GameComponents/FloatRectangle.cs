using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceShooter.Model.GameComponents
{
    class FloatRectangle
    {
        Vector2 topLeft;
        Vector2 bottomRight;

        public FloatRectangle(Vector2 topLeft, Vector2 bottomRight)
        {
            this.topLeft = topLeft;
            this.bottomRight = bottomRight;
        }

        public static FloatRectangle createFromCenter(Vector2 center, float size)
        {
            Vector2 topLeft = new Vector2(center.X - size / 2.0f, center.Y - size / 2.0f);
            Vector2 bottomRight = new Vector2(center.X + size / 2.0f, center.Y + size / 2.0f);

            return new FloatRectangle(topLeft, bottomRight);
        }

        public static FloatRectangle createFromLeftTop(Vector2 topLeft, float width, float height)
        {
            Vector2 leftTop = new Vector2(topLeft.X, topLeft.Y);
            Vector2 bottomRight = new Vector2(topLeft.X + width, topLeft.Y + height);

            return new FloatRectangle(leftTop, bottomRight);
        }

        public float Right
        {
            get { return bottomRight.X; }
        }

        public float Bottom
        {
            get { return bottomRight.Y; }
        }

        public float Left
        {
            get { return topLeft.X; }
        }

        public float Top
        {
            get { return topLeft.Y; }
        }

        internal bool isIntersecting(FloatRectangle other)
        {
            if (Right < other.Left)
                return false;
            if (Bottom < other.Top)
                return false;
            if (Left > other.Right)
                return false;
            if (Top > other.Bottom)
                return false;

            return true;
        }
    }
}
