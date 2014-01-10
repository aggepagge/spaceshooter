using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceShooter.Model.GameComponents.Weapons;
using SpaceShooter.Model.GameComponents.Weapons.Weapon;
using Microsoft.Xna.Framework;

namespace SpaceShooter.Model
{
    public static class StaticHelper
    {
        public static float getFireRate(WeaponTypes weapon)
        {
            switch(weapon)
            {
                case WeaponTypes.Raygun:
                    return 0.1f;
                case WeaponTypes.Missile:
                    return 0.4f;
                case WeaponTypes.EnemyRaygun:
                    return 0.6f;
                default:
                    return 0.1f;
            }
        }

        public static float getFireSpeed(WeaponTypes weapon)
        {
            switch (weapon)
            {
                case WeaponTypes.Raygun:
                    return 1.6f;
                case WeaponTypes.Missile:
                    return 0.6f;
                case WeaponTypes.EnemyRaygun:
                    return 0.4f;
                default:
                    return 0.6f;
            }
        }

        public static float getBulletWidth(WeaponTypes weapon)
        {
            switch (weapon)
            {
                case WeaponTypes.Raygun:
                    return 0.03f;
                case WeaponTypes.Missile:
                    return 0.06f;
                case WeaponTypes.EnemyRaygun:
                    return 0.02f;
                default:
                    return 0.05f;
            }
        }

        public static float getBulletHeight(WeaponTypes weapon)
        {
            switch (weapon)
            {
                case WeaponTypes.Raygun:
                    return 0.01f;
                case WeaponTypes.Missile:
                    return 0.02f;
                case WeaponTypes.EnemyRaygun:
                    return 0.02f;
                default:
                    return 0.05f;
            }
        }
    }
}
