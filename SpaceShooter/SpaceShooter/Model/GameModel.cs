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

            if (LevelCount == 2)
                levelContent = Level.getLevelTwo();
            else if (LevelCount == 3)
                levelContent = Level.getLevelTree();

            if (!levelContent.CreateBoss)
                TheBoss = null;

            setTimeForObsticlesAndPowerUps();
        }

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

        internal bool gameIsFinished()
        {
            if (LevelCount + 1 == 4)
                return true;

            return false;
        }

        internal void UpdateModel(float elapsedGameTime, IGameModelListener listener)
        {
            GameTime += elapsedGameTime;

            Player.Update(elapsedGameTime);

            EnemyShips.RemoveAll(x => x.RemoveMe == true);
            Shoots.RemoveAll(x => x.RemoveMe == true);
            Obsticles.RemoveAll(x => x.RemoveMe == true);
            Power.RemoveAll(x => x.RemoveMe == true);

            if (Player.Firering && Player.readyToFire())
            {
                WeaponTypes weaponType = Player.getCurrentWeapon();

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

            foreach (EnemySpaceShip enemy in EnemyShips)
            {
                enemy.Update(elapsedGameTime);

                if (enemy.ReadyToFire)
                {
                    WeaponTypes weaponType = enemy.WeaponType;

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
                    else
                    {
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

            if (levelContent.CreateBoss && 
                countNewEnemy >= levelContent.EnemyStorage.Count && 
                countNewObsticle >= levelContent.ObsticlesStorage.Count)
            {
                if (TheBoss == null)
                    TheBoss = levelContent.Boss;

                TheBoss.Update(elapsedGameTime);
                
                if (TheBoss.ReadyToFire && !TheBoss.RemoveMe)
                {
                    foreach (Vector2 poss in TheBoss.FirePossitions)
                    {
                        WeaponTypes weaponType = TheBoss.WeaponType;

                        int count = 0;
                        while (count < 3)
                        {
                            float directionX = 0.0f;

                            if (count == 0)
                                directionX = -0.8f;
                            else if (count == 2)
                                directionX = 0.8f;

                            float xPoss = TheBoss.getPossitionX() + poss.X;
                            float yPoss = TheBoss.getPossitionY() - (TheBoss.SpaceShipHeight / 2) + poss.Y;

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
            
            foreach (Weapon shot in Shoots)
            {
                shot.Update(elapsedGameTime);

                if (!shot.EnemyWepon)
                {
                    FloatRectangle shotRect = FloatRectangle.createFromLeftTop(new Vector2(shot.PossitionX, shot.PossitionY), shot.Width, shot.Height);

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
                                listener.killed(enemy.getCurrentAndPreviousPossition());
                            }
                            else
                                listener.wounded(new Vector2(shot.PossitionX, shot.PossitionY));
                        }
                    }

                    if (TheBoss != null && TheBoss.HasBeenShoot(shotRect))
                    {
                        TheBoss.Healt -= shot.Damage;
                        Player.PlayerScoore += shot.Damage;
                        shot.RemoveMe = true;
                        shot.Damage = 0;

                        if (TheBoss.Healt < 1)
                        {
                            TheBoss.RemoveMe = true;
                            Player.PlayerScoore += TheBoss.DeathPoint;
                            listener.killed(new Vector2(TheBoss.getPossitionX(), TheBoss.getPossitionY()));
                            TheBoss.imDead();
                        }
                        else
                            listener.wounded(new Vector2(shot.PossitionX, shot.PossitionY));
                    }

                    foreach (GameObsticle obsticle in Obsticles)
                    {
                        if (obsticle.HasBeenShoot(shotRect))
                        {
                            obsticle.Healt -= shot.Damage;
                            Player.PlayerScoore += shot.Damage;
                            shot.RemoveMe = true;
                            shot.Damage = 0;

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
                else if (shot.EnemyWepon)
                {
                    FloatRectangle shotRect = FloatRectangle.createFromLeftTop(new Vector2(shot.PossitionX, shot.PossitionY), shot.Width, shot.Height);

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

            foreach (GameObsticle obsticle in Obsticles)
            {
                obsticle.Update(elapsedGameTime);

                FloatRectangle asteroidRect = FloatRectangle.createFromCenter(
                                                                                new Vector2(obsticle.getPossitionX(),
                                                                                            obsticle.getPossitionY()),
                                                                                obsticle.Size
                                                                               );

                if (Player.HasBeenShoot(asteroidRect))
                {
                    Player.Healt -= obsticle.Damage;
                    obsticle.RemoveMe = true;
                    obsticle.Damage = 0;
                    listener.killed(new Vector2(obsticle.getPossitionX(), obsticle.getPossitionY()));

                    if (Player.Healt < 1)
                    {
                        Player.RemoveMe = true;
                        listener.killed(Player.getShipPossition());
                    }
                    else
                        listener.wounded(Player.getShipPossition());
                }
            }

            foreach (PowerUp powerup in Power)
            {
                powerup.Update(elapsedGameTime);

                FloatRectangle powerUpRect = FloatRectangle.createFromCenter(
                                                                                new Vector2(powerup.getPossitionX(),
                                                                                            powerup.getPossitionY()),
                                                                                powerup.Size
                                                                               );

                if (Player.HasBeenShoot(powerUpRect))
                {
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

            if (createNewPowerUp > restart_count_powerup && countNewPowerUp < levelContent.PowerUpStorage.Count)
            {   
                Power.Add((PowerUp)levelContent.PowerUpStorage[countNewPowerUp]);

                createNewPowerUp = 0.0f;
                countNewPowerUp++;
            }

            createNewShipCount += elapsedGameTime;

            if (createNewShipCount > RESTART_COUNT_ENEMY && countNewEnemy < levelContent.EnemyStorage.Count)
            {
                EnemyShips.Add(levelContent.EnemyStorage[countNewEnemy]);

                createNewShipCount = 0.0f;
                countNewEnemy++;
            }

            createNewObsticleCount += elapsedGameTime;

            if (createNewObsticleCount > restart_count_obsticle && countNewObsticle < levelContent.ObsticlesStorage.Count)
            {
                Obsticles.Add(levelContent.ObsticlesStorage[countNewObsticle]);

                createNewObsticleCount = 0.0f;
                countNewObsticle++;
            }

            if (countNewEnemy >= levelContent.EnemyStorage.Count && countNewObsticle >= levelContent.ObsticlesStorage.Count)
            {
                if (levelContent.CreateBoss)
                {
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
                else
                {
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
