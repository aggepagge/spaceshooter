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
    /// <summary>
    /// Klass som fyller innehåll för de olika nivåerna/banorna (Genom att skapa och fylla LevelContent-objekt)
    /// samt sätter 
    /// </summary>
    class Level
    {
        //Prop för spelarens startpossition
        public Vector2 StartPossition { get; private set; }
        public float BoardWidth { get; private set; }
        public float BoardHeight { get; private set; }
        public float BoardTotalWidth { get; private set; }

        //Initsierar startpossitionerna för spelaren
        internal Level()
        {
            BoardWidth = XNAController.BOARD_LOGIC_WIDTH;
            BoardHeight = XNAController.BOARD_LOGIC_HEIGHT;
            BoardTotalWidth = XNAController.BOARD_LOGIC_GAMEFIELDWIDTH;
            StartPossition = new Vector2(BoardTotalWidth / 2, BoardHeight * 0.9f);
        }

        //Uträkningen av Bezier-kurvan är inspirerad av: http://www.gamedev.net/reference/articles/article1808.asp
        //Skapar en List<Vector2> som innehåller X och Y-possitioner för fiendeskeppen basserat på 3 st XY-possitioner
        //degrade anger hur stort glappet ska vara mellan possitionerna
        //reverse anger om listan av possitioner ska vändas och därmed få banan att gå åt motsatt håll (Detta för att öka variationen på flyg-banorna)
        internal static List<Vector2> drawCurveFlat(float aX, float aY, float bX, float bY, float degrade, int reverse)
        {
            List<Vector2> possitions = new List<Vector2>();

            //Bizier-uträkning
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

            //Om 1 så omvänd ordning
            if (reverse == 1)
                possitions.Reverse();

            return possitions;
        }

        //Uträkningen av Bezier-kurvan är inspirerad av: http://www.gamedev.net/reference/articles/article1808.asp
        //Skapar en List<Vector2> som innehåller X och Y-possitioner för fiendeskeppen basserat på 4 st XY-possitioner
        //degrade anger hur stort glappet ska vara mellan possitionerna
        //reverse anger om listan av possitioner ska vändas och därmed få banan att gå åt motsatt håll (Detta för att öka variationen på flyg-banorna)
        internal static List<Vector2> drawCurveQuadratic(float aX, float aY, float bX, float bY, float cX, float cY, float degrade, int reverse)
        {
            List<Vector2> possitions = new List<Vector2>();

            //Bizier-uträkning
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

            //Om 1 så omvänd ordning
            if (reverse == 1)
                possitions.Reverse();

            return possitions;
        }

        //Uträkningen av Bezier-kurvan är inspirerad av: http://www.gamedev.net/reference/articles/article1808.asp
        //Skapar en List<Vector2> som innehåller X och Y-possitioner för fiendeskeppen basserat på 5 st XY-possitioner
        //degrade anger hur stort glappet ska vara mellan possitionerna
        //reverse anger om listan av possitioner ska vändas och därmed få banan att gå åt motsatt håll (Detta för att öka variationen på flyg-banorna)
        internal static List<Vector2> drawCurveCubic(float aX, float aY, float bX, float bY, float cX, float cY, float dX, float dY, float degrade, int reverse)
        {
            List<Vector2> possitions = new List<Vector2>();

            //Bizier-uträkning
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

            //Om 1 så omvänd ordning
            if (reverse == 1)
                possitions.Reverse();
            
            return possitions;
        }

        //Skapar innehåll för första banan/nivån
        internal LevelContent getLevelOne()
        {
            //Lista med listor av Vector2 för possitioner för fiendeskepp
            List<List<Vector2>> enemyPossitions = new List<List<Vector2>>(10);
            //Lista med par för fiendetyp och antal av fiender som ska skapas
            List<KeyValuePair<EnemyTypes, int>> enemyType = new List<KeyValuePair<EnemyTypes, int>>();

            //Adderar Fiendetyper och hur många som ska skapas av varje typ
            enemyType.Add(new KeyValuePair<EnemyTypes, int>(EnemyTypes.Easy, 40));
            enemyType.Add(new KeyValuePair<EnemyTypes, int>(EnemyTypes.Middle, 20));
            enemyType.Add(new KeyValuePair<EnemyTypes, int>(EnemyTypes.Hard, 12));

            //Ranndom-objekt som används för att ge en int som är 0 eller 1. Värdet används sedan för att reversera listan av possitioner
            Random rand = new Random(enemyType.Count);
            //Skapar ´listor med banor för fiendeskeppen
            enemyPossitions.Add(Level.drawCurveQuadratic(0.0f, 0.0f, 0.5f, 0.5f, 1.6f, 0.0f, 0.005f, rand.Next(0, 2)));
            enemyPossitions.Add(Level.drawCurveFlat(0.0f, 0.40f, 0.99f, 0.01f, 0.005f, rand.Next(0, 2)));
            enemyPossitions.Add(Level.drawCurveQuadratic(0.0f, 0.0f, 0.5f, 0.5f, 1.6f, 0.0f, 0.005f, rand.Next(0, 2)));
            enemyPossitions.Add(Level.drawCurveCubic(0.0f, 0.17f, 0.85f, 0.93f, 0.33f, 1.98f, -0.27f, 0.71f, 0.005f, rand.Next(0, 2)));
            enemyPossitions.Add(Level.drawCurveCubic(0.0f, 0.0f, 0.5f, 0.3f, 0.4f, 0.0f, 0.8f, 0.0f, 0.005f, rand.Next(0, 2)));
            enemyPossitions.Add(Level.drawCurveCubic(0.0f, 0.0f, 0.5f, 0.3f, 0.4f, 0.5f, 1.6f, 0.0f, 0.005f, rand.Next(0, 2)));

            //Returnerar banans/nivåns innehåll
            //Första inten är för hur många kometer som ska skapas, andra för antalet hälso-powerup's 
            //och sista inten är för hur många vapen-powerup's som ska skapas
            return new LevelContent(this, 0, 8, 4, enemyPossitions, enemyType);
        }

        //Skapar innehåll för andra banan/nivån
        internal LevelContent getLevelTwo()
        {
            //Lista med listor av Vector2 för possitioner för fiendeskepp
            List<List<Vector2>> enemyPossitions = new List<List<Vector2>>(10);
            //Lista med par för fiendetyp och antal av fiender som ska skapas
            List<KeyValuePair<EnemyTypes, int>> enemyType = new List<KeyValuePair<EnemyTypes, int>>();

            //Adderar Fiendetyper och hur många som ska skapas av varje typ
            enemyType.Add(new KeyValuePair<EnemyTypes, int>(EnemyTypes.Easy, 50));
            enemyType.Add(new KeyValuePair<EnemyTypes, int>(EnemyTypes.Middle, 30));
            enemyType.Add(new KeyValuePair<EnemyTypes, int>(EnemyTypes.Hard, 20));

            //Ranndom-objekt som används för att ge en int som är 0 eller 1. Värdet används sedan för att reversera listan av possitioner
            Random rand = new Random(enemyType.Count);
            //Skapar listor med banor för fiendeskeppen
            enemyPossitions.Add(Level.drawCurveQuadratic(0.0f, 0.0f, 0.5f, 0.5f, 1.6f, 0.0f, 0.005f, rand.Next(0, 2)));
            enemyPossitions.Add(Level.drawCurveFlat(0.0f, 0.40f, 0.99f, 0.01f, 0.005f, rand.Next(0, 2)));
            enemyPossitions.Add(Level.drawCurveQuadratic(0.0f, 0.0f, 0.5f, 0.5f, 1.6f, 0.0f, 0.005f, rand.Next(0, 2)));
            enemyPossitions.Add(Level.drawCurveCubic(0.0f, 0.17f, 0.85f, 0.93f, 0.33f, 1.98f, -0.27f, 0.71f, 0.005f, rand.Next(0, 2)));
            enemyPossitions.Add(Level.drawCurveCubic(0.0f, 0.0f, 0.5f, 0.3f, 0.4f, 0.0f, 0.8f, 0.0f, 0.005f, rand.Next(0, 2)));
            enemyPossitions.Add(Level.drawCurveCubic(0.0f, 0.0f, 0.5f, 0.3f, 0.4f, 0.5f, 1.6f, 0.0f, 0.005f, rand.Next(0, 2)));

            //Returnerar banans/nivåns innehåll
            //Första inten är för hur många kometer som ska skapas, andra för antalet hälso-powerup's 
            //och sista inten är för hur många vapen-powerup's som ska skapas
            return new LevelContent(this, 20, 14, 8, enemyPossitions, enemyType);
        }

        internal LevelContent getLevelTree()
        {
            //Lista med listor av Vector2 för possitioner för fiendeskepp
            List<List<Vector2>> enemyPossitions = new List<List<Vector2>>(10);
            //Lista med par för fiendetyp och antal av fiender som ska skapas
            List<KeyValuePair<EnemyTypes, int>> enemyType = new List<KeyValuePair<EnemyTypes, int>>();

            //Adderar Fiendetyper och hur många som ska skapas av varje typ
            enemyType.Add(new KeyValuePair<EnemyTypes, int>(EnemyTypes.Easy, 30));
            enemyType.Add(new KeyValuePair<EnemyTypes, int>(EnemyTypes.Middle, 16));
            enemyType.Add(new KeyValuePair<EnemyTypes, int>(EnemyTypes.Hard, 10));

            //Ranndom-objekt som används för att ge en int som är 0 eller 1. Värdet används sedan för att reversera listan av possitioner
            Random rand = new Random(enemyType.Count);
            //Skapar listor med banor för fiendeskeppen
            enemyPossitions.Add(Level.drawCurveQuadratic(0.0f, 0.0f, 0.5f, 0.5f, 1.6f, 0.0f, 0.005f, rand.Next(0, 2)));
            enemyPossitions.Add(Level.drawCurveFlat(0.0f, 0.40f, 0.99f, 0.01f, 0.005f, rand.Next(0, 2)));
            enemyPossitions.Add(Level.drawCurveQuadratic(0.0f, 0.0f, 0.5f, 0.5f, 1.6f, 0.0f, 0.005f, rand.Next(0, 2)));
            enemyPossitions.Add(Level.drawCurveCubic(0.0f, 0.17f, 0.85f, 0.93f, 0.33f, 1.98f, -0.27f, 0.71f, 0.005f, rand.Next(0, 2)));
            enemyPossitions.Add(Level.drawCurveCubic(0.0f, 0.0f, 0.5f, 0.3f, 0.4f, 0.0f, 0.8f, 0.0f, 0.005f, rand.Next(0, 2)));
            enemyPossitions.Add(Level.drawCurveCubic(0.0f, 0.0f, 0.5f, 0.3f, 0.4f, 0.5f, 1.6f, 0.0f, 0.005f, rand.Next(0, 2)));

            //Returnerar banans/nivåns innehåll
            //Första inten är för hur många kometer som ska skapas, andra för antalet hälso-powerup's 
            //och sista inten är för hur många vapen-powerup's som ska skapas. 
            //True gör att banan får en boss
            return new LevelContent(this, 20, 10, 4, enemyPossitions, enemyType, true);
        }
    }
}
