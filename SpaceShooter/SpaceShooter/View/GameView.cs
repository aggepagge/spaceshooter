using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceShooter.Model;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using SpaceShooter.Model.GameComponents;
using SpaceShooter.Model.GameComponents.Weapons.Weapon;
using SpaceShooter.Model.GameComponents.Ships;
using Microsoft.Xna.Framework.Audio;
using System.Collections;
using SpaceShooter.View.Particles;
using SpaceShooter.Model.GameComponents.Parts;

namespace SpaceShooter.View
{
    class GameView : IGameModelListener
    {
        private GameModel m_gameModel;
        private Camera camera;
        private GraphicsDevice graphDevice;
        private SpriteBatch spriteBatch;
        private ContentManager content;

        private Texture2D player_spaceShipTexture;
        private Rectangle spriteDestinationRectangle;

        //Variabler för vinkling av skeppet åt vänster och höger
        private float playerPreviousX;
        //Aktuell ruta i X-led
        private int frameX = 5;
        //Aktuell ruta i Y-led
        private int frameY = 4;
        //Antal rutor i X-led
        private static int numFramesX = 5;
        //Antal rutor i Y-led
        private static int numFramesY = 8;
        //Aktuell bildruta
        private int imageCount = 6;
        private static float IMAGE_COUNT_UPDATE = 0.05f;
        private float timeCountUpdate = 0.0f;

        private SoundEffect soundExplotion;
        private SoundEffect raygun_Sound;
        private SoundEffectInstance raygun_Sound_Instance;
        private SoundEffect plasma_Sound;
        private SoundEffectInstance plasma_Sound_Instance;

        private Texture2D textureExplotion;
        private Texture2D textureSplitter;

        private Texture2D background_TMP_TEXTURE;
        private Rectangle background_TMP_Rect;
        private Rectangle spaceShipDestinationRectangle;

        private KeyboardState currentKeyBoardState;
        private KeyboardState previousKeyBoardState;

        private Texture2D enemy_Easy_Texture;
        private Texture2D enemy_Middle_Texture;
        private Texture2D enemy_Hard_Texture;
        private Texture2D playerShot_Texture;
        private Texture2D enemyRayGun_Texture;
        private ArrayList explotions = new ArrayList();
        private ArrayList splitters = new ArrayList();
        private BackgroundSmokeSystem smokeSystem;

        private Texture2D scoreBackground;
        private Rectangle scoreRect;
        private SpriteFont theFont;
        private Texture2D healtCount;
        private Rectangle healtCountRect;
        private Rectangle healtCountRectCoverRect;
        private Texture2D healtCountForeground;
        private Texture2D smokeBackground_Texture;
        private Texture2D asteroid_Texture;
        private Texture2D powerup_Texture;
        private Texture2D weaponupgrade_Texture;
        private Texture2D plasma_Texture;
        private Texture2D enemyLaser_Texture;
        private Texture2D enemyBoss_Texture;

        public GameView(GraphicsDevice graphDevice, GameModel model, Camera camera, SpriteBatch spriteBatch, ContentManager content)
        {
            this.graphDevice = graphDevice;
            this.m_gameModel = model;
            this.camera = camera;
            this.spriteBatch = spriteBatch;
            this.content = content;
            this.playerPreviousX = m_gameModel.Player.getPossitionX();
            this.smokeSystem = new BackgroundSmokeSystem();

            loadContent();
        }

        private void loadContent()
        {
            player_spaceShipTexture = content.Load<Texture2D>("bulletship");
            enemyRayGun_Texture = content.Load<Texture2D>("EnemyRaygunLaser");
            background_TMP_TEXTURE = content.Load<Texture2D>("background2");
            enemy_Easy_Texture = content.Load<Texture2D>("alien5");
            enemy_Middle_Texture = content.Load<Texture2D>("alien8");
            enemy_Hard_Texture = content.Load<Texture2D>("alien9");
            playerShot_Texture = content.Load<Texture2D>("fireone");
            textureExplotion = content.Load<Texture2D>("explotion3");
            textureSplitter = content.Load<Texture2D>("splitterballtree");
            scoreBackground = content.Load<Texture2D>("scoreBackground");
            healtCountForeground = content.Load<Texture2D>("healtBackground");
            healtCount = content.Load<Texture2D>("healt");
            smokeBackground_Texture = content.Load<Texture2D>("smoke");
            asteroid_Texture = content.Load<Texture2D>("asteroid1");
            powerup_Texture = content.Load<Texture2D>("powerup");
            weaponupgrade_Texture = content.Load<Texture2D>("weaponupgrade");
            plasma_Texture = content.Load<Texture2D>("plasma");
            enemyLaser_Texture = content.Load<Texture2D>("splitterRed2");
            enemyBoss_Texture = content.Load<Texture2D>("boss");

            soundExplotion = content.Load<SoundEffect>("explosion_sound");
            raygun_Sound = content.Load<SoundEffect>("laserDowe");
            plasma_Sound = content.Load<SoundEffect>("laserSound");

            theFont = content.Load<SpriteFont>("Titanium Motors");

            setStatictextures();
            resetSpritesheet();
            setSound();
        }

