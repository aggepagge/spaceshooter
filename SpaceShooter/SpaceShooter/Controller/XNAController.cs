using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SpaceShooter.Model;
using SpaceShooter.View;

namespace SpaceShooter
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class XNAController : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private GameModel m_gameModel;
        private GameView v_gameView;
        private Camera camera;

        private MenuGUI v_GUI;
        private Rectangle backgroundRect;
        private Texture2D background;
 
        //Statiska variabler för logisk höjd och bredd på panelen
        internal static float BOARD_LOGIC_WIDTH = 1.0f;
        internal static float BOARD_LOGIC_HEIGHT = 1.0f;

        //TODO: ANVÄNDS FÖR BREDDEN PÅ SPELPLANEN... 
        internal static float BOARD_LOGIC_GAMEFIELDWIDTH = 1.6f;
        internal static int PLAYER_START_HEALT = 100;

        //Statiska variabler för logisk höjd och bredd på spelarens rymdskepp
        internal static float PLAYER_SPACESHIP_HEIGHT = 0.1f;
        internal static float PLAYER_SPACESHIP_WIDTH = 0.1f;

        internal static float PLAYER_SPACESHIP_SPEEDX = 0.014f;
        internal static float PLAYER_SPACESHIP_SPEEDY = 0.01f;

        //Konstanter för fönster-bredd och höjd
        private const int SCREEN_HEIGHT = 800;
        private const int SCREEN_WIDTH = 800;

        private bool paused = false;
        internal bool ShowingMenu { get; set; }
        private bool showIngameMenu = false;
        private float pullingForMenu = 0.0f;
        private float WAIT_TIME = 0.01f;

        public XNAController()
        {
            graphics = new GraphicsDeviceManager(this);
            //Sätter storlek på fönstret
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            Content.RootDirectory = "Content";
            ShowingMenu = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            m_gameModel = new GameModel();
            camera = new Camera(graphics.GraphicsDevice.Viewport);
            v_gameView = new GameView(graphics.GraphicsDevice, m_gameModel, camera, spriteBatch, Content);
            v_GUI = new MenuGUI(graphics.GraphicsDevice, Content, spriteBatch);

            background = Content.Load<Texture2D>("backgroundImage");
            backgroundRect = new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (m_gameModel.Player.RemoveMe || m_gameModel.LevelFinished)
                showIngameMenu = true;

            pullingForMenu += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (pullingForMenu > WAIT_TIME)
            {
                if (v_gameView.showMenu())
                {
                    ShowingMenu = !ShowingMenu;

                    if (ShowingMenu)
                        v_gameView.pauseSound();
                    else
                        v_gameView.resumeSound();
                }

                pullingForMenu = 0.0f;
            }

            if (!ShowingMenu && !showIngameMenu)
            {
                this.IsMouseVisible = false;

                if (!paused)
                {
                    if (v_gameView.playerMovesUp())
                        m_gameModel.playerMovesUp();

                    if (v_gameView.playerMovesDown())
                        m_gameModel.playerMovesDown();

                    if (v_gameView.playerMovesLeft())
                        m_gameModel.playerMovesLeft();

                    if (v_gameView.playerMovesRight())
                        m_gameModel.playerMovesRight();

                    if (v_gameView.playerShoots())
                        m_gameModel.playerShoots();

                    m_gameModel.UpdateModel((float)gameTime.ElapsedGameTime.TotalSeconds, v_gameView);
                    v_gameView.UpdateView((float)gameTime.ElapsedGameTime.TotalSeconds);
                }
            }
            else
                this.IsMouseVisible = true;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (ShowingMenu || showIngameMenu)
            {
                int possitionX = 100;
                int possitionY = 100;
                int buttonSeparation = 60;

                GraphicsDevice.Clear(Color.White);
                spriteBatch.Begin();
                spriteBatch.Draw(background, backgroundRect, Color.White);

                if (showIngameMenu)
                {
                    if (m_gameModel.Player.RemoveMe)
                    {
                        v_GUI.DrawTitle("GAME OVER", 2.0f, possitionX, possitionY);
                        if (v_GUI.DrawMenu(Mouse.GetState(), "Start New Game", possitionX, possitionY += buttonSeparation * 2))
                        {
                            m_gameModel.startNewGame(v_gameView);
                            showIngameMenu = false;
                        }

                        if (v_GUI.DrawMenu(Mouse.GetState(), "Quit", possitionX, possitionY += buttonSeparation))
                        {
                            this.Exit();
                        }
                    }
                    else if (m_gameModel.LevelCount < 4)
                    {
                        v_GUI.DrawTitle("NEXT LEVEL", 2.0f, possitionX, possitionY);
                        v_GUI.DrawResult("Your score is " + m_gameModel.Player.PlayerScoore + " points", possitionX, possitionY += buttonSeparation * 2);
                        if (v_GUI.DrawMenu(Mouse.GetState(), "Play Next Level", possitionX, possitionY += buttonSeparation))
                        {
                            m_gameModel.playNextLevel(v_gameView);
                            showIngameMenu = false;
                        }

                        if (v_GUI.DrawMenu(Mouse.GetState(), "Quit", possitionX, possitionY += buttonSeparation))
                        {
                            this.Exit();
                        }
                    }
                    else if (m_gameModel.LevelCount > 3)
                    {
                        v_GUI.DrawTitle("YOU MADE IT!", 2.0f, possitionX, possitionY);
                        v_GUI.DrawResult("Your score is " + m_gameModel.Player.PlayerScoore + " points", possitionX, possitionY += buttonSeparation * 2);

                        if (v_GUI.DrawMenu(Mouse.GetState(), "Play Again", possitionX, possitionY += buttonSeparation))
                        {
                            m_gameModel.startNewGame(v_gameView);
                            showIngameMenu = false;
                        }

                        if (v_GUI.DrawMenu(Mouse.GetState(), "Quit", possitionX, possitionY += buttonSeparation))
                        {
                            this.Exit();
                        }
                    }
                }
                else
                {
                    if (v_GUI.DrawMenu(Mouse.GetState(), "Start New Game", possitionX, possitionY))
                    {
                        m_gameModel.startNewGame(v_gameView);
                        ShowingMenu = false;
                    }

                    if (m_gameModel.GameTime > 0)
                    {
                        if (v_GUI.DrawMenu(Mouse.GetState(), "Continue", possitionX, possitionY += buttonSeparation))
                        {
                            ShowingMenu = false;
                        }
                    }

                    if (v_GUI.DrawMenu(Mouse.GetState(), "Quit", possitionX, possitionY += buttonSeparation))
                    {
                        this.Exit();
                    }
                }

                spriteBatch.End();

                v_GUI.setOldState(Mouse.GetState());
            }
            else
                v_gameView.Draw((float)gameTime.ElapsedGameTime.TotalSeconds);

            base.Draw(gameTime);
        }
    }
}
