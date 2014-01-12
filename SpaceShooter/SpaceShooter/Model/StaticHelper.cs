using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceShooter.Model.GameComponents.Weapons;
using SpaceShooter.Model.GameComponents.Weapons.Weapon;
using Microsoft.Xna.Framework;
using SpaceShooter.Model.GameComponents.Ships;

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
                case WeaponTypes.Plasma:
                    return 0.3f;
                case WeaponTypes.EnemyRaygun:
                    return 0.6f;
                case WeaponTypes.EnemyLaser:
                    return 0.6f;
                case WeaponTypes.EnemyPlasma:
                    return 0.6f;
                case WeaponTypes.EnemyBossPlasma:
                    return 0.8f;
                default:
                    return 0.1f;
            }
        }

        public static int getFireDamage(WeaponTypes weapon)
        {
            switch (weapon)
            {
                case WeaponTypes.Raygun:
                    return 20;
                case WeaponTypes.Missile:
                    return 100;
                case WeaponTypes.Plasma:
                    return 20;
                case WeaponTypes.EnemyRaygun:
                    return 10;
                case WeaponTypes.EnemyLaser:
                    return 20;
                case WeaponTypes.EnemyPlasma:
                    return 10;
                case WeaponTypes.EnemyBossPlasma:
                    return 10;
                default:
                    return 10;
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
                case WeaponTypes.Plasma:
                    return 1.0f;
                case WeaponTypes.EnemyPlasma:
                    return 0.4f;
                case WeaponTypes.EnemyRaygun:
                    return 0.4f;
                case WeaponTypes.EnemyLaser:
                    return 0.3f;
                case WeaponTypes.EnemyBossPlasma:
                    return 0.8f;
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
                case WeaponTypes.Plasma:
                    return 0.02f;
                case WeaponTypes.EnemyRaygun:
                    return 0.02f;
                case WeaponTypes.EnemyLaser:
                    return 0.04f;
                case WeaponTypes.EnemyPlasma:
                    return 0.03f;
                case WeaponTypes.EnemyBossPlasma:
                    return 0.03f;
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
                case WeaponTypes.Plasma:
                    return 0.02f;
                case WeaponTypes.EnemyRaygun:
                    return 0.02f;
                case WeaponTypes.EnemyLaser:
                    return 0.06f;
                case WeaponTypes.EnemyPlasma:
                    return 0.03f;
                case WeaponTypes.EnemyBossPlasma:
                    return 0.03f;
                default:
                    return 0.05f;
            }
        }

        public static int getBulletCount(WeaponTypes weapon)
        {
            switch (weapon)
            {
                case WeaponTypes.Raygun:
                    return 1;
                case WeaponTypes.Missile:
                    return 1;
                case WeaponTypes.Plasma:
                    return 3;
                case WeaponTypes.EnemyRaygun:
                    return 1;
                case WeaponTypes.EnemyLaser:
                    return 1;
                case WeaponTypes.EnemyPlasma:
                    return 3;
                case WeaponTypes.EnemyBossPlasma:
                    return 3;
                default:
                    return 1;
            }
        }

        public static WeaponTypes getWeaponForEnemy(EnemyTypes enemy)
        {
            switch (enemy)
            {
                case EnemyTypes.Easy:
                    return WeaponTypes.EnemyRaygun;
                case EnemyTypes.Middle:
                    return WeaponTypes.EnemyPlasma;
                case EnemyTypes.Hard:
                    return WeaponTypes.EnemyLaser;
                case EnemyTypes.Boss:
                    return WeaponTypes.EnemyBossPlasma;
                default:
                    return WeaponTypes.EnemyRaygun;
            }
        }

        public static float getEnemyShipWidth(EnemyTypes enemy)
        {
            switch (enemy)
            {
                case EnemyTypes.Easy:
                    return 0.05f;
                case EnemyTypes.Middle:
                    return 0.08f;
                case EnemyTypes.Hard:
                    return 0.08f;
                case EnemyTypes.Boss:
                    return 0.2f;
                default:
                    return 0.05f;
            }
        }

        public static float getEnemyShipHeight(EnemyTypes enemy)
        {
            switch (enemy)
            {
                case EnemyTypes.Easy:
                    return 0.05f;
                case EnemyTypes.Middle:
                    return 0.08f;
                case EnemyTypes.Hard:
                    return 0.12f;
                case EnemyTypes.Boss:
                    return 0.2f;
                default:
                    return 0.05f;
            }
        }

        public static int getEnemyHealt(EnemyTypes enemy)
        {
            switch (enemy)
            {
                case EnemyTypes.Easy:
                    return 20;
                case EnemyTypes.Middle:
                    return 30;
                case EnemyTypes.Hard:
                    return 50;
                case EnemyTypes.Boss:
                    return 500;
                default:
                    return 20;
            }
        }
    }
}