        private void setSound()
        {
            raygun_Sound_Instance = raygun_Sound.CreateInstance();
            raygun_Sound_Instance.IsLooped = true;
            raygun_Sound_Instance.Volume = 0.2f;

            plasma_Sound_Instance = plasma_Sound.CreateInstance();
            plasma_Sound_Instance.IsLooped = true;
            plasma_Sound_Instance.Volume = 0.2f;
        }

        internal void pauseSound()
        {
            foreach (MakeExplotion explotion in explotions)
                explotion.pauseSound();

            foreach (MakeSplitter splitter in splitters)
                splitter.pauseSound();

            if (raygun_Sound_Instance.State == SoundState.Playing)
                raygun_Sound_Instance.Pause();

            if (plasma_Sound_Instance.State == SoundState.Playing)
                plasma_Sound_Instance.Pause();
        }

        internal void stopFireSound()
        {
            if (raygun_Sound_Instance.State == SoundState.Playing)
                raygun_Sound_Instance.Stop();

            if (plasma_Sound_Instance.State == SoundState.Playing)
                plasma_Sound_Instance.Stop();
        }

        internal void resumeSound()
        {
            foreach (MakeExplotion explotion in explotions)
                explotion.resumeSound();

            foreach (MakeSplitter splitter in splitters)
                splitter.resumeSound();
        }

        public void restartGame()
        {
            this.playerPreviousX = m_gameModel.Player.getPossitionX();
            explotions.Clear();
            splitters.Clear();
            camera.restartGame();
            background_TMP_TEXTURE = content.Load<Texture2D>("background2");
        }

        public void setNextLevel(int nextLevel)
        {
            this.playerPreviousX = m_gameModel.Player.getPossitionX();
            explotions.Clear();
            splitters.Clear();
            camera.restartGame();

            if (nextLevel == 2)
            {
                background_TMP_TEXTURE = content.Load<Texture2D>("background5");
            }
            else if (nextLevel == 3)
            {
                background_TMP_TEXTURE = content.Load<Texture2D>("background6");
            }
        }

        private void resetSpritesheet(int firstX = 5, int firstY = 4)
        {
            int spriteWidth = (int)(player_spaceShipTexture.Width / numFramesX);
            int spriteHeight = (int)(player_spaceShipTexture.Height / numFramesY);

            spriteDestinationRectangle = new Rectangle(
                                                            (firstX - 1) * spriteWidth,
                                                            (firstY - 1) * spriteHeight,
                                                            spriteWidth,
                                                            spriteHeight
                                                      );
        }

        private void turnSpritesheet()
        {
            float playerDirection = m_gameModel.Player.getPossitionX() - playerPreviousX;

            if (playerDirection < 0 && imageCount > 1)
            {
                imageCount--;
                setFrame();
            }
            else if (playerDirection > 0 && imageCount < 11)
            {
                imageCount++;
                setFrame();
            }
        }

        //Uträkning av vilken sprite-bild i spitesheeten som ska visas.
        //Uträkningen är lite udda, men detta beror på att det bara är vissa 
        //sprite-rutor som används (Rad 3y, 1x till 5y, 5x)
        private void setFrame()
        {
            //För visning av bilderna åt ett håll
            //if (imageCount > 6)
            //    frameY = 5;
            //else if (imageCount < 2)
            //    frameY = 3;
            //else
            //    frameY = 4;

            //if (frameY == 4)
            //    frameX = imageCount - 1;
            //else if (frameY == 5)
            //    frameX = imageCount - 6;
            //else
            //    frameX = 5;

            //För visning av bilderna åt motsatt håll
            if (imageCount < 6)
                frameY = 5;
            else if (imageCount > 10)
                frameY = 3;
            else
                frameY = 4;

            if (frameY == 4)
                frameX = 11 - imageCount;
            else if (frameY == 5)
                frameX = 6 - imageCount;
            else
                frameX = 5;

            //Skapar den inre rektangeln där spriten ritas ut
            this.resetSpritesheet(frameX, frameY);
            timeCountUpdate = 0.0f;
        }

