using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SpaceShooter.Model.GameComponents.Ships;
using SpaceShooter.Model.GameComponents.Weapons;
using SpaceShooter.Model.GameComponents.Weapons.Weapon;

namespace SpaceShooter.Model.GameComponents.Levels
{
    class LevelContent
    {
        private List<List<Vector2>> enemyPossitions;
        internal List<EnemySpaceShip> EnemyStorage { get; private set; }
        private Level level;
        private int numberOfEnemies;

        internal LevelContent(Level level, int numberOfEnemies, List<List<Vector2>> listOfCurves)
        {
            enemyPossitions = new List<List<Vector2>>(10);
            EnemyStorage = new List<EnemySpaceShip>();

            this.level = level;
            this.numberOfEnemies = numberOfEnemies;

            setListOfCurves(listOfCurves);
            setLevelContent();
        }

        internal void clearLevelContent()
        {
            enemyPossitions.Clear();
            EnemyStorage.Clear();
            numberOfEnemies = 0;
        }

        private void setListOfCurves(List<List<Vector2>> listOfCurves)
        {
            enemyPossitions = listOfCurves;
        }

        private void setLevelContent()
        {
            int count = 0;
            for (int i = 0; i < numberOfEnemies; i++)
            {
                if (count > enemyPossitions.Count - 1)
                    count = 0;

                EnemyStorage.Add(new EnemySpaceShip(
                                                        0.05f, 
                                                        0.05f, 
                                                        level, 
                                                        0.02f, 
                                                        1.0f, 
                                                        20, 
                                                        enemyPossitions[count], 
                                                        0.001f,
                                                        StaticHelper.getFireRate(WeaponTypes.EnemyRaygun), 
                                                        WeaponTypes.EnemyRaygun
                                                    ));
                count++;
            }
        }

        internal void setLevelEnemies()
        {

        }
    }
}
