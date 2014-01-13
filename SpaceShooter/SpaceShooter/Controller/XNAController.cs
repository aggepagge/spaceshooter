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
 
        //Statiska variabler för logisk höjd och bredd på camera-vyn
        internal static float BOARD_LOGIC_WIDTH = 1.0f;
        internal static float BOARD_LOGIC_HEIGHT = 1.0f;

        //Statisk variablel för logisk bredd på spelplanen
        internal static float BOARD_LOGIC_GAMEFIELDWIDTH = 1.6f;
        //Variabel för spelarens starthälsa
        internal static int PLAYER_START_HEALT = 100;

        //Statiska variabler för logisk höjd och bredd på spelarens rymdskepp
        internal static float PLAYER_SPACESHIP_HEIGHT = 0.1f;
        internal static float PLAYER_SPACESHIP_WIDTH = 0.1f;

        //Variabler för spelarens skepp i X och Y-led
        internal static float PLAYER_SPACESHIP_SPEEDX = 0.014f;
        internal static float PLAYER_SPACESHIP_SPEEDY = 0.01f;

        //Konstanter för fönster-bredd och höjd
        private const int SCREEN_HEIGHT = 800;
        private const int SCREEN_WIDTH = 800;

        //Variabler för om menyn ska visas eller ej
        private bool paused = false;
        internal bool ShowingMenu { get; set; }
        private bool showIngameMenu = false;
        private float pullingForMenu = 0.0f;
        private float WAIT_TIME = 2.0f;

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
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            //initsierar alla model och view-objekt
            m_gameModel = new GameModel();
            camera = new Camera(graphics.GraphicsDevice.Viewport);
            v_gameView = new GameView(graphics.GraphicsDevice, m_gameModel, camera, spriteBatch, Content);
            v_GUI = new MenuGUI(graphics.GraphicsDevice, Content, spriteBatch, camera);
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
            //Om användaren har tryckt på ESC så returnerar gameview true
            if (v_gameView.showMenu())
            {
                //ShowingMenu sätts till true (Genom att sätta den till sitt motsatta värde)
                ShowingMenu = !ShowingMenu;

                //Allt ljud pausas
                if (ShowingMenu)
                {
                    v_gameView.pauseSound();
                    v_gameView.stopFireSound();
                }
                else
                    v_gameView.resumeSound();
            }

            //Uppräkning för när det ska kollas om spelaren har dött eller banan är färdigspelas
            pullingForMenu += (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Fördröjning för att menyn inte ska visas direkt när något av vilkoren är sanna.
            //(Det görs även en fördröjning i GameModel)
            if (pullingForMenu > WAIT_TIME)
            {
                //Om spelaren dött eller banan har spelats klart så ska menyn visas
                if (m_gameModel.Player.RemoveMe || m_gameModel.LevelFinished)
                {
                    showIngameMenu = true;
                    v_gameView.stopFireSound();
                }

                pullingForMenu = 0.0f;
            }

            //Om inga menyer visas så spelas spelet...
            if (!ShowingMenu && !showIngameMenu)
            {
                this.IsMouseVisible = false;

                //Lystnar efter om användaren trycker på någon tangent
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

        //Draw används för att rita ut knappar i MenuGUI (Taget efter ett av dina exempel)
        private void DrawMenu()
        {
            int possitionX = 100;
            int possitionY = 100;
            int buttonSeparation = 60;

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
                        this.Exit();
                }
                else if (!m_gameModel.gameIsFinished())
                {
                    v_GUI.DrawTitle("NEXT LEVEL", 2.0f, possitionX, possitionY);
                    v_GUI.DrawResult("Your score is " + m_gameModel.Player.PlayerScoore + " points", possitionX, possitionY += buttonSeparation * 2);

                    int addLevel = m_gameModel.LevelCount + 1;

                    if (v_GUI.DrawMenu(Mouse.GetState(), "Play Level " + addLevel, possitionX, possitionY += buttonSeparation))
                    {
                        m_gameModel.playNextLevel(v_gameView);
                        showIngameMenu = false;
                    }

                    if (v_GUI.DrawMenu(Mouse.GetState(), "Quit", possitionX, possitionY += buttonSeparation))
                        this.Exit();
                }
                else if (m_gameModel.gameIsFinished())
                {
                    v_GUI.DrawTitle("YOU MADE IT!", 2.0f, possitionX, possitionY);
                    v_GUI.DrawResult("Your score is " + m_gameModel.Player.PlayerScoore + " points", possitionX, possitionY += buttonSeparation * 2);

                    if (v_GUI.DrawMenu(Mouse.GetState(), "Play Again", possitionX, possitionY += buttonSeparation))
                    {
                        m_gameModel.startNewGame(v_gameView);
                        showIngameMenu = false;
                    }

                    if (v_GUI.DrawMenu(Mouse.GetState(), "Quit", possitionX, possitionY += buttonSeparation))
                        this.Exit();
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
                    if (v_GUI.DrawMenu(Mouse.GetState(), "Continue", possitionX, possitionY += buttonSeparation))
                        ShowingMenu = false;

                if (v_GUI.DrawMenu(Mouse.GetState(), "Quit", possitionX, possitionY += buttonSeparation))
                    this.Exit();

                if (m_gameModel.GameTime == 0)
                    v_GUI.DrawResult(
                                        "Use arrow keys to stear and \nspace to shoot (Once to start \nfirering, again to stop) \n\nEsc for main menu",
                                        possitionX,
                                        possitionY += buttonSeparation,
                                        100
                                    );
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //Kollar om menyn ska visas, annars så ritas spelet ut
            if (ShowingMenu || showIngameMenu)
            {
                GraphicsDevice.Clear(Color.White);
                spriteBatch.Begin();

                v_GUI.DrawBackground();
                this.DrawMenu();

                spriteBatch.End();

                v_GUI.setOldState(Mouse.GetState());
            }
            else
                v_gameView.Draw((float)gameTime.ElapsedGameTime.TotalSeconds);

            base.Draw(gameTime);
        }
    }
}