        internal void UpdateView(float elapsedGameTime)
        {
            if (m_gameModel.Player.Firering)
            {
                if (m_gameModel.Player.CurrentWeapon == WeaponTypes.Raygun)
                {
                    if (plasma_Sound_Instance.State != SoundState.Stopped)
                        plasma_Sound_Instance.Stop();

                    if (raygun_Sound_Instance.State == SoundState.Stopped)
                        raygun_Sound_Instance.Play();
                    else if(raygun_Sound_Instance.State == SoundState.Paused)
                        raygun_Sound_Instance.Resume();
                }
                else if (m_gameModel.Player.CurrentWeapon == WeaponTypes.Plasma)
                {
                    if (raygun_Sound_Instance.State != SoundState.Stopped)
                        raygun_Sound_Instance.Stop();

                    if (plasma_Sound_Instance.State == SoundState.Stopped)
                        plasma_Sound_Instance.Play();
                    else if (plasma_Sound_Instance.State == SoundState.Paused)
                        plasma_Sound_Instance.Resume();
                }
            }
            else
            {
                raygun_Sound_Instance.Stop();
                raygun_Sound_Instance.Stop();
            }

            smokeSystem.Update(elapsedGameTime);

            foreach (MakeExplotion explotion in explotions)
                explotion.UpdateExplotion(elapsedGameTime);

            foreach (MakeSplitter splitter in splitters)
                splitter.UpdateSplitter(elapsedGameTime);

            timeCountUpdate += elapsedGameTime;

            if (timeCountUpdate > IMAGE_COUNT_UPDATE)
            {
                turnSpritesheet();
            }

            if (timeCountUpdate > IMAGE_COUNT_UPDATE / 60) //Delat med 60 så uppdateringen av possitionen hinns med
            {
                if (playerPreviousX != m_gameModel.Player.getPossitionX())
                {
                    playerPreviousX = m_gameModel.Player.getPossitionX();
                }
                else
                {
                    if (imageCount > 6)
                    {
                        imageCount--;
                        setFrame();
                    }
                    else if (imageCount < 6)
                    {
                        imageCount++;
                        setFrame();
                    }

                    timeCountUpdate = 0.0f;
                }
            }

            spaceShipDestinationRectangle = camera.getPlayerVisualRectangle(
                                                                                m_gameModel.Player.getPossitionX() + (m_gameModel.Player.SpaceShipWidth / 4),
                                                                                m_gameModel.Player.getPossitionY() + (m_gameModel.Player.SpaceShipHeight / 4),
                                                                                m_gameModel.Player.SpaceShipHeight,
                                                                                m_gameModel.Player.SpaceShipWidth
                                                                           );

            background_TMP_Rect = camera.getBoardVisualRectangle(m_gameModel.Level.BoardTotalWidth);

            //Uträkning av hur många procent av hälsostapeln som ska räknas bort
            int healtLost = m_gameModel.Player.PlayerStartHealt - m_gameModel.Player.Healt;
            float heltPercent = (float)healtLost / (float)m_gameModel.Player.PlayerStartHealt;
            int result = 200;

            if (heltPercent != 0)
                result = 200 - (int)(200 * heltPercent);

            healtCountRectCoverRect = new Rectangle(580, 10, result, 30);

            //Sätter nuvarande och tidigare tangentbordstillstånd
            previousKeyBoardState = currentKeyBoardState;
            currentKeyBoardState = Keyboard.GetState();
        }

        internal bool showMenu()
        {
            return Keyboard.GetState().IsKeyDown(Keys.Escape);
        }

        internal bool playerMovesUp()
        {
            return Keyboard.GetState().IsKeyDown(Keys.Up);
        }

        internal bool playerMovesDown()
        {
            return Keyboard.GetState().IsKeyDown(Keys.Down);
        }

        internal bool playerMovesLeft()
        {
            return Keyboard.GetState().IsKeyDown(Keys.Left);
        }

        internal bool playerMovesRight()
        {
            return Keyboard.GetState().IsKeyDown(Keys.Right);
        }

        internal bool playerShoots()
        {
            if (currentKeyBoardState.IsKeyDown(Keys.Space) && !previousKeyBoardState.IsKeyDown(Keys.Space))
                return true;

            return false;
        }

