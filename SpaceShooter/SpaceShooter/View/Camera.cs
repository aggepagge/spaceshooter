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

        internal Rectangle getPlayerVisualRectangle(float modelX, float modelY, float modelDimentionHeight, float modelDimentionWidth)
        {
            if (playerPrevPoss.X != -1 && playerPrevPoss.Y != -1)
            {
                if (!playerIsCloseToLeftEdge && !playerIsCloseToRightEdge)
                {
                    int diff = (int)((playerPrevPoss.X - modelX) * scaleX);
                    displacementX += diff;
                }

                if (playerIsCloseToLeftEdge && (modelX * scaleX) > screenWidth / 2)
                {
                    playerIsCloseToLeftEdge = false;
                }

                if (playerIsCloseToRightEdge && (modelX * scaleX) < displacementXStartPossition + (screenWidth / 2))
                {
                    playerIsCloseToRightEdge = false;
                }
            }

            playerPrevPoss.X = modelX;
            playerPrevPoss.Y = modelY;

            return new Rectangle(
                                    (int)(modelX * scaleX) + (int)(widthMargin) + displacementX, 
                                    (int)(modelY * scaleY) + (int)(heightMargin),
                                    (int)(modelDimentionWidth * scaleX),
                                    (int)(modelDimentionHeight * scaleY)
                                );
        }

        internal Rectangle getBoardVisualRectangle(float gameFieldTotalWidth)
        {
            if (displacementX > 0)
            {
                displacementX = 0;
                playerIsCloseToLeftEdge = true;
            }
            
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
            return new Vector2(widthMargin + (modelX * scaleX), heightMargin + (modelY * scaleY));
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

        internal void restartGame()
        {
            displacementX = displacementXStartPossition / 2 * -1;
            playerPrevPoss = new Vector2(-1, -1);
        }
    }
}