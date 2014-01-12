using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SpaceShooter.Model.GameComponents.Levels;
using SpaceShooter.Model.GameComponents;
using SpaceShooter.Model.GameComponents.Ships;

namespace SpaceShooter.Model
{
    class Level
    {
        //Prop för startpossition
        public Vector2 StartPossition { get; private set; }
        public float BoardWidth { get; private set; }
        public float BoardHeight { get; private set; }
        public float BoardTotalWidth { get; private set; }

        //Initsierar startpossitionerna
        internal Level()
        {
            BoardWidth = XNAController.BOARD_LOGIC_WIDTH;
            BoardHeight = XNAController.BOARD_LOGIC_HEIGHT;
            BoardTotalWidth = XNAController.BOARD_LOGIC_GAMEFIELDWIDTH;
            StartPossition = new Vector2(BoardTotalWidth / 2, BoardHeight * 0.9f);
        }

        internal static List<Vector2> drawCurveFlat(float aX, float aY, float bX, float bY, float degrade, int reverse)
        {
            List<Vector2> possitions = new List<Vector2>();

            float countA = 1.0f;
            float countB = 0.0f; ;

            while (countB < 1.0f)
            {
                float calculateX = aX * countA * countA + bX * 2 * countA * countB;
                float calculateY = aY * countA * countA + bY * 2 * countA * countB;

                possitions.Add(new Vector2(calculateX, calculateY));

                countA -= degrade;
                countB = 1.0f - countA;
            }

            if (reverse == 1)
                possitions.Reverse();

            return possitions;
        }

        //Uträkningen av Bezier-kurvan är inspirerad av: http://www.gamedev.net/reference/articles/article1808.asp
        //
        internal static List<Vector2> drawCurveQuadratic(float aX, float aY, float bX, float bY, float cX, float cY, float degrade, int reverse)
        {
            List<Vector2> possitions = new List<Vector2>();

            float countA = 1.0f;
            float countB = 0.0f;;

            while (countB < 1.0f)
            {
                float calculateX = aX * countA * countA + bX * 2 * countA * countB + cX * countB * countB;
                float calculateY = aY * countA * countA + bY * 2 * countA * countB + cY * countB * countB;

                possitions.Add(new Vector2(calculateX, calculateY));

                countA -= degrade;
                countB = 1.0f - countA;
            }

            if (reverse == 1)
                possitions.Reverse();

            return possitions;
        }

        //Uträkningen av Bezier-kurvan är inspirerad av: http://www.gamedev.net/reference/articles/article1808.asp
        //
        internal static List<Vector2> drawCurveCubic(float aX, float aY, float bX, float bY, float cX, float cY, float dX, float dY, float degrade, int reverse)
        {
            List<Vector2> possitions = new List<Vector2>();

            float countA = 1.0f;
            float countB = 0.0f; ;

            while (countB < 1.0f)
            {
                float calculateX = aX * countA * countA * countA + bX * 3 * countA * countA * countB + 
                                    cX * 3 * countA * countB * countB + dX * countB * countB * countB;
                float calculateY = aY * countA * countA * countA + bY * 3 * countA * countA * countB + 
                                    cY * 3 * countA * countB * countB + dY * countB * countB * countB;

                possitions.Add(new Vector2(calculateX, calculateY));

                countA -= degrade;
                countB = 1.0f - countA;
            }

            if (reverse == 1)
                possitions.Reverse();
            
            return possitions;
        }

        internal LevelContent getLevelOne()
        {
            List<List<Vector2>> enemyPossitions = new List<List<Vector2>>(10);
            List<KeyValuePair<EnemyTypes, int>> enemyType = new List<KeyValuePair<EnemyTypes, int>>();

            enemyType.Add(new KeyValuePair<EnemyTypes, int>(EnemyTypes.Easy, 10));
            enemyType.Add(new KeyValuePair<EnemyTypes, int>(EnemyTypes.Middle, 4));
            enemyType.Add(new KeyValuePair<EnemyTypes, int>(EnemyTypes.Hard, 2));

            Random rand = new Random(enemyType.Count);
            enemyPossitions.Add(Level.drawCurveQuadratic(0.0f, 0.0f, 0.5f, 0.5f, 1.6f, 0.0f, 0.005f, rand.Next(0, 2)));
            enemyPossitions.Add(Level.drawCurveFlat(0.0f, 0.40f, 0.99f, 0.01f, 0.005f, rand.Next(0, 2)));
            enemyPossitions.Add(Level.drawCurveQuadratic(0.0f, 0.0f, 0.5f, 0.5f, 1.6f, 0.0f, 0.005f, rand.Next(0, 2)));
            enemyPossitions.Add(Level.drawCurveCubic(0.0f, 0.17f, 0.85f, 0.93f, 0.33f, 1.98f, -0.27f, 0.71f, 0.005f, rand.Next(0, 2)));
            enemyPossitions.Add(Level.drawCurveCubic(0.0f, 0.0f, 0.5f, 0.3f, 0.4f, 0.0f, 0.8f, 0.0f, 0.005f, rand.Next(0, 2)));
            enemyPossitions.Add(Level.drawCurveCubic(0.0f, 0.0f, 0.5f, 0.3f, 0.4f, 0.5f, 1.6f, 0.0f, 0.005f, rand.Next(0, 2)));

            return new LevelContent(this, 0, 2, 1, enemyPossitions, enemyType);
        }

