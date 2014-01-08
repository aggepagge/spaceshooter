using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceShooter.Model.GameComponents.Weapons
{
    public enum WeaponTypes
    {
        Raygun,
        Missile
    }

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
