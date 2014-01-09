using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceShooter.Model.GameComponents.Weapons;

namespace SpaceShooter.Model.GameComponents
{
    public static class StaticHelper
    {
        public static float getFireRate(WeaponTypes weapon)
        {
            WeaponTypes tmpWeapon = weapon;

            switch (tmpWeapon)
            {
                case WeaponTypes.Raygun:
                    return 0.14f;
                case WeaponTypes.Missile:
                    return 3.0f;
                default:
                    return 1.5f;
            }
        }
    }
}
