using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SpaceShooter.Model.GameComponents.Weapons;
using SpaceShooter.Model.GameComponents.Weapons.Weapon;

namespace SpaceShooter.Model.GameComponents.Ships
{
    class EnemySpaceShip : SpaceShip
    {
        private List<Vector2> possitions;

        private float updateTime = 0;
        private int numberOfPossitions = 1;
        private float updatePossitionTime;
        private float shootRate;
        internal int DeathPoint { get; private set; }

        internal WeaponTypes WeaponType { get; private set; }
        internal bool ReadyToFire { get; set; }
        private float FireRate { get; set; }
        private float fireTimer = 0.0f;

        internal EnemySpaceShip(float height, float width, Level level, float speedX, float speedY, int healt,
                                    List<Vector2> possitions, float updatePossitionTime, float shootRate, WeaponTypes weaponType)
            : base(height, width, level, speedX, speedY, healt)
        {
            this.possitions = possitions;
            this.shootRate = shootRate;
            spaceShipPossition.X = possitions[0].X;
            spaceShipPossition.Y = possitions[0].Y;

            this.updatePossitionTime = updatePossitionTime;
            this.DeathPoint = healt;
            this.WeaponType = weaponType;
            this.FireRate = StaticHelper.getFireRate(WeaponType);
            this.ReadyToFire = false;
        }

        internal override void Update(float elapsedTimeSeconds)
        {
            fireTimer += elapsedTimeSeconds;

            if (fireTimer > FireRate)
            {
                fireTimer = 0.0f;
                ReadyToFire = true;
            }
            else
                ReadyToFire = false;

            if (numberOfPossitions < possitions.Count)
            {
                updateTime += elapsedTimeSeconds;

                if (updateTime > updatePossitionTime)
                {
                    spaceShipPossition.X = possitions[numberOfPossitions].X;
                    spaceShipPossition.Y = possitions[numberOfPossitions].Y;

                    numberOfPossitions++;

                    updateTime = 0.0f;
                }
            }
            else
                RemoveMe = true;
        }

        internal override bool HasBeenShoot(FloatRectangle shotRect)
        {
            FloatRectangle shipRect = FloatRectangle.createFromLeftTop(spaceShipPossition, SpaceShipWidth, SpaceShipHeight);

            if (shipRect.isIntersecting(shotRect))
                return true;

            return false;
        }

        internal Vector2 getCenterBottomPossition()
        {
            return new Vector2(spaceShipPossition.X + (SpaceShipWidth / 2), spaceShipPossition.Y + (SpaceShipHeight / 2));
        }
    }
}
