﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceShooter.Model.GameComponents;
using SpaceShooter.Model.GameComponents.Ships;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using SpaceShooter.Model.GameComponents.Weapons.Weapon;
using SpaceShooter.Model.GameComponents.Weapons;
using SpaceShooter.Model.GameComponents.Levels;

namespace SpaceShooter.Model
{
    class GameModel
    {
        internal PlayerSpaceShip Player { get; private set; }
        internal Level Level { get; private set; }
        private LevelContent levelContent;

        internal List<EnemySpaceShip> EnemyShips { get; private set; }
        internal List<Weapon> Shoots { get; private set; }

        private float createNewShipCount = 0.0f;
        private static float RESTART_COUNT = 1.0f;
        private int countNewEnemy = 0;
        internal float GameTime { get; private set; }
        internal bool LevelFinished { get; private set; }
        internal int LevelCount { get; private set; }

        internal GameModel()
        {
            Level = new Level();
            this.Player = new PlayerSpaceShip(
                                                XNAController.PLAYER_SPACESHIP_HEIGHT, 
                                                XNAController.PLAYER_SPACESHIP_WIDTH, 
                                                Level,
                                                XNAController.PLAYER_SPACESHIP_SPEEDX, 
                                                XNAController.PLAYER_SPACESHIP_SPEEDY, 
                                                XNAController.PLAYER_START_HEALT
                                             );


            levelContent = Level.getLevelOne();
            EnemyShips = new List<EnemySpaceShip>();
            Shoots = new List<Weapon>();
            GameTime = 0.0f;
            LevelFinished = false;
            LevelCount = 1;
        }

        internal void startNewGame(IGameModelListener listener)
        {
            this.Level = new Level();
            this.Player = new PlayerSpaceShip(
                                                XNAController.PLAYER_SPACESHIP_HEIGHT,
                                                XNAController.PLAYER_SPACESHIP_WIDTH,
                                                Level,
                                                XNAController.PLAYER_SPACESHIP_SPEEDX,
                                                XNAController.PLAYER_SPACESHIP_SPEEDY,
                                                XNAController.PLAYER_START_HEALT
                                             );

            EnemyShips.Clear();
            Shoots.Clear();
            levelContent = Level.getLevelOne();
            Player.setPossitionX(Level.StartPossition.X);
            Player.setPossitionY(Level.StartPossition.Y);

            listener.restartGame();
            GameTime = 0.0f;
            countNewEnemy = 0;
            LevelFinished = false;
            LevelCount = 1;
        }

        internal void playNextLevel(IGameModelListener listener)
        {
            LevelCount++;
            EnemyShips.Clear();
            Shoots.Clear();

            if(LevelCount == 2)
                levelContent = Level.getLevelTwo();
            else if (LevelCount == 3)
                levelContent = Level.getLevelTree();

            Player.setPossitionX(Level.StartPossition.X);
            Player.setPossitionY(Level.StartPossition.Y);

            listener.restartGame();
            GameTime = 0.0f;
            countNewEnemy = 0;
            LevelFinished = false;
        }

        internal void UpdateModel(float elapsedGameTime, IGameModelListener listener)
        {
            GameTime += elapsedGameTime;

            if (Player.RemoveMe)
                Player.setDead();

            Player.Update(elapsedGameTime);

            EnemyShips.RemoveAll(x => x.RemoveMe == true);
            Shoots.RemoveAll(x => x.RemoveMe == true);

            if (Player.Firering && Player.readyToFire())
            {
                WeaponTypes weaponType = Player.getCurrentWeapon();

                Weapon gunfire = new Weapon(
                                                Player.getCenterTopPossition(), 
                                                weaponType, 
                                                StaticHelper.getBulletWidth(weaponType),
                                                StaticHelper.getBulletHeight(weaponType),
                                                10, 
                                                StaticHelper.getFireSpeed(weaponType), 
                                                1, 
                                                false, 
                                                false
                                            );
                Shoots.Add(gunfire);
            }

            foreach (EnemySpaceShip enemy in EnemyShips)
            {
                enemy.Update(elapsedGameTime);

                if (enemy.ReadyToFire)
                {
                    WeaponTypes weaponType = enemy.WeaponType;

                    Weapon gunfire = new Weapon(
                                                    enemy.getCenterBottomPossition(), 
                                                    weaponType,
                                                    StaticHelper.getBulletWidth(weaponType),
                                                    StaticHelper.getBulletHeight(weaponType),
                                                    10, 
                                                    StaticHelper.getFireSpeed(weaponType),
                                                    1, 
                                                    false, 
                                                    true
                                                );
                    Shoots.Add(gunfire);

                    enemy.ReadyToFire = false;
                }
            }

            foreach (Weapon shot in Shoots)
            {
                shot.Update(elapsedGameTime);

                if (!shot.EnemyWepon)
                {
                    FloatRectangle shotRect = FloatRectangle.createFromLeftTop(shot.Possition, shot.Width, shot.Height);

                    foreach (EnemySpaceShip enemy in EnemyShips)
                    {
                        if (enemy.HasBeenShoot(shotRect))
                        {
                            enemy.Healt -= shot.Damage;
                            Player.PlayerScoore += shot.Damage;
                            shot.RemoveMe = true;
                            shot.Damage = 0;

                            if (enemy.Healt < 1)
                            {
                                enemy.RemoveMe = true;
                                Player.PlayerScoore += enemy.DeathPoint;
                                listener.killed(shot.Possition);
                            }
                            else
                                listener.wounded(shot.Possition);
                        }
                    }
                }
                else if (shot.EnemyWepon)
                {
                    FloatRectangle shotRect = FloatRectangle.createFromLeftTop(shot.Possition, shot.Width, shot.Height);

                    if (Player.HasBeenShoot(shotRect))
                    {
                        Player.Healt -= shot.Damage;
                        shot.RemoveMe = true;
                        shot.Damage = 0;

                        if (Player.Healt < 1)
                        {
                            Player.RemoveMe = true;
                            listener.killed(shot.Possition);
                        }
                        else
                            listener.wounded(shot.Possition);
                    }
                }
            }

            createNewShipCount += elapsedGameTime;
            
            if(createNewShipCount > RESTART_COUNT)
            {
                if (countNewEnemy < levelContent.EnemyStorage.Count)
                {
                    EnemyShips.Add(levelContent.EnemyStorage[countNewEnemy]);

                    createNewShipCount = 0.0f;
                    countNewEnemy++;
                }
            }

            if (countNewEnemy >= levelContent.EnemyStorage.Count)
                if(EnemyShips.Count == 0)
                    LevelFinished = true;
        }

        internal void playerMovesUp()
        {
            Player.setPossitionY(-1);
        }

        internal void playerMovesDown()
        {
            Player.setPossitionY();
        }

        internal void playerMovesLeft()
        {
            Player.setPossitionX(-1);
        }

        internal void playerMovesRight()
        {
            Player.setPossitionX();
        }

        internal void playerShoots()
        {
            Player.Firering = !Player.Firering;
        }
    }
}
