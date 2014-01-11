using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SpaceShooter.Model.GameComponents.Levels;

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

        internal static List<Vector2> drawCurveFlat(float aX, float aY, float bX, float bY, float degrade)
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

            return possitions;
        }

        //Uträkningen av Bezier-kurvan är inspirerad av: http://www.gamedev.net/reference/articles/article1808.asp
        //
        internal static List<Vector2> drawCurveQuadratic(float aX, float aY, float bX, float bY, float cX, float cY, float degrade)
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

            return possitions;
        }

        //Uträkningen av Bezier-kurvan är inspirerad av: http://www.gamedev.net/reference/articles/article1808.asp
        //
        internal static List<Vector2> drawCurveCubic(float aX, float aY, float bX, float bY, float cX, float cY, float dX, float dY, float degrade)
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

            return possitions;
        }

        internal LevelContent getLevelOne()
        {
            List<List<Vector2>> enemyPossitions = new List<List<Vector2>>(10);

            enemyPossitions.Add(Level.drawCurveQuadratic(0.0f, 0.0f, 0.5f, 0.5f, 1.6f, 0.0f, 0.005f));
            enemyPossitions.Add(Level.drawCurveFlat(0.0f, 0.40f, 0.99f, 0.01f, 0.005f));
            enemyPossitions.Add(Level.drawCurveQuadratic(0.0f, 0.0f, 0.5f, 0.5f, 1.6f, 0.0f, 0.005f));
            enemyPossitions.Add(Level.drawCurveCubic(0.0f, 0.17f, 0.85f, 0.93f, 0.33f, 1.98f, -0.27f, 0.71f, 0.005f));
            enemyPossitions.Add(Level.drawCurveCubic(0.0f, 0.0f, 0.5f, 0.3f, 0.4f, 0.0f, 0.8f, 0.0f, 0.005f));
            enemyPossitions.Add(Level.drawCurveCubic(0.0f, 0.0f, 0.5f, 0.3f, 0.4f, 0.5f, 1.6f, 0.0f, 0.005f));

            return new LevelContent(this, 10, enemyPossitions);
        }

        internal LevelContent getLevelTwo()
        {
            List<List<Vector2>> enemyPossitions = new List<List<Vector2>>(10);

            enemyPossitions.Add(Level.drawCurveQuadratic(0.0f, 0.0f, 0.5f, 0.5f, 1.6f, 0.0f, 0.005f));
            enemyPossitions.Add(Level.drawCurveFlat(0.0f, 0.40f, 0.99f, 0.01f, 0.005f));
            enemyPossitions.Add(Level.drawCurveQuadratic(0.0f, 0.0f, 0.5f, 0.5f, 1.6f, 0.0f, 0.005f));
            enemyPossitions.Add(Level.drawCurveCubic(0.0f, 0.17f, 0.85f, 0.93f, 0.33f, 1.98f, -0.27f, 0.71f, 0.005f));
            enemyPossitions.Add(Level.drawCurveCubic(0.0f, 0.0f, 0.5f, 0.3f, 0.4f, 0.0f, 0.8f, 0.0f, 0.005f));
            enemyPossitions.Add(Level.drawCurveCubic(0.0f, 0.0f, 0.5f, 0.3f, 0.4f, 0.5f, 1.6f, 0.0f, 0.005f));

            return new LevelContent(this, 10, enemyPossitions);
        }

        internal LevelContent getLevelTree()
        {
            List<List<Vector2>> enemyPossitions = new List<List<Vector2>>(10);

            enemyPossitions.Add(Level.drawCurveQuadratic(0.0f, 0.0f, 0.5f, 0.5f, 1.6f, 0.0f, 0.005f));
            enemyPossitions.Add(Level.drawCurveFlat(0.0f, 0.40f, 0.99f, 0.01f, 0.005f));
            enemyPossitions.Add(Level.drawCurveQuadratic(0.0f, 0.0f, 0.5f, 0.5f, 1.6f, 0.0f, 0.005f));
            enemyPossitions.Add(Level.drawCurveCubic(0.0f, 0.17f, 0.85f, 0.93f, 0.33f, 1.98f, -0.27f, 0.71f, 0.005f));
            enemyPossitions.Add(Level.drawCurveCubic(0.0f, 0.0f, 0.5f, 0.3f, 0.4f, 0.0f, 0.8f, 0.0f, 0.005f));
            enemyPossitions.Add(Level.drawCurveCubic(0.0f, 0.0f, 0.5f, 0.3f, 0.4f, 0.5f, 1.6f, 0.0f, 0.005f));

            return new LevelContent(this, 10, enemyPossitions);
        }
    }
}
