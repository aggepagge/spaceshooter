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
    /// Klass som kappslar in ett SplitterSystem-objekt och hanterar ljud för splitter
    /// </summary>
    class MakeSplitter
    {
        private SplitterSystem splitter;

        private Texture2D textureSplitter;
        private SoundEffectInstance splitterSoundInstance;

        //Konstruktor som tar possition, storlek, en ljudinstans och texturen för splittret
        internal MakeSplitter(Vector2 startPossition, float scale, SoundEffectInstance soundInstance, Texture2D splitter)
        {
            this.splitter = new SplitterSystem(startPossition, scale);
            this.splitterSoundInstance = soundInstance;
            this.textureSplitter = splitter;

            setSound();
        }

        //Ställer in ljudet
        private void setSound()
        {
            splitterSoundInstance.Volume = 0.2f;
            splitterSoundInstance.Play();
        }

        //Pausar ljuduppspelningen
        internal void pauseSound()
        {
            if (splitterSoundInstance.State == SoundState.Playing)
                splitterSoundInstance.Pause();
        }

        //Fortsätter ljuduppspelningen
        internal void resumeSound()
        {
            if (splitterSoundInstance.State == SoundState.Paused)
                splitterSoundInstance.Resume();
        }

        //Uppdaterar splittersystemet
        internal void UpdateSplitter(float elapsedGameTime)
        {
            splitter.Update(elapsedGameTime);
        }

        //Anropar splittersystemet så den kan sköta sin utritning
        internal void DrawSplitter(SpriteBatch spriteBatch, Camera camera)
        {
            splitter.Draw(spriteBatch, camera, textureSplitter);
        }
    }
}
