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
 
        //Statiska variabler för logisk höjd och bredd på panelen
        internal static float BOARD_LOGIC_WIDTH = 1.0f;
        internal static float BOARD_LOGIC_HEIGHT = 1.0f;

        //TODO: ANVÄNDS FÖR BREDDEN PÅ SPELPLANEN... 
        internal static float BOARD_LOGIC_GAMEFIELDWIDTH = 1.6f;
        internal static int PLAYER_START_HEALT = 100;

        //Statiska variabler för logisk höjd och bredd på spelarens rymdskepp
        internal static float PLAYER_SPACESHIP_HEIGHT = 0.05f;
        internal static float PLAYER_SPACESHIP_WIDTH = 0.05f;

        internal static float PLAYER_SPACESHIP_SPEEDX = 0.02f;
        internal static float PLAYER_SPACESHIP_SPEEDY = 0.03f;

        //Konstanter för fönster-bredd och höjd
        private int screenHeight = 800;
        private int screenWidth = 800;

        public XNAController()
        {
            graphics = new GraphicsDeviceManager(this);
            //Sätter storlek på fönstret
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.PreferredBackBufferWidth = screenWidth;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            m_gameModel = new GameModel();

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

            camera = new Camera(graphics.GraphicsDevice.Viewport);
            v_gameView = new GameView(graphics.GraphicsDevice, m_gameModel, camera, spriteBatch, Content);
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
            if (v_gameView.playerWantsToQuit())
                this.Exit();

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

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            v_gameView.Draw((float)gameTime.ElapsedGameTime.TotalSeconds);

            base.Draw(gameTime);
        }
    }
}
