using System;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;


namespace No_Brainer
{
    class LoadingScene : Scene
    {

        Rectangle b;

        float time;

        SCENE from;
        SCENE to;

        bool load_slow;

        public LoadingScene(SceneManager manager)
        {
            this.SceneManager = manager;
        }

        public void Switch(SCENE to, SCENE from, bool slow)
        {
            load_slow = slow;
            time = 0f;

            this.to = to;
            this.from = from;

            manager.ActivateScene(SCENE.SCENE_LOADING);

            if (!slow)
            {
                manager.ActivateScene(to);
                manager.Scene(to).Activate();
            }

            b = new Rectangle(-800, -800, 800, 800);
        }

        public override void Draw()
        {
            manager.SpriteBatch.Begin();

            Color c = Color.Black;

            if (time > 1f)
            {
                c.A = 255;
            }
            else
            {
                c.A = (byte)(time * 255.0f);
            }


            manager.SpriteBatch.Draw(manager.TMainBackground, manager.GraphicsDevice.Viewport.Bounds, Color.White);
            manager.SpriteBatch.Draw(manager.TPoint, manager.GraphicsDevice.Viewport.Bounds, c);

            manager.SpriteBatch.DrawString(manager.Font, "Loading scene", Vector2.Zero, Color.White);

            c = Color.Transparent;

            c.A = 25;

            manager.SpriteBatch.Draw(manager.TFlashScreen, b, c);

            manager.SpriteBatch.End();
        }

        public void Load()
        {

        }

        public override void Update(float dt)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                manager.ActivateScene(SCENE.SCENE_MAIN_MENU);
                manager.Scene(SCENE.SCENE_MAIN_MENU).Activate();
            }


            time += dt * 5f;

            b.X = (int)(MathHelper.Lerp(-800f, b.Width, time - 0.2f));
            b.Y = (int)(MathHelper.Lerp(-800f, 480, time - 0.2f));


            if (time > 0.9f)
            {
                manager.ActivateScene(to);
                manager.Scene(to).Activate();
            }
        }
    }
}
