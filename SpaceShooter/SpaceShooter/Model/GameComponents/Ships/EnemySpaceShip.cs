using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SpaceShooter.Model.GameComponents.Weapons;
using SpaceShooter.Model.GameComponents.Weapons.Weapon;

namespace SpaceShooter.Model.GameComponents.Ships
{
    public enum EnemyTypes
    {
        Easy,
        Middle,
        Hard,
        Boss
    }

    class EnemySpaceShip : SpaceShip
    {
        private List<Vector2> possitions;
        internal EnemyTypes EnemyType { get; private set; }

        private float updateTime = 0;
        private int numberOfPossitions = 1;
        protected float updatePossitionTime;
        private float shootRate;
        internal int DeathPoint { get; private set; }

        internal WeaponTypes WeaponType { get; set; }
        internal bool ReadyToFire { get; set; }
        private float FireRate { get; set; }
        private float fireTimer = 0.0f;

        private Vector2 PreviousPossition { get; set; }
        private Vector2 CurrentPossition { get; set; }

        internal EnemySpaceShip(EnemyTypes enemyType, float height, float width, Level level, float speedX, float speedY, int healt,
                                    List<Vector2> possitions, float updatePossitionTime)
            : base(height, width, level, speedX, speedY, healt)
        {
            this.possitions = possitions;
            spaceShipPossition.X = possitions[0].X;
            spaceShipPossition.Y = possitions[0].Y;

            this.updatePossitionTime = updatePossitionTime;
            this.DeathPoint = healt;
            this.EnemyType = enemyType;
            this.WeaponType = StaticHelper.getWeaponForEnemy(enemyType);
            this.FireRate = StaticHelper.getFireRate(WeaponType);
            this.shootRate = StaticHelper.getFireRate(WeaponType);
            this.ReadyToFire = false;
            this.PreviousPossition = spaceShipPossition;
            this.CurrentPossition = spaceShipPossition;
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
                    PreviousPossition = spaceShipPossition;

                    spaceShipPossition.X = possitions[numberOfPossitions].X;
                    spaceShipPossition.Y = possitions[numberOfPossitions].Y;

                    CurrentPossition = spaceShipPossition;

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

        internal KeyValuePair<Vector2, Vector2> getCurrentAndPreviousPossition()
        {
            return new KeyValuePair<Vector2, Vector2>(CurrentPossition, PreviousPossition);
        }
    }
}
