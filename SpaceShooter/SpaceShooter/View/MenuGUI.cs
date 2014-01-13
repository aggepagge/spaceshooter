using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace SpaceShooter.View
{
    /// <summary>
    /// Klass som ritar ut menyknappar och menytext
    /// </summary>
    class MenuGUI
    {
        private SpriteBatch sprite;
        private SpriteFont theFont;
        private MouseState oldMouseState;
        private GraphicsDevice graphics;
        private ContentManager content;
        private Camera camera;
        private Rectangle backgroundRect;
        private Texture2D background;

        internal MenuGUI(GraphicsDevice graphics, ContentManager content, SpriteBatch sprite, Camera camera)
        {
            this.graphics = graphics;
            this.sprite = sprite;
            this.content = content;
            this.camera = camera;

            this.LoadContent();
        }

        //Laddar in font och bakgrundsbild
        private void LoadContent()
        {
            theFont = content.Load<SpriteFont>("Titanium Motors");
            background = content.Load<Texture2D>("backgroundImage");
            backgroundRect = new Rectangle(
                                                0, 
                                                0, 
                                                (int)(XNAController.BOARD_LOGIC_WIDTH * camera.GetScale()),
                                                (int)(XNAController.BOARD_LOGIC_HEIGHT * camera.GetScale())
                                           );
        }

        //Metod som skriver ut en titel
        internal void DrawTitle(string title, float size, int leftPossX, int topPossY)
        {
            Vector2 position = new Vector2(leftPossX, topPossY);
            sprite.DrawString(theFont, title, position, Color.White, 0, Vector2.Zero, size, SpriteEffects.None, 0.5f);
        }

        //Metod som ritar ut bakgrundsbilden
        internal void DrawBackground()
        {
            sprite.Draw(background, backgroundRect, Color.White);
        }

        //Metod som ritar ut ressultat av en spelomgång
        internal void DrawResult(string text, int leftPossX, int topPossY, int extraHeight = 0)
        {
            int margin = 10; 
            int buttonWidth = 400;
            int buttonHeight = 40 + extraHeight;

            Rectangle destinationRectangle = new Rectangle(leftPossX - margin, topPossY - margin, buttonWidth, buttonHeight);

            //Skapar en fejkad texture
            Texture2D rect = new Texture2D(graphics, buttonWidth, buttonHeight);
            Color[] data = new Color[buttonWidth * buttonHeight];
            for (int i = 0; i < data.Length; ++i)
                data[i] = Color.White;

            rect.SetData(data);

            Vector2 position = new Vector2(leftPossX, topPossY);

            sprite.Draw(rect, destinationRectangle, Color.LightGray);
            sprite.DrawString(theFont, text, position, Color.Black);
        }

        //Metod som ritar ut knappar och returnerar true om användaren klickat på någon av knapparna
        internal bool DrawMenu(MouseState mouseState, string buttonText, int leftPossX, int topPossY)
        {
            bool mouseOver = false;
            bool wasClicked = false;
            int buttonWidth = 200;
            int buttonHeight = 40;
            int margin = 10;

            if (mouseState.X > leftPossX - margin && mouseState.X < leftPossX + buttonWidth - margin &&
                mouseState.Y > topPossY - margin && mouseState.Y < topPossY + buttonHeight - margin)
            {
                mouseOver = true;

                if (mouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed)
                    wasClicked = true;
            }

            //Possition för texten
            Vector2 position = new Vector2(leftPossX, topPossY);

            //Possition för knappen
            Rectangle destinationRectangle = new Rectangle(leftPossX - margin, topPossY - margin, buttonWidth, buttonHeight);

            //Skapar en fejkad texture
            Texture2D rect = new Texture2D(graphics, buttonWidth, buttonHeight);
            Color[] data = new Color[buttonWidth * buttonHeight];
            for (int i = 0; i < data.Length; ++i) 
                data[i] = Color.White;

            rect.SetData(data);

            //Ritar ut knappen beroende på om muspekaren är över knappen eller inte, samt om man klickat på höger musknapp
            if (mouseOver)
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    sprite.Draw(rect, destinationRectangle, Color.LightGray);
                    sprite.DrawString(theFont, buttonText, position, Color.White);
                }
                else
                {
                    sprite.Draw(rect, destinationRectangle, Color.LightGray);
                    sprite.DrawString(theFont, buttonText, position, Color.Black);
                }
            }
            else
            {
                sprite.Draw(rect, destinationRectangle, Color.White);
                sprite.DrawString(theFont, buttonText, new Vector2(leftPossX, topPossY), Color.Gray);
            }

            return wasClicked;
        }

        internal void setOldState(MouseState state)
        {
            oldMouseState = state;
        }
    }
}
