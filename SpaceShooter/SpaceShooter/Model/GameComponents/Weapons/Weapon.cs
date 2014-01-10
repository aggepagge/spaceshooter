using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceShooter.Model.GameComponents.Weapons.Weapon
{
    public enum WeaponTypes
    {
        Raygun,
        Missile,
        EnemyRaygun
    }

    class Weapon
    {
        internal Vector2 Possition { get; private set; }
        internal float Height { get; private set; }
        internal float Width { get; private set; }
        internal int Damage { get; set; }
        internal float FireSpeed { get; private set; }
        internal int NumberOfBullets { get; private set; }
        internal bool HeatSeeking { get; private set; }
        internal bool EnemyWepon { get; private set; }
        internal bool RemoveMe { get; set; }
        internal WeaponTypes WeaponType { get; private set; }

        private float time = 0.0f;

        internal Weapon(Vector2 possition, WeaponTypes weaponType, float width, float height, int damage, float fireSpeed,
                        int numberOfBullets, bool heatSeeking, bool enemyWepon)
        {
            //TODO: Kolla vad det är för bugg här!!!!!! (Varför width / 8)
            this.Possition = new Vector2(possition.X - (width / 8), possition.Y);
            this.Width = width;
            this.Height = height;
            this.Damage = damage;
            this.FireSpeed = fireSpeed;
            this.NumberOfBullets = numberOfBullets;
            this.HeatSeeking = heatSeeking;
            this.EnemyWepon = enemyWepon;

            WeaponType = weaponType;
            RemoveMe = false;
        }

        internal void setNewPossition(float x, float y)
        {
            Possition = new Vector2(x, y);

            if ((Possition.Y + Height < 0 && !EnemyWepon) || (Possition.Y + Height > XNAController.BOARD_LOGIC_HEIGHT && EnemyWepon))
                RemoveMe = true;
        }

        internal void setNewPossition(Vector2 newPossition)
        {
            Possition = newPossition;

            if ((Possition.Y + Height < 0 && !EnemyWepon) || (Possition.Y + Height > XNAController.BOARD_LOGIC_HEIGHT && EnemyWepon))
                RemoveMe = true;
        }

        internal void Update(float elapsedTimeSeconds)
        {
            time += elapsedTimeSeconds;

            float possY = (float)(elapsedTimeSeconds * FireSpeed);

            if (EnemyWepon)
                setNewPossition(Possition.X, Possition.Y + possY);
            else
                setNewPossition(Possition.X, Possition.Y - possY);
        }
    }
}
