using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SpaceShooter.Model.GameComponents.Weapons.Weapon;
using SpaceShooter.Model.GameComponents.Weapons;

namespace SpaceShooter.Model.GameComponents.Ships
{
    /// <summary>
    /// Klass för användarens rymdskepp
    /// //Subklass till SpaceShip
    /// </summary>
    class PlayerSpaceShip : SpaceShip
    {
        internal bool Firering { get; set; }
        internal WeaponTypes CurrentWeapon { get; private set; }
        internal float FireRate { get; private set; }
        private bool CanFire { get; set; }
        private float timeUntillNextShoot = 0.0f;
        internal int PlayerScoore { get; set; }
        internal int PlayerStartHealt { get; private set; }

        //Lista med vapentyper (Används inte, men var tänkt som plats för olika vapen som spelaren
        //samlat på sig och som man skulle kunna välja mellan
        private List<WeaponTypes> weapons = new List<WeaponTypes>(10);

        //Konstruktor som tar variabler för denna klass och super-klassen
        internal PlayerSpaceShip(float height, float width, Level level, float speedX, float speedY, int healt) 
            : base (height, width, level, speedX, speedY, healt)
        {
            Firering = false;
            //Sätter raygun som start-vapen
            CurrentWeapon = WeaponTypes.Raygun;
            this.addWeapon(WeaponTypes.Raygun);
            //Hämtar skut-intervallet för aktuellt vapen
            FireRate = StaticHelper.getFireRate(CurrentWeapon);
            CanFire = false;
            PlayerScoore = 0;
            this.PlayerStartHealt = healt;
        }

        //Returnerar vector med mittpossition (X) samt toppen (Y)
        internal Vector2 getCenterTopPossition()
        {
            return new Vector2(spaceShipPossition.X + (SpaceShipWidth / 2), spaceShipPossition.Y);
        }

        //Sätter ny possition
        internal void setPossition(float possX, float possY)
        {
            spaceShipPossition.X = possX;
            spaceShipPossition.Y = possY;
        }

        //Sätter possition i Y-led efter farten har adderats.
        //Om moveDirection är possitivt så är rörelsen åt höger, negativ vänster
        internal void setPossitionY(float moveDirection = 1)
        {
            spaceShipPossition.Y += SpaceShipSpeed.Y * moveDirection;
        }

        //Sätter possition i X-led efter farten har adderats.
        //Om moveDirection är possitivt så är rörelsen åt nedåt, negativ uppåt
        internal void setPossitionX(float moveDirection = 1)
        {
            spaceShipPossition.X += SpaceShipSpeed.X * moveDirection;
        }

        //Hämtar ut skeppets mittpossition
        internal Vector2 spaceShipCenterPossition()
        {
            return spaceShipPossition;
        }

        //Adderar ett vapen till vapensamlingen
        internal void addWeapon(WeaponTypes newWepon)
        {
            weapons.Add(newWepon);
        }

        //Sätter nuvarande vapen och adderar det till samlingen av vapen
        internal void setCurrentWeapon(WeaponTypes currentWepon)
        {
            CurrentWeapon = currentWepon;
            if (!weapons.Contains(currentWepon))
                weapons.Add(currentWepon);
            //Hämtar skottintervallet för aktuellt vapen
            FireRate = StaticHelper.getFireRate(CurrentWeapon);
        }

        //Metod som tar bort ett specifikt vapen
        internal void removeWeapon(int weaponIndex)
        {
            if (weapons.Count > weaponIndex)
                weapons.RemoveAt(weaponIndex);
        }

        //Metod som tar bort ett specifikt vapen
        internal void removeWeapon(WeaponTypes weaponEnum)
        {
            weapons.Remove(weaponEnum);
        }

        //Metod som tar bort alla vapen och sätter raygun som enda vapen
        internal void removeAllWeapon()
        {
            weapons.Clear();
            weapons.Add(WeaponTypes.Raygun);
        }

        //Metod som returnerar om spelarens vapen ska skuta igen
        internal bool readyToFire()
        {
            bool ready = CanFire;
            CanFire = false;
            return ready;
        }

        //Returnerar aktuellt vapen
        internal WeaponTypes getCurrentWeapon()
        {
            if (weapons.Count > 0)
            {
                return CurrentWeapon;
            }
            else
            {
                weapons.Add(WeaponTypes.Raygun);
                return WeaponTypes.Raygun;
            }
        }

        //Stannar skeppet om det håller på att röra sig utanför spelplanen,
        //samt kollar om skeppet ska skuta igen (Bestäms av vapentypen)
        internal override void Update(float elapsedTimeSeconds)
        {
            if (spaceShipPossition.X < -(SpaceShipWidth / 2))
                spaceShipPossition.X = -(SpaceShipWidth / 2);

            if (spaceShipPossition.X > level.BoardTotalWidth - (SpaceShipWidth / 2))
                spaceShipPossition.X = level.BoardTotalWidth - (SpaceShipWidth / 2);

            if (spaceShipPossition.Y < -(SpaceShipHeight / 2))
                spaceShipPossition.Y = -(SpaceShipHeight / 2);

            if (spaceShipPossition.Y > level.BoardHeight - (SpaceShipHeight / 2))
                spaceShipPossition.Y = level.BoardHeight - (SpaceShipHeight / 2);

            if (Firering)
            {
                timeUntillNextShoot += elapsedTimeSeconds;
                if (timeUntillNextShoot > FireRate)
                {
                    CanFire = true;
                    timeUntillNextShoot = 0.0f;
                }
            }
            else if (timeUntillNextShoot != 0.0f)
                timeUntillNextShoot = 0.0f;

            //Sätter raygun som vapen om spelaren förlorat mer än hälften av hälsan
            if (this.Healt < this.PlayerStartHealt / 2)
            {
                this.setCurrentWeapon(WeaponTypes.Raygun);
            }
        }

        //Kollar om detta objekt har kolliderat med något annat
        internal override bool HasBeenShoot(FloatRectangle shotRect)
        {
            FloatRectangle shipRect = new FloatRectangle(
                                                            spaceShipPossition, 
                                                            new Vector2(spaceShipPossition.X + SpaceShipWidth,
                                                                        spaceShipPossition.Y + SpaceShipHeight)
                                                        );

            if (shipRect.isIntersecting(shotRect))
                return true;

            return false;
        }

        //Metod som sätter rymdskeppet utanför spelplanen
        //Anropas efter spelaren har dött så det inte sker några explotioner 
        //där spelarens rymdskepp är possitionerat (Även om det inte syns i vyn, så "lever" det i modellen)
        internal void setDead()
        {
            Firering = false;
            CanFire = false;
            base.SpaceShipHeight = 0.0f;
            base.SpaceShipWidth = 0.0f;
            spaceShipPossition.X = -1;
            spaceShipPossition.Y = -1;
        }

        //Återställer hälsan till full
        internal void setFullHealt()
        {
            base.Healt = PlayerStartHealt;
        }
    }
}
