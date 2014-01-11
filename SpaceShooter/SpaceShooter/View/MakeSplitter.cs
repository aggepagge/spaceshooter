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
    class MakeSplitter
    {
        private SplitterSystem splitter;

        private Texture2D textureSplitter;
        private SoundEffectInstance explotionSoundInstance;

        internal MakeSplitter(Vector2 startPossition, float scale, SoundEffectInstance soundInstance, Texture2D splitter)
        {
            this.splitter = new SplitterSystem(startPossition, scale);
            this.explotionSoundInstance = soundInstance;
            this.textureSplitter = splitter;

            setSound();
        }

        private void setSound()
        {
            explotionSoundInstance.Volume = 0.5f;
            explotionSoundInstance.Play();
        }

        internal void pauseSound()
        {
            explotionSoundInstance.Pause();
        }

        internal void resumeSound()
        {
            explotionSoundInstance.Resume();
        }

        internal void UpdateSplitter(float elapsedGameTime)
        {
            splitter.Update(elapsedGameTime);
        }

        internal void DrawSplitter(SpriteBatch spriteBatch, Camera camera)
        {
            splitter.Draw(spriteBatch, camera, textureSplitter);
        }
    }
}
