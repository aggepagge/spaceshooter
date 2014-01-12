using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SpaceShooter.Model.GameComponents.Weapons.Weapon;
using SpaceShooter.Model.GameComponents.Weapons;

namespace SpaceShooter.Model.GameComponents.Ships
{
    class PlayerSpaceShip : SpaceShip
    {
        internal bool Firering { get; set; }
        internal WeaponTypes CurrentWeapon { get; private set; }
        internal float FireRate { get; private set; }
        private bool CanFire { get; set; }
        private float timeUntillNextShoot = 0.0f;
        internal int PlayerScoore { get; set; }
        internal int PlayerStartHealt { get; private set; }

        private List<WeaponTypes> weapons = new List<WeaponTypes>(10);

        internal PlayerSpaceShip(float height, float width, Level level, float speedX, float speedY, int healt) 
            : base (height, width, level, speedX, speedY, healt)
        {
            Firering = false;
            CurrentWeapon = WeaponTypes.Raygun;
            this.addWeapon(WeaponTypes.Raygun);
            FireRate = StaticHelper.getFireRate(CurrentWeapon);
            CanFire = false;
            PlayerScoore = 0;
            this.PlayerStartHealt = healt;
        }

        internal Vector2 getCenterTopPossition()
        {
            return new Vector2(spaceShipPossition.X + (SpaceShipWidth / 2), spaceShipPossition.Y);
        }

        internal void setPossition(float possX, float possY)
        {
            spaceShipPossition.X = possX;
            spaceShipPossition.Y = possY;
        }

        internal void setPossitionY(float moveDirection = 1)
        {
            spaceShipPossition.Y += SpaceShipSpeed.Y * moveDirection;
        }

        internal void setPossitionX(float moveDirection = 1)
        {
            spaceShipPossition.X += SpaceShipSpeed.X * moveDirection;
        }

        internal Vector2 spaceShipCenterPossition()
        {
            return spaceShipPossition;
        }

        internal void addWeapon(WeaponTypes newWepon)
        {
            weapons.Add(newWepon);
        }

        internal void setCurrentWeapon(WeaponTypes currentWepon)
        {
            CurrentWeapon = currentWepon;
            if (!weapons.Contains(currentWepon))
                weapons.Add(currentWepon);
            FireRate = StaticHelper.getFireRate(CurrentWeapon);
        }

        internal void removeWeapon(int weaponIndex)
        {
            if (weapons.Count > weaponIndex)
                weapons.RemoveAt(weaponIndex);
        }

        internal void removeWeapon(WeaponTypes weaponEnum)
        {
            weapons.Remove(weaponEnum);
        }

        internal void removeAllWeapon()
        {
            weapons.Clear();
            weapons.Add(WeaponTypes.Raygun);
        }

        internal bool readyToFire()
        {
            bool ready = CanFire;
            CanFire = false;
            return ready;
        }

        internal WeaponTypes getCurrentWeapon()
        {
            if (weapons.Count > 0)
            {
                return CurrentWeapon;
            }
            else
            {
                weapons.Add(WeaponTypes.Raygun);
                return WeaponTypes.Raygun;
            }
        }

        internal override void Update(float elapsedTimeSeconds)
        {
            if (spaceShipPossition.X < -(SpaceShipWidth / 2))
                spaceShipPossition.X = -(SpaceShipWidth / 2);

            if (spaceShipPossition.X > level.BoardTotalWidth - (SpaceShipWidth / 2))
                spaceShipPossition.X = level.BoardTotalWidth - (SpaceShipWidth / 2);

            if (spaceShipPossition.Y < -(SpaceShipHeight / 2))
                spaceShipPossition.Y = -(SpaceShipHeight / 2);

            if (spaceShipPossition.Y > level.BoardHeight - (SpaceShipHeight / 2))
                spaceShipPossition.Y = level.BoardHeight - (SpaceShipHeight / 2);

            if (Firering)
            {
                timeUntillNextShoot += elapsedTimeSeconds;
                if (timeUntillNextShoot > FireRate)
                {
                    CanFire = true;
                    timeUntillNextShoot = 0.0f;
                }
            }
            else if (timeUntillNextShoot != 0.0f)
                timeUntillNextShoot = 0.0f;

            if (this.Healt < this.PlayerStartHealt / 2)
            {
                this.setCurrentWeapon(WeaponTypes.Raygun);
            }
        }

        internal override bool HasBeenShoot(FloatRectangle shotRect)
        {
            FloatRectangle shipRect = new FloatRectangle(
                                                            spaceShipPossition, 
                                                            new Vector2(spaceShipPossition.X + SpaceShipWidth,
                                                                        spaceShipPossition.Y + SpaceShipHeight)
                                                        );

            if (shipRect.isIntersecting(shotRect))
                return true;

            return false;
        }

        internal void setDead()
        {
            Firering = false;
            CanFire = false;
            base.SpaceShipHeight = 0.0f;
            base.SpaceShipWidth = 0.0f;
            spaceShipPossition.X = -1;
            spaceShipPossition.Y = -1;
        }

        internal void setFullHealt()
        {
            base.Healt = PlayerStartHealt;
        }
    }
}
