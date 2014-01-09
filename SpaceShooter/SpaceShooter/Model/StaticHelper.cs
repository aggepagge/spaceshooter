using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceShooter.Model.GameComponents.Weapons;

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
                default:
                    return 0.1f;
            }

        }
    }
}
