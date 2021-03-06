﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceShooter.Model.GameComponents;
using SpaceShooter.Model.GameComponents.Ships;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using SpaceShooter.Model.GameComponents.Weapons.Weapon;

namespace SpaceShooter.Model
{
    class GameModel
    {
        internal PlayerSpaceShip Player { get; private set; }
        internal Level Level { get; private set; }

        //TODO: Ta bort det här sen...
        //private List<Vector2> enemyTest;
        //internal EnemySpaceShip Enemy { get; private set; }
        //internal List<Weapon> Shoots { get; private set; }

        private List<List<Vector2>> enemyPossitions;
        private List<EnemySpaceShip> enemyStorage;
        internal List<EnemySpaceShip> EnemyShips { get; private set; }
        internal List<Weapon> Shoots { get; private set; }

        private float createNewShipCount = 0.0f;
        private static float RESTART_COUNT = 1.0f;
        private int countNewEnemy = 0;

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

            enemyPossitions = new List<List<Vector2>>(10);
            enemyStorage = new List<EnemySpaceShip>();
            EnemyShips = new List<EnemySpaceShip>();
            Shoots = new List<Weapon>();
            setLevelOne();
        }

        private void setLevelOne()
        {
            enemyPossitions.Add(Level.drawCurveQuadratic(0.0f, 0.0f, 0.5f, 0.5f, 1.0f, 0.0f, 0.005f, 0.1f, 0.1f));
            enemyPossitions.Add(Level.drawCurveFlat(0.0f, 0.40f, 0.99f, 0.01f, 0.005f, 0.1f, 0.1f));
            enemyPossitions.Add(Level.drawCurveQuadratic(0.0f, 0.0f, 0.5f, 0.5f, 1.0f, 0.0f, 0.005f, 0.1f, 0.1f));
            enemyPossitions.Add(Level.drawCurveCubic(0.0f, 0.17f, 0.85f, 0.93f, 0.33f, 0.98f, -0.27f, 0.71f, 0.005f, 0.1f, 0.1f));
            enemyPossitions.Add(Level.drawCurveCubic(0.0f, 0.0f, 0.5f, 0.3f, 0.4f, 0.0f, 0.8f, 0.0f, 0.005f, 0.1f, 0.1f));
            enemyPossitions.Add(Level.drawCurveCubic(0.0f, 0.0f, 0.5f, 0.3f, 0.4f, 0.5f, 1.0f, 0.0f, 0.005f, 0.1f, 0.1f));

            int count = 0;
            for (int i = 0; i < 100; i++)
            {
                if (count > 5)
                    count = 0;

                enemyStorage.Add(new EnemySpaceShip(0.1f, 0.1f, Level, 0.02f, 1.0f, 20, enemyPossitions[count], 0.001f, 1.0f));
                count++;
            }
        }

        internal void UpdateModel(float elapsedGameTime, IGameModelListener listener)
        {
            Player.Update(elapsedGameTime);

            EnemyShips.RemoveAll(x => x.RemoveMe == true);
            Shoots.RemoveAll(x => x.RemoveMe == true);

            foreach (EnemySpaceShip enemy in EnemyShips)
            {
                enemy.Update(elapsedGameTime);
            }

            foreach (Weapon shot in Shoots)
            {
                shot.Update(elapsedGameTime);

                FloatRectangle shotRect = FloatRectangle.createFromLeftTop(shot.Possition, shot.Width, shot.Height);

                foreach (EnemySpaceShip enemy in EnemyShips)
                {
                    if (enemy.HasBeenShoot(shotRect))
                    {
                        enemy.Healt -= shot.Damage;
                        shot.RemoveMe = true;
                        shot.Damage = 0;

                        if (enemy.Healt < 1)
                        {
                            enemy.RemoveMe = true;
                            listener.killed(enemy.getShipPossition());
                        }
                        else
                            listener.wounded(enemy.getShipPossition());
                    }
                }
            }

            createNewShipCount += elapsedGameTime;
            
            if(createNewShipCount > RESTART_COUNT)
            {
                if (countNewEnemy < enemyStorage.Count)
                {
                    EnemyShips.Add(enemyStorage[countNewEnemy]);

                    createNewShipCount = 0.0f;
                    countNewEnemy++;
                }
            }
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
            Vector2 playerPossition = Player.getCenterTopPossition();
            Raygun gunfire = new Raygun(playerPossition, 0.01f, 0.02f, 10, 1.6f, 1, false, false, 0.0005f);
            Shoots.Add(gunfire);
        }
    }
}
