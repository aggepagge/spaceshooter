using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceShooter.Model.GameComponents.Weapons.Weapon
{
    class Raygun : Weapon
    {
        private float updatePossitionTime;
        private float time = 0.0f;

        internal Raygun(Vector2 possition, float width, float height, int damage, 
                        float fireRate, int numberOfBullets, bool heatSeeking, bool enemyWeapon, float updatePossitionTime)
            : base(possition, width, height, damage, fireRate, numberOfBullets, heatSeeking, enemyWeapon)
        {
            this.updatePossitionTime = updatePossitionTime;
        }

        internal override void Update(float elapsedTimeSeconds)
        {
            time += elapsedTimeSeconds;

            if (time > updatePossitionTime)
            {
                float possY = (float)(elapsedTimeSeconds * FireRate);

                if (EnemyWepon)
                    setNewPossition(Possition.X, Possition.Y + possY);
                else
                    setNewPossition(Possition.X, Possition.Y - possY);

                time = 0.0f;
            }
        }
    }
}
