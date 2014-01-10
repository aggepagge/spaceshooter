using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter.View
{
    class MenuGUI
    {
        private SpriteBatch sprite;
        private ContentManager content;

        internal MenuGUI(GraphicsDevice graphDevice, ContentManager content, SpriteBatch sprite)
        {
            this.content = content;
            this.sprite = sprite;
        }

        internal bool DrawMenu(float elapsedTimeSeconds)
        {


            return true;
        }
    }
}