        internal LevelContent getLevelTwo()
        {
            List<List<Vector2>> enemyPossitions = new List<List<Vector2>>(10);
            List<KeyValuePair<EnemyTypes, int>> enemyType = new List<KeyValuePair<EnemyTypes, int>>();

            enemyType.Add(new KeyValuePair<EnemyTypes, int>(EnemyTypes.Easy, 10));
            enemyType.Add(new KeyValuePair<EnemyTypes, int>(EnemyTypes.Middle, 6));
            enemyType.Add(new KeyValuePair<EnemyTypes, int>(EnemyTypes.Hard, 4));

            Random rand = new Random(enemyType.Count);
            enemyPossitions.Add(Level.drawCurveQuadratic(0.0f, 0.0f, 0.5f, 0.5f, 1.6f, 0.0f, 0.005f, rand.Next(0, 2)));
            enemyPossitions.Add(Level.drawCurveFlat(0.0f, 0.40f, 0.99f, 0.01f, 0.005f, rand.Next(0, 2)));
            enemyPossitions.Add(Level.drawCurveQuadratic(0.0f, 0.0f, 0.5f, 0.5f, 1.6f, 0.0f, 0.005f, rand.Next(0, 2)));
            enemyPossitions.Add(Level.drawCurveCubic(0.0f, 0.17f, 0.85f, 0.93f, 0.33f, 1.98f, -0.27f, 0.71f, 0.005f, rand.Next(0, 2)));
            enemyPossitions.Add(Level.drawCurveCubic(0.0f, 0.0f, 0.5f, 0.3f, 0.4f, 0.0f, 0.8f, 0.0f, 0.005f, rand.Next(0, 2)));
            enemyPossitions.Add(Level.drawCurveCubic(0.0f, 0.0f, 0.5f, 0.3f, 0.4f, 0.5f, 1.6f, 0.0f, 0.005f, rand.Next(0, 2)));

            return new LevelContent(this, 8, 2, 2, enemyPossitions, enemyType);
        }

        internal LevelContent getLevelTree()
        {
            List<List<Vector2>> enemyPossitions = new List<List<Vector2>>(10);
            List<KeyValuePair<EnemyTypes, int>> enemyType = new List<KeyValuePair<EnemyTypes, int>>();

            enemyType.Add(new KeyValuePair<EnemyTypes, int>(EnemyTypes.Easy, 10));
            enemyType.Add(new KeyValuePair<EnemyTypes, int>(EnemyTypes.Middle, 8));
            enemyType.Add(new KeyValuePair<EnemyTypes, int>(EnemyTypes.Hard, 4));

            Random rand = new Random(enemyType.Count);
            enemyPossitions.Add(Level.drawCurveQuadratic(0.0f, 0.0f, 0.5f, 0.5f, 1.6f, 0.0f, 0.005f, rand.Next(0, 2)));
            enemyPossitions.Add(Level.drawCurveFlat(0.0f, 0.40f, 0.99f, 0.01f, 0.005f, rand.Next(0, 2)));
            enemyPossitions.Add(Level.drawCurveQuadratic(0.0f, 0.0f, 0.5f, 0.5f, 1.6f, 0.0f, 0.005f, rand.Next(0, 2)));
            enemyPossitions.Add(Level.drawCurveCubic(0.0f, 0.17f, 0.85f, 0.93f, 0.33f, 1.98f, -0.27f, 0.71f, 0.005f, rand.Next(0, 2)));
            enemyPossitions.Add(Level.drawCurveCubic(0.0f, 0.0f, 0.5f, 0.3f, 0.4f, 0.0f, 0.8f, 0.0f, 0.005f, rand.Next(0, 2)));
            enemyPossitions.Add(Level.drawCurveCubic(0.0f, 0.0f, 0.5f, 0.3f, 0.4f, 0.5f, 1.6f, 0.0f, 0.005f, rand.Next(0, 2)));

            return new LevelContent(this, 12, 3, 2, enemyPossitions, enemyType, true);
        }
    }
}
