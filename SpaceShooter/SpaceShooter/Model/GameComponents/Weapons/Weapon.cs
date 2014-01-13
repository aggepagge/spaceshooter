using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace SpaceShooter.Model.GameComponents.Weapons.Weapon
{
    //Enum för de olika vapentyperna som finns
    public enum WeaponTypes
    {
        Raygun,
        Missile,
        Plasma,
        EnemyRaygun,
        EnemyPlasma,
        EnemyLaser,
        EnemyBossPlasma
    }

    /// <summary>
    /// Klass som heter weapon men egentligen hanterar information om ett skott
    /// </summary>
    class Weapon
    {
        //Variabler för bl.a. possition och skada
        internal float PossitionX { get; set; }
        internal float PossitionY { get; set; }
        internal float Height { get; private set; }
        internal float Width { get; private set; }
        internal int Damage { get; set; }
        internal Vector2 FireSpeed { get; private set; }
        internal bool HeatSeeking { get; private set; }
        //bool variabler för om det är ett fiendeskott eller spelarskott (Används för att åka upp eller ner)
        internal bool EnemyWepon { get; private set; }
        internal bool RemoveMe { get; set; }
        internal WeaponTypes WeaponType { get; private set; }

        private float time = 0.0f;

        //Konstruktor som tar ett antal argument. heatSeeking används inte men var tänkt för missiler som är målsökande
        internal Weapon(Vector2 possition, WeaponTypes weaponType, float width, float height, int damage, Vector2 fireSpeed,
                        bool heatSeeking, bool enemyWepon)
        {
            this.PossitionX = possition.X - (width / 8);
            this.PossitionY = possition.Y;
            this.Width = width;
            this.Height = height;
            this.Damage = damage;
            this.FireSpeed = fireSpeed;
            this.HeatSeeking = heatSeeking;
            this.EnemyWepon = enemyWepon;

            WeaponType = weaponType;
            RemoveMe = false;
        }

        //Sätter ny possition. Räknar ut om skottet ska raderas basserat på om det är ett spelarskott eller fiendeskott samt possitioner för detta
        internal void setNewPossition(float xPoss, float yPoss)
        {
            PossitionX = xPoss; 
            PossitionY = yPoss;

            if ((PossitionY + Height < 0 && !EnemyWepon) || (PossitionY + Height > XNAController.BOARD_LOGIC_HEIGHT && EnemyWepon))
                RemoveMe = true;
        }

        //Uppdaterar possition (Basserat på om det är ett fiendeskott eller spelarskott
        internal void Update(float elapsedTimeSeconds)
        {
            time += elapsedTimeSeconds;

            float possX = (float)(elapsedTimeSeconds * FireSpeed.X);
            float possY = (float)(elapsedTimeSeconds * FireSpeed.Y);

            if (EnemyWepon)
            {
                PossitionX += possX;
                PossitionY += possY;
            }
            else
            {
                PossitionX += possX;
                PossitionY -= possY;
            }
        }
    }
}
