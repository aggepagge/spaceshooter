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
        private Texture2D textureExplotion;
        private Texture2D textureSplitter;

        private Texture2D background_TMP_TEXTURE;
        private Rectangle background_TMP_Rect;
        private Rectangle spaceShipDestinationRectangle;

        private KeyboardState currentKeyBoardState;
        private KeyboardState previousKeyBoardState;

        private Texture2D enemy_Texture;
        private Texture2D playerShot_Texture;
        private Texture2D enemyRayGun_Texture;
        private ArrayList explotions = new ArrayList();
        private ArrayList splitters = new ArrayList();

        private Texture2D scoreBackground;
        private Rectangle scoreRect;
        private SpriteFont theFont;
        private Texture2D healtCount;
        private Rectangle healtCountRect;
        private Rectangle healtCountRectCoverRect;
        private Texture2D healtCountForeground;

        public GameView(GraphicsDevice graphDevice, GameModel model, Camera camera, SpriteBatch spriteBatch, ContentManager content)
        {
            this.graphDevice = graphDevice;
            this.m_gameModel = model;
            this.camera = camera;
            this.spriteBatch = spriteBatch;
            this.content = content;
            this.playerPreviousX = m_gameModel.Player.getPossitionX();

            loadContent();
        }

        private void loadContent()
        {
            player_spaceShipTexture = content.Load<Texture2D>("bulletship");
            enemyRayGun_Texture = content.Load<Texture2D>("EnemyRaygunLaser");
            background_TMP_TEXTURE = content.Load<Texture2D>("background2");
            enemy_Texture = content.Load<Texture2D>("alien5");
            playerShot_Texture = content.Load<Texture2D>("fireone");
            soundExplotion = content.Load<SoundEffect>("explosion_sound");
            textureExplotion = content.Load<Texture2D>("explotion3");
            textureSplitter = content.Load<Texture2D>("splitterballtree");
            scoreBackground = content.Load<Texture2D>("scoreBackground");
            healtCountForeground = content.Load<Texture2D>("healtBackground");
            healtCount = content.Load<Texture2D>("healt");

            theFont = content.Load<SpriteFont>("Titanium Motors");

            setStatictextures();
            resetSpritesheet();
        }

        internal void pauseSound()
        {
            foreach (MakeExplotion explotion in explotions)
                explotion.pauseSound();
        }

        internal void resumeSound()
        {
            foreach (MakeExplotion explotion in explotions)
                explotion.resumeSound();
        }

        public void restartGame()
        {
            this.playerPreviousX = m_gameModel.Player.getPossitionX();
            camera.restartGame();
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
            foreach (MakeExplotion explotion in explotions)
                explotion.UpdateExplotion(elapsedGameTime);

            foreach (MakeSplitter splitter in splitters)
                splitter.UpdateSplitter(elapsedGameTime);

            timeCountUpdate += elapsedGameTime;

            if (timeCountUpdate > IMAGE_COUNT_UPDATE)
            {
                turnSpritesheet();
            }

            if (timeCountUpdate > IMAGE_COUNT_UPDATE / 30) //Delat med 30 så uppdateringen av possitionen hinns med
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

            foreach (EnemySpaceShip enemy in m_gameModel.EnemyShips)
            {
                Rectangle enemyRectangle = camera.getVisualRectangle(
                                                                            enemy.getPossitionX(),
                                                                            enemy.getPossitionY(),
                                                                            enemy.SpaceShipHeight,
                                                                            enemy.SpaceShipWidth
                                                                         );

                spriteBatch.Draw(enemy_Texture, enemyRectangle, Color.White);
            }

            foreach (Weapon shot in m_gameModel.Shoots)
            {
                Rectangle shotRectangle = camera.getVisualRectangle(
                                                                        shot.Possition.X,
                                                                        shot.Possition.Y,
                                                                        shot.Width,
                                                                        shot.Height
                                                                     );

                if(shot.WeaponType == WeaponTypes.Raygun)
                    spriteBatch.Draw(playerShot_Texture, shotRectangle, Color.White);
                else if(shot.WeaponType ==WeaponTypes.EnemyRaygun)
                    spriteBatch.Draw(enemyRayGun_Texture, shotRectangle, Color.White);
            }

            foreach (MakeExplotion explotion in explotions)
                explotion.DrawExplotion(spriteBatch, camera);

            foreach (MakeSplitter splitter in splitters)
                splitter.DrawSplitter(spriteBatch, camera);

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
