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
    /// <summary>
    /// Den här klassen skapar innehåll för en bana (level). Den tar en lista som innehåller listor av vectorer och dessa används för
    /// fiendeskeppen så dom får olika banor i spelet. Den tar dessutom lista som innehåller par av fiendetyper och int för hur många
    /// fiender som ska skapas av varje typ
    /// </summary>
    class LevelContent
    {
        //Lista av listor med Vector2 för X och Y-possitioner för rymdskepp
        private List<List<Vector2>> enemyPossitions;
        //Samlingar av olika spelelement som lagras i properties
        internal List<EnemySpaceShip> EnemyStorage { get; private set; }
        internal List<GameObsticle> ObsticlesStorage { get; private set; }
        internal ArrayList PowerUpStorage { get; private set; }
        internal EnemyBossSpaceShip Boss { get; private set; }
        internal bool CreateBoss { get; private set; }
        //List mer par av fiendetyper och antal som ska skapas av varje typ
        private List<KeyValuePair<EnemyTypes, int>> Enemies { get; set; }
        private Level level;
        //Variabler som används för att ge en jämn fördelning över när ett nytt spelelement ska komma in på banan
        private int numberOfObsticles;
        private int numberOfPowerUps;
        private int numberOfNewWeapons;

        //Förutom listorna som beskrivits tidigare så tar konstruktorn int för antal powerup (För hälsa och för vapenuppgradering)
        //samt för hur många kometer som ska skapas. Sist är det en bool för om banan ska innehålla någon slut-boss
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

        //Metod som byter possition på objekten i enemystorage-listan.
        //Metoden har hittats på stackoverflow
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

        //Metod som skapar innehållet för banan (Initsierar fiender, powerups, vapen, kometer och eventuell boss 
        private void setLevelContent()
        {
            //Går igenom paren av fiendetyper och antal fiender
            foreach (KeyValuePair<EnemyTypes, int> enemyAndType in Enemies)
            {
                //count används för att hämta ut en List<Vector2>-objekt som skickas med till fiendeskeppet
                int count = 0;
                //Skapar fiender av Key-typ Value-antal gånger 
                for (int i = 0; i < enemyAndType.Value; i++)
                {
                    if (count > enemyPossitions.Count - 1)
                        count = 0;

                    //Adderar fienden till fiende-listan
                    //StaticHelper är en statisk klas som returnerar rätt värden för vapen och fiendetyper
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

            //Blandar fienderna
            shuffleEnemies();

            //Skapar kometer
            for (int i = 0; i < numberOfObsticles; i++)
            {
                Random rand = new Random(i);

                ObsticlesStorage.Add(new GameObsticle(
                                                    i,
                                                    0.8f,
                                                    new Vector2((float)(rand.NextDouble() * 1.6), -0.1f),
                                                    30
                                               ));
            }

            //Skapar powerups
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

            //Gör uträkning så vapenuppgraderingarna kommer in i jämn fördelning mot powerupsen
            int insertPlace = 0;
            int countAddPowerUp = 0;

            if(PowerUpStorage.Count > 0)
                insertPlace = PowerUpStorage.Count / 2;

            Random random = new Random(PowerUpStorage.Count);

            //Powerups och weapong upgrade är samma objekt-typer så weapon upgrade adderas till samma samling
            //Dom är inte subklasser, vilket jag skulle gjort om jag hade mer tid) :)
            do
            {
                PowerUp tmpPowerUp = new PowerUp(
                                                    0.1f,
                                                    0.1f,
                                                    new Vector2((float)(random.NextDouble() * 1.6), -0.1f),
                                                    PowerUpType.Wepon
                                                );
                
                //Insertar vapenuppgraderingar  
                PowerUpStorage.Insert(insertPlace, tmpPowerUp);
                insertPlace += insertPlace;
                countAddPowerUp++;
            }
            while (countAddPowerUp > insertPlace);

            //Om det ska skapas en boss så görs det här
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
