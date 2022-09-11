using System;

using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace No_Brainer
{
    public class OptionScene : Scene
    {
        ButtonGeneral b_off;
        ButtonGeneral b_on;

        ButtonGeneral b_back;

        MENU_STATE menu_state;

        public OptionScene(SceneManager manager)
        {
            this.SceneManager = manager;
        }

        public override void Initialize()
        {

        }

        public override void Activate()
        {
            pressed_touch = false;

            menu_state = MENU_STATE.MENU_CHOOSE;
        }

        public override void LoadContent()
        {
            b_off = new ButtonGeneral(new Rectangle(150, 120, 100, 100));
            b_on = new ButtonGeneral(new Rectangle(550, 120, 100, 100));

            b_back = new ButtonGeneral(new Rectangle(272, 340, 256, 64));
        }

        public override void UnloadContent()
        {
            b_off = null;
            b_on = null;
            b_back = null;
        }

        public override void Update(float dt)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                Back();
            }

            if (menu_state == MENU_STATE.MENU_CHOOSE)
            {

                for (byte i = 0; i < manager.Touches.Count; i++)
                {

                    if (manager.Touches[i].State == TouchLocationState.Pressed)
                    {
                        Pressed(manager.Touches[i].Position);
                    }
                    if (manager.Touches[i].State == TouchLocationState.Released)
                    {
                        Released(manager.Touches[i].Position);
                    }
                    if (manager.Touches[i].State == TouchLocationState.Moved)
                    {
                        Moved(manager.Touches[i].Position);
                    }
                }
            }

        }

        private void Pressed(Vector2 p)
        {
            if (b_back.Collide(p))
            {
                manager.PlayPress();
            }
            else if (b_off.Collide(p))
            {
                manager.SoundOn = false;
            }
            else if (b_on.Collide(p))
            {
                manager.SoundOn = true;
            }
        }
        private void Moved(Vector2 p)
        {
            b_back.Collide(p);

            if (b_off.Collide(p))
            {
                manager.SoundOn = false;
            }
            else if (b_on.Collide(p))
            {
                manager.SoundOn = true;
            }
        }
        private void Released(Vector2 p)
        {
            if (b_back.Collide(p))
            {
                Back();
                b_back.Pressed = false;
            }
            else if (b_off.Collide(p))
            {
                manager.SoundOn = false;
            }
            else if (b_on.Collide(p))
            {
                manager.SoundOn = true;
            }
        }

        public override void Draw()
        {
            manager.SpriteBatch.Begin();

            manager.SpriteBatch.Draw(manager.TMainBackground, manager.GraphicsDevice.Viewport.Bounds, Color.White);


            b_off.Draw(manager.SpriteBatch, manager.TCircleBig, Color.DarkGray);
            b_on.Draw(manager.SpriteBatch, manager.TCircleBig, Color.DarkGray);

            Vector2 pos = Vector2.Zero;

            pos.X = b_off.X - 15f;
            pos.Y = b_off.Y - 45f;

            manager.SpriteBatch.DrawString(manager.Font24, "Sound off", pos, Color.Black);

            pos.X = b_on.X - 15f;
            pos.Y = b_on.Y - 45f;

            manager.SpriteBatch.DrawString(manager.Font24, "Sound on", pos, Color.Black);

            Rectangle b = new Rectangle();

            if (manager.SoundOn)
            {
                b.X = b_on.X + 20;
                b.Y = b_on.Y + 20;
                b.Width = 60;
                b.Height = 60;

                manager.SpriteBatch.Draw(manager.TCircleSmall, b, Color.Green);
            }
            else
            {
                b.X = b_off.X + 20;
                b.Y = b_off.Y + 20;
                b.Width = 60;
                b.Height = 60;

                manager.SpriteBatch.Draw(manager.TCircleSmall, b, Color.Red);
            }

            if (b_back.Pressed)
            {
                b_back.Draw(manager.SpriteBatch, manager.TReady, Color.DarkGray);
            }
            else
            {
                b_back.Draw(manager.SpriteBatch, manager.TReady, Color.White);
            }

            manager.SpriteBatch.DrawString(manager.Font, "Return back", new Vector2(b_back.X + 60, b_back.Y + 10), Color.Black);

            manager.SpriteBatch.End();
        }

        private void Back()
        {
            LoadingScene s = manager.Scene(SCENE.SCENE_LOADING) as LoadingScene;

            if (s != null)
            {
                s.Switch(SCENE.SCENE_MAIN_MENU, SCENE.SCENE_CONTINUE, true);
            }
        }

    }
}
