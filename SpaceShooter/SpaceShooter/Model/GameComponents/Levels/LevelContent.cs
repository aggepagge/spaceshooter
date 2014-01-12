using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SpaceShooter.Model.GameComponents.Ships;
using SpaceShooter.Model.GameComponents.Weapons;
using SpaceShooter.Model.GameComponents.Weapons.Weapon;
using SpaceShooter.Model.GameComponents.Parts;
using System.Collections;

namespace SpaceShooter.Model.GameComponents.Levels
{
    class LevelContent
    {
        private List<List<Vector2>> enemyPossitions;
        internal List<EnemySpaceShip> EnemyStorage { get; private set; }
        internal List<GameObsticle> ObsticlesStorage { get; private set; }
        internal ArrayList PowerUpStorage { get; private set; }
        internal EnemyBossSpaceShip Boss { get; private set; }
        internal bool CreateBoss { get; private set; }
        private List<KeyValuePair<EnemyTypes, int>> Enemies { get; set; }
        private Level level;
        private int numberOfObsticles;
        private int numberOfPowerUps;
        private int numberOfNewWeapons;

        internal LevelContent(Level level, int numberOfObsticles, int numberOfPowerUps, int numberOfNewWeapons,
                                        List<List<Vector2>> listOfCurves, List<KeyValuePair<EnemyTypes, int>> enemies, bool boss = false)
        {
            enemyPossitions = new List<List<Vector2>>(listOfCurves.Count);
            EnemyStorage = new List<EnemySpaceShip>();
            ObsticlesStorage = new List<GameObsticle>();
            PowerUpStorage = new ArrayList();

            this.level = level;
            this.numberOfObsticles = numberOfObsticles;
            this.numberOfPowerUps = numberOfPowerUps;
            this.numberOfNewWeapons = numberOfNewWeapons;
            this.Enemies = enemies;
            this.CreateBoss = boss;

            enemyPossitions = listOfCurves;
            setLevelContent();
        }

        internal void clearLevelContent()
        {
            enemyPossitions.Clear();
            EnemyStorage.Clear();
            ObsticlesStorage.Clear();
            PowerUpStorage.Clear();
            Boss = null;

            numberOfObsticles = 0;
            numberOfPowerUps = 0;
            numberOfNewWeapons = 0;
        }

        private void setListOfCurves(List<List<Vector2>> listOfCurves)
        {
            enemyPossitions = listOfCurves;
        }

        private void shuffleEnemies()
        {
            Random rand = new Random(EnemyStorage.Count);
            for (int i = EnemyStorage.Count; i > 1; i--)
            {
                int pos = rand.Next(i);
                var x = EnemyStorage[i - 1];
                EnemyStorage[i - 1] = EnemyStorage[pos];
                EnemyStorage[pos] = x;
            }
        }

        private void setLevelContent()
        {
            foreach (KeyValuePair<EnemyTypes, int> enemyAndType in Enemies)
            {
                int count = 0;
                for (int i = 0; i < enemyAndType.Value; i++)
                {
                    if (count > enemyPossitions.Count - 1)
                        count = 0;

                    EnemyStorage.Add(new EnemySpaceShip(
                                                            enemyAndType.Key,
                                                            StaticHelper.getEnemyShipWidth(enemyAndType.Key),
                                                            StaticHelper.getEnemyShipHeight(enemyAndType.Key),
                                                            level,
                                                            0.02f,
                                                            1.0f,
                                                            StaticHelper.getEnemyHealt(enemyAndType.Key),
                                                            enemyPossitions[count],
                                                            0.001f
                                                        ));
                    count++;
                }
            }

            shuffleEnemies();

            for (int i = 0; i < numberOfObsticles; i++)
            {
                Random rand = new Random(i);

                ObsticlesStorage.Add(new GameObsticle(
                                                    i,
                                                    0.8f,
                                                    0.1f,
                                                    new Vector2((float)(rand.NextDouble() * 1.6), -0.1f),
                                                    30
                                               ));
            }

            for (int i = 0; i < numberOfPowerUps; i++)
            {
                Random rand = new Random(i);

                PowerUpStorage.Add(new PowerUp(
                                                    0.1f,
                                                    0.1f,
                                                    new Vector2((float)(rand.NextDouble() * 1.6), -0.1f),
                                                    PowerUpType.Health
                                               ));
            }

            int insertPlace = 0;
            int countAddPowerUp = 0;

            if(PowerUpStorage.Count > 0)
                insertPlace = PowerUpStorage.Count / 2;

            Random random = new Random(PowerUpStorage.Count);

            do
            {
                PowerUp tmpPowerUp = new PowerUp(
                                            0.1f,
                                            0.1f,
                                            new Vector2((float)(random.NextDouble() * 1.6), -0.1f),
                                            PowerUpType.Wepon
                                        );
                
                PowerUpStorage.Insert(insertPlace, tmpPowerUp);
                insertPlace += insertPlace;
                countAddPowerUp++;
            }
            while (countAddPowerUp > insertPlace);

            if (CreateBoss)
            {
                List<Vector2> tmpList = new List<Vector2>();
                tmpList.Add(new Vector2(-1.0f, 0.0f));

                Boss = new EnemyBossSpaceShip(
                                                    EnemyTypes.Boss,
                                                    StaticHelper.getEnemyShipWidth(EnemyTypes.Boss),
                                                    StaticHelper.getEnemyShipHeight(EnemyTypes.Boss),
                                                    level,
                                                    -0.5f,
                                                    0.0f,
                                                    StaticHelper.getEnemyHealt(EnemyTypes.Boss),
                                                    tmpList,
                                                    0.05f
                                              );
            }
        }
    }
}
