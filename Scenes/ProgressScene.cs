using System;

using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace No_Brainer
{
    class ProgressScene : Scene
    {
        ButtonGeneral b_back;

        MENU_STATE menu_state;

        public ProgressScene(SceneManager manager)
        {
            this.SceneManager = manager;
        }


        public override void LoadContent()
        {
            b_back = new ButtonGeneral(new Rectangle(0, 0, 128, 64));
        }

        public override void UnloadContent()
        {
            b_back = null;
        }

        public override void Activate()
        {
            menu_state = MENU_STATE.MENU_CHOOSE;
            b_back.Pressed = false;
        }

        public override void Initialize()
        {

        }

        public override void Draw()
        {
            if (menu_state == MENU_STATE.MENU_CHOOSE)
            {
                DrawChoose();
            }
            else if (menu_state == MENU_STATE.MENU_TRANSIT)
            {
                DrawTransit();
            }
        }

        private void DrawChoose()
        {
            manager.SpriteBatch.Begin();

            Rectangle _b = new Rectangle();

            _b.X = 5;
            _b.Y = 2;

            _b.Width = 790;
            _b.Height = 476;

            manager.SpriteBatch.Draw(manager.TBoxTransparent, _b, Color.White);

            _b.Width = 760;
            _b.Height = 460;


            _b.X = 20;
            _b.Y = 60;

            _b.Width = 760;
            _b.Height = 2;

            for (int i = 0; i < 5; i++)
            {
                manager.SpriteBatch.Draw(manager.TPoint, _b, Color.Black);

                _b.Y += 92;
            }

            _b.X = 50;// 115 - 10;
            _b.Y = _b.Height - 10;

            _b.Width = 5;
            _b.Height = 5;

            Vector2 prev = new Vector2(20, 422);
            Vector2 next;

            Vector3 axis = new Vector3(0f, 1f, 0f);
            Vector3 dir;

            float scale = (float)(60.0f / (float)manager.GetStatistic.DaysPlayed);

            for (int d = 0; d < manager.GetStatistic.DaysPlayed; d++)
            {
                float accamulate = 0f;

                if(d > 0)
                    prev = new Vector2((int)(_b.X + 2.5f), (int)(_b.Y + 2.5f));

                _b.Y = 422;
                _b.X += (int)(12.0f * scale);

                for (int g = 0; g < manager.GetStatistic.GameCount(d); g++)
                {
                    int __id = manager.GetStatistic.GameID(d, g);
                    float __first_score = manager.GetStatistic.FirstScore(__id);

                    if (__first_score == 0f)
                        accamulate += 1f;
                    else
                        accamulate += (manager.GetStatistic.Score(d, g) / __first_score);
                }

                _b.Y -= (int)(accamulate * 15.20f);

                Rectangle point = _b;

                next = new Vector2((int)(_b.X + 2.5f), (int)(_b.Y + 2.5f));

                dir = new Vector3(_b.X, _b.Y, 0f);
                float lenght = dir.Length();
                dir.Normalize();

                dir = new Vector3(next.X, next.Y, 0f) - new Vector3(prev.X, prev.Y, 0f);
                lenght = dir.Length();
                dir.Normalize();

                double a = 2 * MathHelper.Pi - Math.Acos(Vector3.Dot(axis, dir));

                Rectangle line = new Rectangle((int)prev.X, (int)prev.Y, 3, (int)lenght);

                lenght -= prev.Length();

                manager.SpriteBatch.Draw(manager.TPoint, line, null, Color.Blue, (float)a, Vector2.Zero, SpriteEffects.None, 0f);
                manager.SpriteBatch.Draw(manager.TCircleSmall, point, Color.Green);
            }

            if(b_back.Pressed)
                b_back.Draw(manager.SpriteBatch, manager.TRectangle, Color.DarkGray);
            else
                b_back.Draw(manager.SpriteBatch, manager.TRectangle, Color.White);

            manager.SpriteBatch.DrawString(manager.Font, "Back", new Vector2(20, 10), Color.White);

            manager.SpriteBatch.End();

        }

        private void DrawTransit()
        {

        }

        public override void Update(float dt)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                Back();
            }

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

        private void Pressed(Vector2 p)
        {
            if (b_back.Collide(p))
            {
                manager.PlayPress();
            }
        }

        private void Moved(Vector2 p)
        {
            b_back.Collide(p);
        }

        private void Released(Vector2 p)
        {
            if (b_back.Collide(p))
            {
                Back();
            }
        }

        private void Back()
        {
            LoadingScene s = manager.Scene(SCENE.SCENE_LOADING) as LoadingScene;

            if (s != null)
            {
                s.Switch(SCENE.SCENE_MAIN_MENU, SCENE.SCENE_PROGRESS, true);
            }
        }

    }
}