        public void wounded(Vector2 possition)
        {
            splitters.Add(new MakeSplitter(possition, camera.GetScale(), soundExplotion.CreateInstance(), textureSplitter));
        }

        public void wounded(KeyValuePair<Vector2, Vector2> currentAndPreviousPossition)
        {
            splitters.Add(new MakeSplitter(currentAndPreviousPossition.Key, camera.GetScale(), soundExplotion.CreateInstance(), textureSplitter));
        }

        public void killed(KeyValuePair<Vector2, Vector2> currentAndPreviousPossition)
        {
            explotions.Add(new MakeExplotion(currentAndPreviousPossition, camera.GetScale(), soundExplotion.CreateInstance(), textureExplotion));
        }

        public void killed(Vector2 possition)
        {
            explotions.Add(new MakeExplotion(possition, camera.GetScale(), soundExplotion.CreateInstance(), textureExplotion));
        }

        private void setStatictextures()
        {
            scoreRect = new Rectangle(20, 10, 200, 30);
            healtCountRect = new Rectangle(580, 10, 200, 30);
        }

        internal void Draw(float elapsedGameTime)
        {
            graphDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            spriteBatch.Draw(background_TMP_TEXTURE, background_TMP_Rect, Color.White);

            smokeSystem.Draw(spriteBatch, camera, smokeBackground_Texture);

            if (m_gameModel.levelContent.CreateBoss && m_gameModel.TheBoss != null && !m_gameModel.TheBoss.RemoveMe)
            {
                Rectangle frameRect = camera.getVisualRectangle(
                                                                    m_gameModel.TheBoss.getPossitionX() + (m_gameModel.TheBoss.SpaceShipWidth / 2),
                                                                    m_gameModel.TheBoss.getPossitionY() + (m_gameModel.TheBoss.SpaceShipHeight / 8),
                                                                    m_gameModel.TheBoss.SpaceShipWidth,
                                                                    m_gameModel.TheBoss.SpaceShipHeight
                                                                );

                int Xrow = 0;
                int spriteWidth = (int)(enemyBoss_Texture.Width / m_gameModel.TheBoss.NumberOfFramesX);

                if (m_gameModel.TheBoss.Boss_CountFrame == 1)
                    Xrow = 0;
                else if (m_gameModel.TheBoss.Boss_CountFrame > 1)
                    Xrow = (m_gameModel.TheBoss.Boss_CountFrame - 1) * spriteWidth;

                Vector2 printVector = new Vector2((enemyBoss_Texture.Width / m_gameModel.TheBoss.NumberOfFramesX) / 2,
                                                   enemyBoss_Texture.Height / 2);

                Rectangle obsticleRect = new Rectangle(Xrow, 0, spriteWidth, enemyBoss_Texture.Height);

                spriteBatch.Draw(
                                    enemyBoss_Texture,
                                    frameRect,
                                    obsticleRect,
                                    Color.White,
                                    0,
                                    printVector,
                                    SpriteEffects.None,
                                    0
                                );
            }

            foreach (Weapon shot in m_gameModel.Shoots)
            {
                Rectangle shotRectangle = camera.getVisualRectangle(
                                                                        shot.PossitionX,
                                                                        shot.PossitionY,
                                                                        shot.Width,
                                                                        shot.Height
                                                                     );

                if (shot.WeaponType == WeaponTypes.Raygun)
                    spriteBatch.Draw(playerShot_Texture, shotRectangle, Color.White);
                else if (shot.WeaponType == WeaponTypes.Plasma)
                    spriteBatch.Draw(plasma_Texture, shotRectangle, Color.White);
                else if (shot.WeaponType == WeaponTypes.EnemyRaygun)
                    spriteBatch.Draw(enemyRayGun_Texture, shotRectangle, Color.White);
                else if (shot.WeaponType == WeaponTypes.EnemyLaser)
                    spriteBatch.Draw(enemyLaser_Texture, shotRectangle, Color.White);
                else if (shot.WeaponType == WeaponTypes.EnemyPlasma)
                    spriteBatch.Draw(enemyRayGun_Texture, shotRectangle, Color.White);
                else if (shot.WeaponType == WeaponTypes.EnemyBossPlasma)
                    spriteBatch.Draw(enemyRayGun_Texture, shotRectangle, Color.White);
            }

            foreach (EnemySpaceShip enemy in m_gameModel.EnemyShips)
            {
                Rectangle enemyRectangle = camera.getVisualRectangle(
                                                                            enemy.getPossitionX(),
                                                                            enemy.getPossitionY(),
                                                                            enemy.SpaceShipHeight,
                                                                            enemy.SpaceShipWidth
                                                                         );

                if (enemy.EnemyType == EnemyTypes.Easy)
                    spriteBatch.Draw(enemy_Easy_Texture, enemyRectangle, Color.White);
                else if (enemy.EnemyType == EnemyTypes.Middle)
                    spriteBatch.Draw(enemy_Middle_Texture, enemyRectangle, Color.White);
                else if (enemy.EnemyType == EnemyTypes.Hard)
                    spriteBatch.Draw(enemy_Hard_Texture, enemyRectangle, Color.White);
            }

            if (!m_gameModel.Player.RemoveMe)
            {
                spriteBatch.Draw(
                                    player_spaceShipTexture,
                                    spaceShipDestinationRectangle,
                                    spriteDestinationRectangle,
                                    Color.White,
                                    0,
                                    new Vector2((player_spaceShipTexture.Width / numFramesX) / 2,
                                                (player_spaceShipTexture.Height / numFramesY)) / 2,
                                    SpriteEffects.None,
                                    0
                                );
            }

            foreach (GameObsticle obsticle in m_gameModel.Obsticles)
            {
                //Yttre rektangel med startpossition (X och Y) samt logisk storlek.
                Rectangle frameRect = camera.getVisualRectangle(obsticle.getPossitionX(), obsticle.getPossitionY(), obsticle.Size, obsticle.Size);

                int Xrow = 0;
                int Yrow = 0;

                int spriteWidth = (int)(asteroid_Texture.Width / obsticle.NumFramesX);
                int spriteHeight = (int)(asteroid_Texture.Height / obsticle.NumFramesY);

                //Sätter rätt bredd och höjd i pixlar för aktuell ruta
                if (obsticle.FrameY == 1)
                    Yrow = 0;
                else if (obsticle.FrameY > 1)
                    Yrow = (obsticle.FrameY - 1) * spriteHeight;

                if (obsticle.FrameX == 1)
                    Xrow = 0;
                else if (obsticle.FrameX > 1)
                    Xrow = (obsticle.FrameX - 1) * spriteWidth;

                if (obsticle.FrameY == 4 && obsticle.FrameX > 3)
                    obsticle.ImageCount = 1;

                Vector2 printVector = new Vector2((asteroid_Texture.Width / obsticle.NumFramesX) / 2, (asteroid_Texture.Height / obsticle.NumFramesY) / 2);

                Rectangle obsticleRect = new Rectangle(Xrow, Yrow, spriteWidth, spriteHeight);

                spriteBatch.Draw(
                                    asteroid_Texture,
                                    frameRect,
                                    obsticleRect,
                                    Color.White,
                                    obsticle.Rotation,
                                    printVector,
                                    SpriteEffects.None,
                                    0
                                );
            }

            foreach (MakeExplotion explotion in explotions)
                explotion.DrawExplotion(spriteBatch, camera);

            foreach (MakeSplitter splitter in splitters)
                splitter.DrawSplitter(spriteBatch, camera);

            foreach (PowerUp powerup in m_gameModel.Power)
            {
                Rectangle frameRect = camera.getVisualRectangle(powerup.getPossitionX(), powerup.getPossitionY(), powerup.Size, powerup.Size);
                Rectangle powerRect = new Rectangle(0, 0, (int)(powerup.Size * (float)camera.GetScale()), (int)(powerup.Size * (float)camera.GetScale()));

                Texture2D tmpTexture;
                if (powerup.Type == PowerUpType.Health)
                    tmpTexture = powerup_Texture;
                else
                    tmpTexture = weaponupgrade_Texture;

                spriteBatch.Draw(
                                    tmpTexture,
                                    frameRect,
                                    null,
                                    Color.White,
                                    powerup.Rotation,
                                    new Vector2(powerup_Texture.Width / 2,
                                                powerup_Texture.Height / 2),
                                    SpriteEffects.None,
                                    0
                                );
            }

            spriteBatch.Draw(scoreBackground, scoreRect, Color.White);

            string scoreText = "Score: " + m_gameModel.Player.PlayerScoore;
            Vector2 possitionScoreText = new Vector2(44, 12);
            spriteBatch.DrawString(theFont, scoreText, possitionScoreText, Color.White);
            
            spriteBatch.Draw(healtCountForeground, healtCountRect, Color.White);
            spriteBatch.Draw(healtCount, healtCountRectCoverRect, Color.White);

            spriteBatch.End();
        }
    }
}
