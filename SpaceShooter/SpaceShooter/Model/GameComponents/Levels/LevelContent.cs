using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SpaceShooter.Model.GameComponents.Ships;

namespace SpaceShooter.Model.GameComponents.Levels
{
    class LevelContent
    {
        List<EnemySpaceShip> enemies;
        List<List<Vector2>> curves;

        internal LevelContent()
        {

        }
    }
}
