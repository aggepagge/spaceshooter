﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SpaceShooter.Model.GameComponents.Weapons.Weapon;

namespace SpaceShooter.Model.GameComponents.Ships
{
    class EnemyBossSpaceShip : EnemySpaceShip
    {
        internal List<Vector2> FirePossitions { get; private set; }

        internal int NumberOfFramesX { get; private set; }
        internal int Boss_CountFrame { get; set; }
        private float boss_time = 0;
        private static float BOSS_MAX_TIME = 2.0f;
        private float boss_imageTime;

        private float Boss_FireRate { get; set; }
        private float boss_fireTimer = 0.0f;
        private float boss_updateTime = 0.0f;
        private bool hasMovedLeft = false;
        private float leftBoundry;
        private float rightBoundry;
        private float boss_speedX;

        internal EnemyBossSpaceShip(EnemyTypes enemyType, float height, float width, Level level, float speedX, float speedY, int healt,
                                    List<Vector2> possitions, float updatePossitionTime)
            : base(enemyType, height, width, level, speedX, speedY, healt, possitions, updatePossitionTime)
        {
            FirePossitions = new List<Vector2>();
            FirePossitions.Add(this.getCenterBottomPossitionSprite());
            FirePossitions.Add(this.getLeftCenterPossitionSprite());
            FirePossitions.Add(this.getRightCenterPossitionSprite());

            this.NumberOfFramesX = 6;
            this.Boss_CountFrame = 1;
            base.spaceShipPossition.X = XNAController.BOARD_LOGIC_GAMEFIELDWIDTH + base.SpaceShipHeight;
            base.spaceShipPossition.Y = XNAController.BOARD_LOGIC_HEIGHT * 0.1f;
            this.leftBoundry = (float)(XNAController.BOARD_LOGIC_GAMEFIELDWIDTH * 0.2);
            this.rightBoundry = (float)(XNAController.BOARD_LOGIC_GAMEFIELDWIDTH * 0.8);
            this.boss_speedX = speedX;
            this.Boss_FireRate = StaticHelper.getFireRate(base.WeaponType);

            boss_imageTime = BOSS_MAX_TIME / NumberOfFramesX;
        }

        private Vector2 getCenterBottomPossitionSprite()
        {
            return new Vector2(SpaceShipWidth / 2, SpaceShipHeight);
        }

        private Vector2 getLeftCenterPossitionSprite()
        {
            return new Vector2(SpaceShipWidth * 0.2f, SpaceShipHeight / 2);
        }

        private Vector2 getRightCenterPossitionSprite()
        {
            return new Vector2(SpaceShipWidth * 0.8f, SpaceShipHeight / 2);
        }

        internal void imDead()
        {
            boss_speedX = 0.0f;
            hasMovedLeft = false; 
            base.spaceShipPossition.X = XNAController.BOARD_LOGIC_GAMEFIELDWIDTH + base.SpaceShipHeight;
        }

        internal override bool HasBeenShoot(FloatRectangle shotRect)
        {
            //Rektangeln har gjorts mindre än rymdskeppet så träffytan ska bli mindre
            //(Detta skulle kunnats göras i EnemyBossSpaceShips)
            FloatRectangle shipRect = FloatRectangle.createFromLeftTop(
                                                                            new Vector2((float)(spaceShipPossition.X + SpaceShipWidth * 0.3),
                                                                                        (float)(spaceShipPossition.Y - SpaceShipHeight * 0.2)),
                                                                            (float)(SpaceShipWidth - SpaceShipWidth * 0.6), 
                                                                            (float)(SpaceShipHeight * 0.4)
                                                                       );

            if (shipRect.isIntersecting(shotRect))
                return true;

            return false;
        }

        internal override void Update(float elapsedGameTime)
        {
            boss_time += elapsedGameTime;
            boss_fireTimer += elapsedGameTime;

            if (boss_fireTimer > Boss_FireRate)
            {
                boss_fireTimer = 0.0f;
                base.ReadyToFire = true;
            }
            else
                base.ReadyToFire = false;

            if (base.Healt < 0)
                base.RemoveMe = true;

            boss_updateTime += elapsedGameTime;

            if (boss_updateTime > base.updatePossitionTime)
            {
                base.spaceShipPossition.X += boss_speedX * elapsedGameTime;

                if (base.spaceShipPossition.X < leftBoundry && !hasMovedLeft)
                    hasMovedLeft = true;

                if ((base.spaceShipPossition.X > rightBoundry || base.spaceShipPossition.X < leftBoundry) && hasMovedLeft)
                    boss_speedX *= -1;

                Boss_CountFrame++;

                if (Boss_CountFrame > 6)
                    Boss_CountFrame = 1;

                boss_updateTime = 0.0f;
            }
        }
    }
}