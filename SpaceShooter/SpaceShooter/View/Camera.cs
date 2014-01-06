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

        //Variabler för marginaler i höjd eller bredd 
        //för om förnstret har en ojämn form
        private int widthMargin = 0;
        private int heightMargin = 0;

        private float backgroundPercentOfBoard;

        internal Camera(Viewport viewPort)
        {
            this.screenWidth = viewPort.Width;
            this.screenHeight = viewPort.Height;

            this.logicalWidth = XNAController.BOARD_LOGIC_WIDTH;
            this.logicalBoardWidth = XNAController.BOARD_LOGIC_GAMEFIELDWIDTH;

            this.scaleX = (float)screenWidth / logicalWidth;
            this.scaleY = (float)screenHeight / XNAController.BOARD_LOGIC_HEIGHT;

            this.backgroundPercentOfBoard = XNAController.BOARD_LOGIC_GAMEFIELDWIDTH / XNAController.BOARD_LOGIC_WIDTH;
            this.displacementX = (int)(((logicalBoardWidth - logicalWidth) / 2) * scaleX) * -1;

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

        internal Rectangle getVisualRectangle(float modelX, float modelY, float modelDimention)
        {
            return new Rectangle(
                                    (int)(modelX * scaleX) + (int)(widthMargin),
                                    (int)(modelY * scaleY) + (int)(heightMargin),
                                    (int)(modelDimention * scaleX),
                                    (int)(modelDimention * scaleY)
                                );
        }

        //(LOGISK KORDINAT * SKALA) + FÖRSKUTNING (Förskutning är om spelplanen är större än kamerans bredd)

        internal Rectangle getVisualRectangle(float modelX, float modelY, float modelDimentionHeight, float modelDimentionWidth)
        {
            return new Rectangle(
                                    (int)(modelX * scaleX) + (int)(widthMargin),
                                    (int)(modelY * scaleY) + (int)(heightMargin),
                                    (int)(modelDimentionHeight * scaleX),
                                    (int)(modelDimentionWidth * scaleY)
                                );

            //FÖR OM MAN SKA RÄKNA MED HALVA STORLEKEN I X OCH Y-LED... (Görs nu i PlayerSpaceShip ist)
            //(int)((modelX * scaleX) + (int)(widthMargin)) - (int)((modelDimentionHeight * scaleX) / 2),
            //(int)((modelY * scaleY) + (int)(heightMargin)) - (int)((modelDimentionWidth * scaleY) / 2),
        }

        internal Rectangle getExplotionRectangle(float modelX, float modelY, float modelDimention)
        {
            return new Rectangle(
                                    (int)((modelX * scaleX) + (int)(widthMargin)),
                                    (int)((modelY * scaleY) + (int)(heightMargin)),
                                    (int)(modelDimention * scaleX),
                                    (int)(modelDimention * scaleY)
                                );
        }

        internal Rectangle getVisualBackgroundRectangle(Rectangle speceShipRect, PlayerSpaceShip ship)
        {
            //if (speceShipRect.X > (logicalWidth * scaleX) * 0.7)
            //{
            //    if (speceShipRect.X < logicalBoardWidth * scaleX)
            //    {
            //        displaysementBySpaceship = displacementX + ((int)(speceShipRect.X - ((logicalWidth * scaleX) * 0.7)));
            //        speceShipRect.X = displaysementBySpaceship * -1;
            //    }
            //}
            //else if (speceShipRect.X < (logicalWidth * scaleX) * 0.3)
            //{
            //    if (speceShipRect.X > (speceShipRect.Width * -1))
            //    {
            //        int rightMargin = ((int)(speceShipRect.X - ((logicalWidth * scaleX) * 0.3)) * -1);
            //        displacementX = rightMargin;
            //        speceShipRect.X = displacementX;
            //    }
            //}

            //FIXAR SÅ KAMERAN FÖLJER SKEPPET (MAN BLIR ÅKSJUK :S)
            //int displaysementBySpaceship = (int)(displacementX + (ship.getPossitionX() * scaleX) - (screenWidth / 2) + (ship.SpaceShipWidth * scaleX / 2));

            //if (displaysementBySpaceship > 0)
            //    displaysementBySpaceship = 0;

            //if (displaysementBySpaceship < displacementX * 2)
            //    displaysementBySpaceship = displacementX * 2;

            //int displaysementBySpaceship = 0;

            //if (speceShipRect.X > screenWidth * 0.8)
            //{

            //}
            //else if (speceShipRect.X < screenWidth * 0.2)
            //{

            //}

            //return new Rectangle(
            //                        displacementX - displaysementBySpaceship,
            //                        0, 
            //                        (int)(screenWidth * backgroundPercentOfBoard), 
            //                        screenHeight
            //                    );

            return new Rectangle(
                                    0,
                                    0,
                                    (int)(logicalBoardWidth * scaleX),
                                    screenHeight
                                );
        }

        //Funktion som returnerar visuella kordinater för de logiska 
        //kordinater som tas som argument
        internal Vector2 getVisualCoordinates(float modelX, float modelY)
        {
            return new Vector2(widthMargin + (modelX * scaleX), heightMargin + (modelY * scaleY));
        }

        //Returnerar skalan
        internal int GetScale()
        {
            return (int)scaleX;
        }

        //Returnerar skalan i float
        internal float GetFloatScale()
        {
            return scaleX;
        }
    }
}
