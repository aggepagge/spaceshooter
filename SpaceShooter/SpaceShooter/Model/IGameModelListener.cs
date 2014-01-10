using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceShooter.Model
{
    public interface IGameModelListener
    {
        void wounded(Microsoft.Xna.Framework.Vector2 possition);

        void killed(Microsoft.Xna.Framework.Vector2 possition);

        void restartGame();
    }
}
