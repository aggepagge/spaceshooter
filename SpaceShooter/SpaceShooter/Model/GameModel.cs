using System;
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
using SpaceShooter.Model.GameComponents.Parts;

namespace SpaceShooter.Model
{
    /// <summary>
    /// Model-klassen som hanterar all logisk uppdatering av spelet
    /// </summary>
    class GameModel
    {
        internal PlayerSpaceShip Player { get; private set; }
        internal Level Level { get; private set; }
        internal LevelContent levelContent;

        internal List<EnemySpaceShip> EnemyShips { get; private set; }
        internal List<Weapon> Shoots { get; private set; }
        internal List<GameObsticle> Obsticles { get; private set; }
        internal List<PowerUp> Power { get; private set; }
        internal EnemyBossSpaceShip TheBoss { get; private set; }
        private int countNewEnemy = 0;
        private int countNewObsticle = 0;
        private int countNewPowerUp = 0;

        private float createNewShipCount = 0.0f;
        private float createNewObsticleCount = 0.0f;
        private float createNewPowerUp = 0.0f;
        private float restart_count_powerup = 0.0f;
        private static float RESTART_COUNT_ENEMY = 1.0f;
        private float restart_count_obsticle = 3.3f;
        internal float GameTime { get; private set; }
        internal bool LevelFinished { get; private set; }
        internal int LevelCount { get; set; }

        private const float TIME_TO_NEXT_LEVEL = 4.0f;
        private float nextLevelTimer = 0.0f;

        //Konstruktor som initsierar spelet
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
            Obsticles = new List<GameObsticle>();
            Power = new List<PowerUp>();
            GameTime = 0.0f;
            LevelFinished = false;
            LevelCount = 1;

            setTimeForObsticlesAndPowerUps();
        }

        //Metod som initsierar spelet (Nästan som konstruktorn)
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
            Obsticles.Clear();
            Power.Clear();

            levelContent = Level.getLevelOne();
            Player.setPossitionX(Level.StartPossition.X);
            Player.setPossitionY(Level.StartPossition.Y);

            //Om det inte ska finnas någon boss på banan
            if (!levelContent.CreateBoss)
                TheBoss = null;

            listener.restartGame();
            GameTime = 0.0f;
            countNewEnemy = 0;
            countNewObsticle = 0;
            countNewPowerUp = 0;
            createNewObsticleCount = 0.0f;
            createNewShipCount = 0.0f;
            LevelFinished = false;
            LevelCount = 1;

