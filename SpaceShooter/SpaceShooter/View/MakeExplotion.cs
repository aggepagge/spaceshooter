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
    class MakeExplotion
    {
        private ExplotionSystem explotion;

        private Texture2D textureExplotion;
        private SoundEffectInstance explotionSoundInstance;

        internal MakeExplotion(Vector2 startPossition, float scale, SoundEffectInstance soundInstance, Texture2D explotion)
        {
            this.explotion = new ExplotionSystem(startPossition, scale);
            this.explotionSoundInstance = soundInstance;
            this.textureExplotion = explotion;

            setSound();
        }

        private void setSound()
        {
            explotionSoundInstance.Volume = 0.5f;
            explotionSoundInstance.Play();
        }

        internal void UpdateExplotion(float elapsedGameTime)
        {
            explotion.Update(elapsedGameTime);
        }

        internal void DrawExplotion(SpriteBatch spriteBatch, Camera camera)
        {
            explotion.Draw(spriteBatch, camera, textureExplotion);
        }
    }
}
