using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceShooter.View.Particles;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace SpaceShooter.View
{
    /// <summary>
    /// Klass som kapslar in ett Explotion-objekt och hanterar ljud för explotionen
    /// </summary>
    class MakeExplotion
    {
        private ExplotionSystem explotion;

        private Texture2D textureExplotion;
        private SoundEffectInstance explotionSoundInstance;

        //Konstruktor som tar possition, storlek, en ljudinstans och texturen för explotionen
        internal MakeExplotion(Vector2 startPossition, float scale, SoundEffectInstance soundInstance, Texture2D explotion)
        {
            this.explotion = new ExplotionSystem(startPossition, scale);
            this.explotionSoundInstance = soundInstance;
            this.textureExplotion = explotion;

            setSound();
        }

        //Alternativ konstruktor som tar nuvarande och tidigare possition, storlek, en ljudinstans och texturen för explotionen
        internal MakeExplotion(KeyValuePair<Vector2, Vector2> currentAndPreviousPossition, float scale, SoundEffectInstance soundInstance, Texture2D explotion)
        {
            this.explotion = new ExplotionSystem(currentAndPreviousPossition, scale);
            this.explotionSoundInstance = soundInstance;
            this.textureExplotion = explotion;

            setSound();
        }

        //Ställer in ljudet
        private void setSound()
        {
            explotionSoundInstance.Volume = 0.2f;
            explotionSoundInstance.Play();
        }

        //Pausar ljuduppspelningen
        internal void pauseSound()
        {
            if (explotionSoundInstance.State == SoundState.Playing)
                explotionSoundInstance.Pause();
        }

        //Fortsätter ljuduppspelningen
        internal void resumeSound()
        {
            if (explotionSoundInstance.State == SoundState.Paused)
                explotionSoundInstance.Resume();
        }

        //Uppdaterar explotionssystemet
        internal void UpdateExplotion(float elapsedGameTime)
        {
            explotion.Update(elapsedGameTime);
        }

        //Anropar explotionssystemet så den kan sköta sin utritning
        internal void DrawExplotion(SpriteBatch spriteBatch, Camera camera)
        {
            explotion.Draw(spriteBatch, camera, textureExplotion);
        }
    }
}