            setTimeForObsticlesAndPowerUps();
        }

        //Metod som initsierar spelet för nästa bana/nivå
        internal void playNextLevel(IGameModelListener listener)
        {
            EnemyShips.Clear();
            Shoots.Clear();
            Obsticles.Clear();
            Power.Clear();

            Player.setPossition(Level.StartPossition.X, Level.StartPossition.Y);
            Player.Firering = false;
            Player.setFullHealt();

            GameTime = 0.0f;
            countNewEnemy = 0;
            countNewObsticle = 0;
            countNewPowerUp = 0;
            LevelFinished = false;

            LevelCount++;
            listener.setNextLevel(LevelCount);

            //Kollar vilken bana/nivå som ska skapas
            if (LevelCount == 2)
                levelContent = Level.getLevelTwo();
            else if (LevelCount == 3)
                levelContent = Level.getLevelTree();

            //Om det inte ska finnas någon boss på banan
            if (!levelContent.CreateBoss)
                TheBoss = null;

            setTimeForObsticlesAndPowerUps();
        }

        //Räknar ut om och hur tiden för nästa powerup och kometer ska vara basserat på antal fiender + väntetiden för när 
        //nästa fiende ska skapas. Detta för att powerupsen och kometerna ska komma i ett jämt spann i förhållande till 
        //antalet fiender (D.v.s. om det är många fiender så blir väntetiden för nästa powerup och komet längre)
        private void setTimeForObsticlesAndPowerUps()
        {
            float timeForEnemies = RESTART_COUNT_ENEMY * levelContent.EnemyStorage.Count;
            float timeForObsticles = restart_count_obsticle * levelContent.ObsticlesStorage.Count;

            if (timeForEnemies > timeForObsticles && levelContent.ObsticlesStorage.Count > 0)
            {
                restart_count_obsticle = timeForEnemies / levelContent.ObsticlesStorage.Count;
            }

            if (timeForEnemies > timeForObsticles)
                restart_count_powerup = timeForEnemies / levelContent.PowerUpStorage.Count;
            else
                restart_count_powerup = timeForObsticles / levelContent.PowerUpStorage.Count;
        }

        //Metod som kollar om det finns någon nästa nivå 
        //(false) eller om spelet är slut
        internal bool gameIsFinished()
        {
            if (LevelCount + 1 == 4)
                return true;

            return false;
        }

        //Uppdaterar modellen
        internal void UpdateModel(float elapsedGameTime, IGameModelListener listener)
        {
            GameTime += elapsedGameTime;

            Player.Update(elapsedGameTime);

            //raderar alla objekt i samlingarna som har RemoveMe satt till true
            EnemyShips.RemoveAll(x => x.RemoveMe == true);
            Shoots.RemoveAll(x => x.RemoveMe == true);
            Obsticles.RemoveAll(x => x.RemoveMe == true);
            Power.RemoveAll(x => x.RemoveMe == true);

            //Om spelaren skuter och det är dags att skuta
            if (Player.Firering && Player.readyToFire())
            {
                //hämtar vapentyp för spelaren
                WeaponTypes weaponType = Player.getCurrentWeapon();

                //Om vapnet skuter mer än en kula (Så är det WeapnType.Plasma)
                if (StaticHelper.getBulletCount(weaponType) > 1)
                {
                    int count = 0;
                    //Loopar för varje kula
                    //Borde kolla StaticHelper.getBulletCount(weaponType) men
                    //då det bara finns ett spelarvapen som skuter tre skott
                    //så har det "hårdkodats" in här
                    while (count < 3)
                    {
                        float directionX = 0.0f;

                        if (count == 0)
                            directionX = -0.6f;
                        else if (count == 2)
                            directionX = 0.6f;

                        //Skapar nytt skott
                        Weapon gunfire = new Weapon(
                                                Player.getCenterTopPossition(),
                                                weaponType,
                                                StaticHelper.getBulletWidth(weaponType),
                                                StaticHelper.getBulletHeight(weaponType),
                                                StaticHelper.getFireDamage(weaponType),
                                                new Vector2(directionX, StaticHelper.getFireSpeed(weaponType)),
                                                false,
                                                false
                                            );

                        Shoots.Add(gunfire);
                        count++;
                    }
                }
                //Om vapnet bara skuter en kula
                else
                {
                    Weapon gunfire = new Weapon(
                                                Player.getCenterTopPossition(),
                                                weaponType,
                                                StaticHelper.getBulletWidth(weaponType),
                                                StaticHelper.getBulletHeight(weaponType),
                                                StaticHelper.getFireDamage(weaponType),
                                                new Vector2(0.0f, StaticHelper.getFireSpeed(weaponType)),
                                                false,
                                                false
                                            );

                    Shoots.Add(gunfire);
                }
            }

            //Loopar igenom alla fiandeskepp
            foreach (EnemySpaceShip enemy in EnemyShips)
            {
                enemy.Update(elapsedGameTime);

                //Om vapnet skuter mer än en kula (Så är det WeapnType.EnemyBossPlasma)
                if (enemy.ReadyToFire)
                {
                    //hämtar vapentyp
                    WeaponTypes weaponType = enemy.WeaponType;

                    //Om vapnet skuter mer än en kula (Så är det WeapnType.Plasma)
                    if (StaticHelper.getBulletCount(weaponType) > 1)
                    {
                        int count = 0;
                        while (count < 3)
                        {
                            float directionX = 0.0f;

                            if (count == 0)
                                directionX = -0.6f;
                            else if (count == 2)
                                directionX = 0.6f;

                            //Skapar nytt skott
                            Weapon gunfire = new Weapon(
                                                    enemy.getCenterBottomPossition(),
                                                    weaponType,
                                                    StaticHelper.getBulletWidth(weaponType),
                                                    StaticHelper.getBulletHeight(weaponType),
                                                    StaticHelper.getFireDamage(weaponType),
                                                    new Vector2(directionX, StaticHelper.getFireSpeed(weaponType)),
                                                    false,
                                                    true
                                                );

                            Shoots.Add(gunfire);
                            count++;
                        }
                    }
                    //Om vapnet bara skuter en kula
                    else
                    {
                        //Skapar nytt skott
                        Weapon gunfire = new Weapon(
                                                    enemy.getCenterBottomPossition(),
                                                    weaponType,
                                                    StaticHelper.getBulletWidth(weaponType),
                                                    StaticHelper.getBulletHeight(weaponType),
                                                    StaticHelper.getFireDamage(weaponType),
                                                    new Vector2(0.0f, StaticHelper.getFireSpeed(weaponType)),
                                                    false,
                                                    true
                                                );

                        Shoots.Add(gunfire);
                    }

                    enemy.ReadyToFire = false;
                }
            }

            //Kollar om banan/nivån innehåller en boss och att banans/nivåns alla fiender och kometer är slut,
            //då skall bossen uppdateras
            if (levelContent.CreateBoss && 
                countNewEnemy >= levelContent.EnemyStorage.Count && 
                countNewObsticle >= levelContent.ObsticlesStorage.Count)
            {
                //Hämtar referens till banans boss-objekt
                if (TheBoss == null)
                    TheBoss = levelContent.Boss;

                TheBoss.Update(elapsedGameTime);
                
                //Kollar om bossen ska skuta och bossen inte är död
                if (TheBoss.ReadyToFire && !TheBoss.RemoveMe)
                {
                    //Loopar igenom alla possitioner (3 st) som bossens vapen skuter
                    //(Och bossen skuter ju från tre possitioner (Vänster, mitt och höger))
                    foreach (Vector2 poss in TheBoss.FirePossitions)
                    {
                        WeaponTypes weaponType = TheBoss.WeaponType;

                        int count = 0;
                        while (count < 3)
                        {
                            float directionX = 0.0f;

                            //Räknar upp för X-förflyttning
                            if (count == 0)
                                directionX = -0.8f;
                            else if (count == 2)
                                directionX = 0.8f;

                            //Räknar ut var ifrån bossens area skottet skuts ifrån
                            //(poss är vector2 med dessa possitioner)
                            float xPoss = TheBoss.getPossitionX() + poss.X;
                            float yPoss = TheBoss.getPossitionY() - (TheBoss.SpaceShipHeight / 2) + poss.Y;

                            //Adderar skottet
                            Weapon gunfire = new Weapon(
                                                    new Vector2(xPoss, yPoss),
                                                    weaponType,
                                                    StaticHelper.getBulletWidth(weaponType),
                                                    StaticHelper.getBulletHeight(weaponType),
                                                    StaticHelper.getFireDamage(weaponType),
                                                    new Vector2(directionX, StaticHelper.getFireSpeed(weaponType)),
                                                    false,
                                                    true
                                                );

                            Shoots.Add(gunfire);
                            count++;
                        }
                        TheBoss.ReadyToFire = false;
                    }
                }
            }
            
            //Uppdaterar alla skotten
            foreach (Weapon shot in Shoots)
            {
                shot.Update(elapsedGameTime);

                //Om det är spelarens kula
                if (!shot.EnemyWepon)
                {
                    FloatRectangle shotRect = FloatRectangle.createFromLeftTop(new Vector2(shot.PossitionX, shot.PossitionY), shot.Width, shot.Height);

                    //Kollar om kulan träffar någon av fienderna
                    foreach (EnemySpaceShip enemy in EnemyShips)
                    {
                        //Om en fiende träffats 
                        if (enemy.HasBeenShoot(shotRect))
                        {
                            //fiendens hälsa minskas med kulans skada
                            enemy.Healt -= shot.Damage;
                            //Spelarens poäng räknas upp
                            Player.PlayerScoore += shot.Damage;
                            //Skottet görs redo att tas bort
                            shot.RemoveMe = true;
                            shot.Damage = 0;

                            //Kollar om fienden skadas eller dör
                            if (enemy.Healt < 1)
                            {
                                enemy.RemoveMe = true;
                                Player.PlayerScoore += enemy.DeathPoint;
                                //Om död så anropas killed-metoden som tar två Vector-possitioner så explotionen rör sig i skeppets riktning
                                listener.killed(enemy.getCurrentAndPreviousPossition());
                            }
                            else
                                listener.wounded(new Vector2(shot.PossitionX, shot.PossitionY));
                        }
                    }

                    //Om bossen blivit skuten (Om bossen inte är null)
                    if (TheBoss != null && TheBoss.HasBeenShoot(shotRect))
                    {
                        //Minskar bossens hälsa
                        TheBoss.Healt -= shot.Damage;
                        Player.PlayerScoore += shot.Damage;
                        shot.RemoveMe = true;
                        shot.Damage = 0;

                        //Kollar om bossen skadas eller dör
                        if (TheBoss.Healt < 1)
                        {
                            TheBoss.RemoveMe = true;
                            Player.PlayerScoore += TheBoss.DeathPoint;
                            listener.killed(new Vector2(TheBoss.getPossitionX() + (TheBoss.SpaceShipWidth / 2), TheBoss.getPossitionY()));
                            TheBoss.imDead();
                        }
                        else
                            listener.wounded(new Vector2(shot.PossitionX, shot.PossitionY));
                    }

                    //Loopar alla kometer
                    foreach (GameObsticle obsticle in Obsticles)
                    {
                        //Om kometen blivit träffad
                        if (obsticle.HasBeenShoot(shotRect))
                        {
                            obsticle.Healt -= shot.Damage;
                            Player.PlayerScoore += shot.Damage;
                            shot.RemoveMe = true;
                            shot.Damage = 0;

                            //Kollar om kometen skadas eller dör
                            if (obsticle.Healt < 1)
                            {
                                obsticle.RemoveMe = true;
                                Player.PlayerScoore += obsticle.DeathPoint;
                                listener.killed(new Vector2(obsticle.getPossitionX(), obsticle.getPossitionY()));
                            }
                            else
                                listener.wounded(new Vector2(shot.PossitionX, shot.PossitionY));
                        }
                    }
                }
                //Om skottet kommer från ett fiendeskepp
                else if (shot.EnemyWepon)
                {
                    FloatRectangle shotRect = FloatRectangle.createFromLeftTop(new Vector2(shot.PossitionX, shot.PossitionY), shot.Width, shot.Height);

                    //Kollar om spelaren blivit träffad
                    if (Player.HasBeenShoot(shotRect))
                    {
                        Player.Healt -= shot.Damage;
                        shot.RemoveMe = true;
                        shot.Damage = 0;

                        if (Player.Healt < 1)
                        {
                            Player.RemoveMe = true;
                            listener.killed(new Vector2(shot.PossitionX, shot.PossitionY));
                        }
                        else
                            listener.wounded(new Vector2(shot.PossitionX, shot.PossitionY));
                    }
                }
            }

            //Loopar alla kometer
            foreach (GameObsticle obsticle in Obsticles)
            {
                obsticle.Update(elapsedGameTime);

                FloatRectangle asteroidRect = FloatRectangle.createFromCenter(
                                                                                new Vector2(obsticle.getPossitionX(),
                                                                                            obsticle.getPossitionY()),
                                                                                obsticle.Size
                                                                               );

                //Kollar om kometen träffar spelaren
                if (Player.HasBeenShoot(asteroidRect))
                {
                    Player.Healt -= obsticle.Damage;
                    obsticle.RemoveMe = true;
                    obsticle.Damage = 0;
                    listener.killed(new Vector2(obsticle.getPossitionX(), obsticle.getPossitionY()));

                    //Kollar om spelaren skadas elle dör
                    if (Player.Healt < 1)
                    {
                        Player.RemoveMe = true;
                        listener.killed(Player.getShipPossition());
                    }
                    else
                        listener.wounded(Player.getShipPossition());
                }
            }

            //Loopar igenom alla powerups
            foreach (PowerUp powerup in Power)
            {
                powerup.Update(elapsedGameTime);

                FloatRectangle powerUpRect = FloatRectangle.createFromCenter(
                                                                                new Vector2(powerup.getPossitionX(),
                                                                                            powerup.getPossitionY()),
                                                                                powerup.Size
                                                                               );

                //Kollar om spelaren fångat powerup'en (E dumt med namnet på HasBeenShoot här)
                if (Player.HasBeenShoot(powerUpRect))
                {
                    //Kollar vilken typ av powerup det är
                    if (powerup.Type == PowerUpType.Health)
                    {
                        Player.Healt = Player.PlayerStartHealt;
                        powerup.RemoveMe = true;
                    }
                    else
                    {
                        Player.setCurrentWeapon(WeaponTypes.Plasma);
                        powerup.RemoveMe = true;
                    }
                }
            }

            createNewPowerUp += elapsedGameTime;

            //Kollar om det är dags att addera en powerup till banan och ifall det finns någon powerup kvar att addera
            if (createNewPowerUp > restart_count_powerup && countNewPowerUp < levelContent.PowerUpStorage.Count)
            {   
                //Hämtar ut nästa powerup ifrån levelContent-objektet (Borde vara ifrån en samling 
                //som man kan poppa ifrån så den raderas, men jag har inte kodat C# på flera år så skapade
                //listor istället och håller koll på nästa objekt med uppräkningsvariabler)
                Power.Add((PowerUp)levelContent.PowerUpStorage[countNewPowerUp]);

                createNewPowerUp = 0.0f;
                countNewPowerUp++;
            }

            createNewShipCount += elapsedGameTime;

            //Kollar om det är dags att addera en fiende till banan och ifall det finns någon fiende kvar att addera
            if (createNewShipCount > RESTART_COUNT_ENEMY && countNewEnemy < levelContent.EnemyStorage.Count)
            {
                //Hämtar ut nästa fiende ifrån levelContent-objektet 
                EnemyShips.Add(levelContent.EnemyStorage[countNewEnemy]);

                createNewShipCount = 0.0f;
                countNewEnemy++;
            }

            createNewObsticleCount += elapsedGameTime;

            //Kollar om det är dags att addera en komet till banan och ifall det finns någon komet kvar att addera
            if (createNewObsticleCount > restart_count_obsticle && countNewObsticle < levelContent.ObsticlesStorage.Count)
            {
                //Hämtar ut nästa komet ifrån levelContent-objektet 
                Obsticles.Add(levelContent.ObsticlesStorage[countNewObsticle]);

                createNewObsticleCount = 0.0f;
                countNewObsticle++;
            }

            //Om det inte finns några fiender eller kometer kvar i levelcontent
            if (countNewEnemy >= levelContent.EnemyStorage.Count && countNewObsticle >= levelContent.ObsticlesStorage.Count)
            {
                //Kollar om banan har en boss
                if (levelContent.CreateBoss)
                {
                    //Om alla fiender, kometer och bossen är döda så startar uppräkningen för att visa 
                    //menyn (Detta så menyn inte ska dyka upp såfort man dödat alla fiender och bossen)
                    if (EnemyShips.Count == 0 && Obsticles.Count == 0 && TheBoss.RemoveMe)
                    {
                        nextLevelTimer += elapsedGameTime;

                        if (nextLevelTimer > TIME_TO_NEXT_LEVEL)
                        {
                            LevelFinished = true;
                            nextLevelTimer = 0.0f;
                        }
                    }
                }
                //Om banan inte innehåller någon boss
                else
                {
                    //Om alla fiender och kometer är döda så startar uppräkningen för att visa 
                    //menyn (Detta så menyn inte ska dyka upp såfort man dödat alla fiender)
                    if (EnemyShips.Count == 0 && Obsticles.Count == 0)
                    {
                        nextLevelTimer += elapsedGameTime;

                        if (nextLevelTimer > TIME_TO_NEXT_LEVEL)
                        {
                            LevelFinished = true;
                            nextLevelTimer = 0.0f;
                        }
                    }
                }
            }

            //Kollar om spelaren dött. Då startar timern så menyn inte visas direkt
            if (Player.RemoveMe)
            {
                nextLevelTimer += elapsedGameTime;

                if (nextLevelTimer > TIME_TO_NEXT_LEVEL)
                {
                    LevelFinished = true;
                    nextLevelTimer = 0.0f;
                }
            }
        }

        //Rör spelaren uppåt
        internal void playerMovesUp()
        {
            Player.setPossitionY(-1);
        }

        //Rör spelaren neråt
        internal void playerMovesDown()
        {
            Player.setPossitionY();
        }

        //Rör spelaren åt vänster
        internal void playerMovesLeft()
        {
            Player.setPossitionX(-1);
        }

        //Rör spelaren åt höger
        internal void playerMovesRight()
        {
            Player.setPossitionX();
        }

        //Sätter spelaren till att skuta om han inte gör det eller till ej skutande om han skuter
        internal void playerShoots()
        {
            Player.Firering = !Player.Firering;
        }
    }
}
