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
        private PlayerSpaceShip player_spaceShip;

        private SoundEffect soundExplotion;
        private Texture2D textureExplotion;
        private Texture2D textureSplitter;

        //TODO: ÄNDRA BAKGRUND SEN...
        private Texture2D background_TMP_TEXTURE;
        private Rectangle background_TMP_Rect;
        private Rectangle spaceShipDestinationRectangle;

        private KeyboardState currentKeyBoardState;
        private KeyboardState previousKeyBoardState;

        private Texture2D enemy_Texture;
        private List<EnemySpaceShip> enemys;
        private Texture2D shot_Texture;
        private List<Weapon> shoots;
        private ArrayList explotions = new ArrayList();
        private ArrayList splitters = new ArrayList();

        public GameView(GraphicsDevice graphDevice, GameModel model, Camera camera, SpriteBatch spriteBatch, ContentManager content)
        {
            this.graphDevice = graphDevice;
            this.m_gameModel = model;
            this.camera = camera;
            this.spriteBatch = spriteBatch;
            this.content = content;
            this.player_spaceShip = model.Player;

            this.enemys = m_gameModel.EnemyShips;
            this.shoots = m_gameModel.Shoots;

            loadContent();
        }

        private void loadContent()
        {
            player_spaceShipTexture = content.Load<Texture2D>("spaceshiptwo");
            background_TMP_TEXTURE = content.Load<Texture2D>("background");
            enemy_Texture = content.Load<Texture2D>("shipEnemy");
            shot_Texture = content.Load<Texture2D>("fireone");
            soundExplotion = content.Load<SoundEffect>("explosion_sound");
            textureExplotion = content.Load<Texture2D>("explotion3");
            textureSplitter = content.Load<Texture2D>("splitterballtree");
        }

        internal bool playerWantsToQuit()
        {
            return Keyboard.GetState().IsKeyDown(Keys.Escape);
        }

        internal void UpdateView(float elapsedGameTime)
        {
            foreach (MakeExplotion explotion in explotions)
                explotion.UpdateExplotion(elapsedGameTime);

            foreach (MakeSplitter splitter in splitters)
                splitter.UpdateSplitter(elapsedGameTime);

            spaceShipDestinationRectangle = camera.getVisualRectangle(
                                                                        player_spaceShip.getPossitionX(),
                                                                        player_spaceShip.getPossitionY(),
                                                                        player_spaceShip.SpaceShipHeight,
                                                                        player_spaceShip.SpaceShipWidth
                                                                     );

            //TODO: RÄKNA UT VART BAKGRUNDEN SKA VARA BASSERAT PÅ VART RYMDSKEPPET BEFINNER SIG...
            background_TMP_Rect = camera.getVisualBackgroundRectangle(spaceShipDestinationRectangle, player_spaceShip);

            previousKeyBoardState = currentKeyBoardState;
            currentKeyBoardState = Keyboard.GetState();
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

        internal void Draw(float elapsedGameTime)
        {
            graphDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            spriteBatch.Draw(background_TMP_TEXTURE, background_TMP_Rect, Color.White);

            spriteBatch.Draw(player_spaceShipTexture, spaceShipDestinationRectangle, Color.White);

            foreach (EnemySpaceShip enemy in enemys)
            {
                Rectangle enemyRectangle = camera.getVisualRectangle(
                                                                            enemy.getPossitionX(),
                                                                            enemy.getPossitionY(),
                                                                            enemy.SpaceShipHeight,
                                                                            enemy.SpaceShipWidth
                                                                         );

                spriteBatch.Draw(enemy_Texture, enemyRectangle, Color.White);
            }

            foreach (Weapon shot in shoots)
            {
                Rectangle shotRectangle = camera.getVisualRectangle(
                                                                        shot.Possition.X,
                                                                        shot.Possition.Y,
                                                                        shot.Width,
                                                                        shot.Height
                                                                     );

                spriteBatch.Draw(shot_Texture, shotRectangle, Color.White);
            }

            foreach (MakeExplotion explotion in explotions)
                explotion.DrawExplotion(spriteBatch, camera);

            foreach (MakeSplitter splitter in splitters)
                splitter.DrawSplitter(spriteBatch, camera);

            spriteBatch.End();
        }
    }
}
