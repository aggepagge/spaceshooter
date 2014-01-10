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
    class MenuGUI
    {
        private SpriteBatch sprite;
        private SpriteFont theFont;
        private MouseState oldMouseState;
        private GraphicsDevice graphics;

        internal MenuGUI(GraphicsDevice graphics, ContentManager content, SpriteBatch sprite)
        {
            this.graphics = graphics;
            this.sprite = sprite;
            theFont = content.Load<SpriteFont>("Titanium Motors");
        }

        internal void DrawTitle(string title, float size, int leftPossX, int topPossY)
        {
            Vector2 position = new Vector2(leftPossX, topPossY);
            sprite.DrawString(theFont, title, position, Color.White, 0, Vector2.Zero, size, SpriteEffects.None, 0.5f);
        }

        internal void DrawResult(string text, int leftPossX, int topPossY)
        {
            int margin = 10; 
            int buttonWidth = 400;
            int buttonHeight = 40;

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
