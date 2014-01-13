using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SpaceShooter.Model.GameComponents;
using SpaceShooter.Model.GameComponents.Ships;

namespace SpaceShooter.View
{
    /// <summary>
    /// Klass som omvadlar logisk data till visuella possitioner 
    /// </summary>
    class Camera
    {
        //Variabler för visuell bredd och höjd
        private int screenWidth;
        private int screenHeight;

        //Variabler för uträkning av skalan
        private float scaleX;
        private float scaleY;

        private float logicalWidth;
        private float logicalBoardWidth;

        private int displacementX;
        private int displacementXStartPossition;
        private Vector2 playerPrevPoss;
        private bool playerIsCloseToLeftEdge = false;
        private bool playerIsCloseToRightEdge = false;

        //Variabler för marginaler i höjd eller bredd 
        //för om förnstret har en ojämn form
        private int widthMargin = 0;
        private int heightMargin = 0;

        internal Camera(Viewport viewPort)
        {
            this.screenWidth = viewPort.Width;
            this.screenHeight = viewPort.Height;

            this.logicalWidth = XNAController.BOARD_LOGIC_WIDTH;
            this.logicalBoardWidth = XNAController.BOARD_LOGIC_GAMEFIELDWIDTH;

            this.scaleX = (float)screenWidth / logicalWidth;
            this.scaleY = (float)screenHeight / XNAController.BOARD_LOGIC_HEIGHT;

            this.displacementXStartPossition = (int)((logicalBoardWidth - logicalWidth) * scaleX);
            this.displacementX = displacementXStartPossition / 2 * -1;
            this.playerPrevPoss = new Vector2(-1, -1);

            //Sätter höjd och bredd att vara densamma
            if (scaleY < scaleX)
            {
                scaleX = scaleY;
            }
            else if (scaleY > scaleX)
            {
                scaleY = scaleX;
            }

            if (screenHeight < screenWidth)
            {
                widthMargin = (screenWidth - screenHeight) / 2;
            }
            else if (screenHeight > screenWidth)
            {
                heightMargin = (screenHeight - screenWidth) / 2;
            }
        }

        //Metod som anropas för spelarens skepp. När spelarens skepp är i mitten av spelplanen (Banans totala bredd) och bakgrunden
        //inte har nått någon av sina kanter så står spelarens skepp stilla medans bakgrunden och alla spel-element flyttas med
        //displacementX åt höger eller vänster (motsatt till spelarens rörelse)
        //Men om bakgrunden nått längst till vänster eller höger så står bakgrunden still och spelarens skepp och alla spel-element
        //rör sig "normalt". När spelaren rör sig in i banan igen så ska inte bakgrunden förskutas förens spelaren har kommet till halva
        //"camera-bredden" (Visuella centrum). Detta uppnås med de två boolska variablerna playerIsCloseToLeftEdge och playerIsCloseToRightEdge.
        internal Rectangle getPlayerVisualRectangle(float modelX, float modelY, float modelDimentionHeight, float modelDimentionWidth)
        {
            //Kollar så inte possitionen är i startpossition (Detta för det behövs en possition från början)
            if (playerPrevPoss.X != -1 && playerPrevPoss.Y != -1)
            {
                //Räknar ut displaysment för bakgrund och alla element så de 
                //flyttas åt rätt håll
                if (!playerIsCloseToLeftEdge && !playerIsCloseToRightEdge)
                {
                    int diff = (int)((playerPrevPoss.X - modelX) * scaleX);
                    displacementX += diff;
                }

                //Om spelaren rört sig in i banan igen från vänster och kommit över halva "camera-bredden" (Visuella centrum)
                //så ska displaysement återigen adderas
                if (playerIsCloseToLeftEdge && (modelX * scaleX) > screenWidth / 2)
                {
                    playerIsCloseToLeftEdge = false;
                }

                //Om spelaren rört sig in i banan igen från höger och kommit över halva "camera-bredden" (Visuella centrum) +
                //totala displaysement (camera-bredden + totala displaysement) så ska displaysement återigen adderas
                if (playerIsCloseToRightEdge && (modelX * scaleX) < displacementXStartPossition + (screenWidth / 2))
                {
                    playerIsCloseToRightEdge = false;
                }
            }

            //Sätter tidigare possition till spelarens angivna possition. Om spelaren inte rört sig mot kanterna så
            //bakgrunden stannar, så ska spelarens skepp stå stilla och då sätts possitionerna till föregående possition
            playerPrevPoss.X = modelX;
            playerPrevPoss.Y = modelY;

            return new Rectangle(
                                    (int)(modelX * scaleX) + (int)(widthMargin) + displacementX, 
                                    (int)(modelY * scaleY) + (int)(heightMargin),
                                    (int)(modelDimentionWidth * scaleX),
                                    (int)(modelDimentionHeight * scaleY)
                                );
        }

        //Metod som skapar en rektangel för bakgrunden. Om bakgrunden nått längst till vänster eller höger
        //så sätts displaysement till 0, och playerIsCloseToLeftEdge eller playerIsCloseToRightEdge sätts till true
        //beroende på åt vilken kant spelaren rör sig
        internal Rectangle getBoardVisualRectangle(float gameFieldTotalWidth)
        {
            //Om längst till vänster
            if (displacementX > 0)
            {
                displacementX = 0;
                playerIsCloseToLeftEdge = true;
            }

            //Om längst till höger
            if (displacementX < displacementXStartPossition * -1)
            {
                displacementX = displacementXStartPossition * -1;
                playerIsCloseToRightEdge = true;
            }

            return new Rectangle(
                                    displacementX,
                                    0,
                                    (int)(logicalWidth * scaleX * gameFieldTotalWidth),
                                    screenHeight
                                );
        }

        //Metod för alla övriga element (standard-metoden) som skapar en rektangle där elementet ska ritas ut.
        //displacementX adderas i X-led för förskutning i förhållande till spelarens skepp
        internal Rectangle getVisualRectangle(float modelX, float modelY, float modelDimentionHeight, float modelDimentionWidth)
        {
            return new Rectangle(
                                    (int)(modelX * scaleX) + (int)(widthMargin) + displacementX,
                                    (int)(modelY * scaleY) + (int)(heightMargin),
                                    (int)(modelDimentionWidth * scaleX),
                                    (int)(modelDimentionHeight * scaleY)
                                );
        }

        //Funktion som returnerar visuella kordinater för de logiska 
        //kordinater som tas som argument
        internal Vector2 getVisualCoordinates(float modelX, float modelY)
        {
            return new Vector2(widthMargin + (modelX * scaleX) + displacementX, heightMargin + (modelY * scaleY));
        }

        //Returnerar skalan i int
        internal int GetScale()
        {
            return (int)scaleX;
        }

        //Returnerar skalan i float
        internal float GetFloatScale()
        {
            return scaleX;
        }

        //Possitionerar displaysement till startpossition och sätter tidigare possition till -1 (X och Y-led)
        internal void restartGame()
        {
            displacementX = displacementXStartPossition / 2 * -1;
            playerPrevPoss = new Vector2(-1, -1);
        }
    }
}