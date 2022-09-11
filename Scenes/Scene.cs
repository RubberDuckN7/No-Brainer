using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;


using System.IO;

namespace No_Brainer
{
    public abstract class Scene
    {
        protected static SceneManager manager;

        protected Texture2D texture_background;

        protected byte id_form;

        protected bool pressed_touch;


        public SceneManager SceneManager
        {
            get { return manager; }
            internal set { manager = value; }
        }

        public Texture2D Background
        {
            get { return texture_background; }
            set { texture_background = value; }
        }

        public virtual void LoadContent()
        {

        }

        public virtual void UnloadContent()
        {

        }

        // Used first time, create instances
        public virtual void Initialize()
        {
            pressed_touch = false;

        }

        public virtual void Update(float dt)
        {

        }

        public virtual void Draw()
        {
            manager.GraphicsDevice.Clear(Color.Black);
        }

        public virtual void Activate()
        {

        }
    }
}
