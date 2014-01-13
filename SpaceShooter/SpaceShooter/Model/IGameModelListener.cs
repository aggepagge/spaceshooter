using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceShooter.Model
{
    /// <summary>
    /// Interface för metoder där vyn måste lystna på modellen
    /// </summary>
    public interface IGameModelListener
    {
        //För om det ska skapas splitter på possitionen (Används inte)
        void wounded(KeyValuePair<Vector2, Vector2> possition);

        //För om det ska skapas splitter på possitionen
        void wounded(Vector2 possition);

        //För om det ska skapas explotioner på possitionen (Med två vectorer för att skapa explotions-gravitation)
        void killed(KeyValuePair<Vector2, Vector2> possition);

        //För om det ska skapas explotioner på possitionen
        void killed(Vector2 possition);

        //För att sätt saker till startpossition i vyn (T.ex. bakgrunden)
        void restartGame();

        //För att sätt saker till startpossition samt ändra bakgrundsbild för aktuell bana
        void setNextLevel(int level);
    }
}
