using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceShooter.Model.GameComponents.Weapons.Weapon
{
    abstract class Weapon
    {
        internal Vector2 Possition { get; private set; }
        internal float Height { get; private set; }
        internal float Width { get; private set; }
        internal int Damage { get; set; }
        protected float FireRate { get; private set; }
        protected int NumberOfBullets { get; private set; }
        protected bool HeatSeeking { get; private set; }
        protected bool EnemyWepon { get; private set; }

        internal bool RemoveMe { get; set; }

        internal Weapon(Vector2 possition, float width, float height, int damage, float fireRate, 
                        int numberOfBullets, bool heatSeeking, bool enemyWepon)
        {
            this.Possition = new Vector2(possition.X - (width / 2), possition.Y);
            this.Width = width;
            this.Height = height;
            this.Damage = damage;
            this.FireRate = fireRate;
            this.NumberOfBullets = numberOfBullets;
            this.HeatSeeking = heatSeeking;
            this.EnemyWepon = enemyWepon;

            RemoveMe = false;
        }

        internal void setNewPossition(float x, float y)
        {
            Possition = new Vector2(x, y);

            if ((Possition.Y + Height < 0 && !EnemyWepon) || (Possition.Y + Height > XNAController.BOARD_LOGIC_HEIGHT && EnemyWepon))
            {
                RemoveMe = true;
            }
        }

        internal abstract void Update(float elapsedTimeSeconds);
    }
}
