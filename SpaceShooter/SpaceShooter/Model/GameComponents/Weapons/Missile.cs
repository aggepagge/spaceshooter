using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceShooter.Model.GameComponents.Weapons.Weapon
{
    class Missile : Weapon
    {
        internal Missile(Vector2 possition, float width, float height, int damage, float fireSpeed, int numberOfBullets, bool heatSeeking, bool enemyWeapon)
            : base(possition, width, height, damage, fireSpeed, numberOfBullets, heatSeeking, enemyWeapon)
        { }
    }
}
