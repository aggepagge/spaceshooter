using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceShooter.Model
{
    public interface IGameModelListener
    {
        void wounded(KeyValuePair<Vector2, Vector2> possition);

        void wounded(Vector2 possition);

        void killed(KeyValuePair<Vector2, Vector2> possition);

        void killed(Vector2 possition);

        void restartGame();

        void setNextLevel(int level);
    }
}
